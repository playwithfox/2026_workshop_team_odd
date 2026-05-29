using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CallEnding : MonoBehaviour
{
    [SerializeField] private Image endingImage;
    [SerializeField] private TMP_Text endingTitle;
    [SerializeField] private TMP_Text endingBody;
    [SerializeField] private GameManager gamemanager;

    public void ShowEnding(string imageID, string title, string body)
    {
        Sprite sprite = Resources.Load<Sprite>($"Icon_Images/{imageID}");

        if (sprite != null)
        {
            endingImage.sprite = sprite;
        }
        else
        {
            Debug.LogWarning($"엔딩 이미지를 찾을 수 없음: {imageID}");
        }

        endingTitle.text = title;
        endingBody.text = body;
    }
    public void ShowEnding_Good(string imageID)
    {
        Sprite sprite = Resources.Load<Sprite>($"EndingImages/{imageID}");

        if (sprite != null)
        {
            endingImage.sprite = sprite;
        }
        else
        {
            Debug.LogWarning($"엔딩 이미지를 찾을 수 없음: {imageID}");
        }

        endingTitle.text = "승리!";
        endingBody.text = "'아무튼 서비스는 이어질 겁니다.'";
    }
    public void PrintEnding(GameStats gamestats)
    {
        if (gamemanager.IsGameOver){
            if (gamestats.User <= 0)
            {
                ShowEnding("유저 수/0명/1779701060510", "서비스 종료", "게임은 켜져 있지만, 접속하는 사람은 없었습니다.");
            }
            else if (gamestats.Public <= 0)
            {
                ShowEnding("커뮤니티 신뢰/0단계(최하)/1779701068123", "신뢰 붕괴", "공식 공지보다 유저 캡처본이 더 신뢰받기 시작했습니다.");
            }
            else if (gamestats.Server <= 0)
            {
                ShowEnding("서버 상태/0칸/1779701069028", "서버 다운", "접속할 수 없는 게임은 운영할 수도 없습니다.");
            }
            else if (gamestats.Dev <= 0)
            {
                ShowEnding("개발팀/0단계(최하)/1779701096913", "개발팀 붕괴", "다음 패치는 더 이상 올라오지 않았습니다.");
            }
            else if (gamestats.Budget <= 0)
            {
                ShowEnding("운영 예산/0단계/1779701103445", "예산 고갈", "운영 계획은 남았지만, 집행할 돈이 없었습니다.");
            }
        }
        else
        {
            ShowEnding_Good("imageid");
        }
    }
}