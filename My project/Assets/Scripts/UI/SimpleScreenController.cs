// 유니티 화면 전환(패널) 테스트용 임시 코드인 점 참고 바랍니다.

using System;
using System.Collections;
using UnityEngine;

public class SimpleScreenController : MonoBehaviour
{
    [SerializeField] private GameObject titlePanel;
    [SerializeField] private GameObject dayPanel;
    [SerializeField] private GameObject eventPanel;
    [SerializeField] private GameObject reactionPanel;
    [SerializeField] private GameObject resultPanel;

    private Coroutine autoMoveCoroutine;

    private void Start()
    {
        ShowTitlePanel();
    }

    public void ShowTitlePanel()
    {
        ShowOnly(titlePanel);
    }

    public void ShowDayPanel()
    {
        ShowOnly(dayPanel);
        StartAutoMove(5f, ShowEventPanel);
    }

    public void ShowEventPanel()
    {
        ShowOnly(eventPanel);
    }

    public void ShowReactionPanel()
    {
        ShowOnly(reactionPanel);
        StartAutoMove(7f, ShowResultPanel);
    }

    public void ShowResultPanel()
    {
        ShowOnly(resultPanel);
    }

    public void OnTitleButtonClicked()
    {
        ShowDayPanel();
    }

    public void OnEventButtonClicked()
    {
        ShowReactionPanel();
    }

    public void OnResultButtonClicked()
    {
        ShowDayPanel();
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void ShowOnly(GameObject targetPanel)
    {
        StopAutoMove();

        titlePanel.SetActive(false);
        dayPanel.SetActive(false);
        eventPanel.SetActive(false);
        reactionPanel.SetActive(false);
        resultPanel.SetActive(false);

        targetPanel.SetActive(true);
    }

    private void StartAutoMove(float seconds, Action nextScreen)
    {
        StopAutoMove();
        autoMoveCoroutine = StartCoroutine(AutoMoveAfterSeconds(seconds, nextScreen));
    }

    private IEnumerator AutoMoveAfterSeconds(float seconds, Action nextScreen)
    {
        yield return new WaitForSeconds(seconds);
        nextScreen?.Invoke();
    }

    private void StopAutoMove()
    {
        if (autoMoveCoroutine == null)
        {
            return;
        }

        StopCoroutine(autoMoveCoroutine);
        autoMoveCoroutine = null;
    }
}