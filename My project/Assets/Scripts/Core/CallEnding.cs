using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CallEnding : MonoBehaviour
{
    public enum EndingStatType
    {
        User,
        Public,
        Server,
        Dev,
        Budget
    }

    [SerializeField] private Image endingImage;
    [SerializeField] private TMP_Text endingTitle;
    [SerializeField] private TMP_Text endingBody;
    [SerializeField] private GameManager gamemanager;

    public struct EndingData
    {
        public EndingStatType StatType;
        public string ImageId;
        public string Title;
        public string Body;
    }

    public void ShowEnding(string imageID, string title, string body)
    {
        Sprite sprite = Resources.Load<Sprite>($"Icon_Images/{imageID}");

        if (sprite != null)
        {
            endingImage.sprite = sprite;
        }
        else
        {
            Debug.LogWarning($"Ending image not found: {imageID}");
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
            endingImage.sprite = null;
            Debug.LogWarning($"Good ending image not found: {imageID}");
        }

        endingTitle.text = "승리!";
        endingBody.text = "아무튼 서비스는 이어질 겁니다.";
    }

    public void PrintEnding(GameStats gamestats)
    {
        if (gamemanager != null && gamemanager.IsGameOver)
        {
            if (TryGetBadEndingData(gamestats, out EndingData endingData))
            {
                ShowEnding(endingData.ImageId, endingData.Title, endingData.Body);
            }
        }
        else
        {
            ShowEnding_Good("imageid");
        }
    }

    public bool TryGetBadEndingData(GameStats gamestats, out EndingData endingData)
    {
        endingData = default;

        if (gamestats == null)
        {
            return false;
        }

        if (gamestats.User <= 0)
        {
            endingData = new EndingData
            {
                StatType = EndingStatType.User,
                ImageId = "user_ending_zero",
                Title = "소통의 종료",
                Body = "게임은 끝났지만, 작업을 함께하는 동료는 없었습니다."
            };
            return true;
        }

        if (gamestats.Public <= 0)
        {
            endingData = new EndingData
            {
                StatType = EndingStatType.Public,
                ImageId = "public_ending_zero",
                Title = "여론 붕괴",
                Body = "공식 공지보다 너무 늦게 도착한 안내문이 여론을 흔들기 시작했습니다."
            };
            return true;
        }

        if (gamestats.Server <= 0)
        {
            endingData = new EndingData
            {
                StatType = EndingStatType.Server,
                ImageId = "server_ending_zero",
                Title = "서버 다운",
                Body = "작동하는 서버가 없어서 게임은 더 이상 진행될 수 없습니다."
            };
            return true;
        }

        if (gamestats.Dev <= 0)
        {
            endingData = new EndingData
            {
                StatType = EndingStatType.Dev,
                ImageId = "dev_ending_zero",
                Title = "개발자 붕괴",
                Body = "다음 수정은 없고, 화면은 더 이상 아름답지 않았습니다."
            };
            return true;
        }

        if (gamestats.Budget <= 0)
        {
            endingData = new EndingData
            {
                StatType = EndingStatType.Budget,
                ImageId = "budget_ending_zero",
                Title = "예산 고갈",
                Body = "예산 계획은 끝났지만, 진행할 돈은 남아있지 않았습니다."
            };
            return true;
        }

        return false;
    }
}
