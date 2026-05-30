// 이미지 그라데이션
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(Graphic))]
public class ImageGradient : BaseMeshEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float leftAlpha = 0f;

    [Range(0f, 1f)]
    [SerializeField] private float centerAlpha = 1f;

    [Range(0f, 1f)]
    [SerializeField] private float gradientEndPoint = 0.5f;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected void OnValidate()
    {
        //base.OnValidate();
        gradientEndPoint = Mathf.Clamp01(gradientEndPoint);
        leftAlpha = Mathf.Clamp01(leftAlpha);
        centerAlpha = Mathf.Clamp01(centerAlpha);
    }

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive() || vh == null)
        {
            return;
        }

        List<UIVertex> vertices = new List<UIVertex>();
        vh.GetUIVertexStream(vertices);

        if (vertices.Count == 0)
        {
            return;
        }

        Rect rect = graphic.rectTransform.rect;
        float left = rect.xMin;
        float right = rect.xMax;
        float center = Mathf.Lerp(left, right, gradientEndPoint);
        float range = Mathf.Max(0.0001f, center - left);

        for (int i = 0; i < vertices.Count; i++)
        {
            UIVertex vertex = vertices[i];
            float x = vertex.position.x;
            float t = Mathf.Clamp01((x - left) / range);
            float alpha = x <= center
                ? Mathf.Lerp(leftAlpha, centerAlpha, t)
                : centerAlpha;

            Color32 color = vertex.color;
            color.a = (byte)Mathf.RoundToInt(color.a * alpha);
            vertex.color = color;
            vertices[i] = vertex;
        }

        vh.Clear();
        vh.AddUIVertexTriangleStream(vertices);
    }
}
