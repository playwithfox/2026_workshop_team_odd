using System.Collections.Generic;

[System.Serializable]
public class EventData
{
    public string EventID;          // 사건 고유 번호 (예: EVT_001)
    public string Phase;            // 등장 구간 (예: 전반부)
    public string Title;            // 사건 제목 (예: 공식 SNS 말실수)
    public string Description;      // 사건 상세 설명 텍스트
    public string ImageID;          // 연결될 이미지 파일명 (예: img_event_sns_01)

    // 등장 조건 필터링용
    public string ConditionFlag;    // 필요 플래그 (없으면 "None")
    public string ConditionStat;    // 사람이 읽는 지표 조건 (예: 커뮤니티 여론 20 이하)
    public string ConditionStatType; // 코드 판정용 지표 키 (예: Public, Server)
    public string ConditionOperator; // 코드 판정용 비교 연산자 (예: <=, >=)
    public int ConditionValue;       // 코드 판정용 기준값

    // UI 및 후속 데이터 연결용
    public List<string> ActivePopups = new List<string>(); // 강조될 지표 팝업 리스트 (예: ["UserAnalysis", "CommunityBoard"])
    public List<string> ChoiceIDs = new List<string>();    // 이 사건에 달릴 선택지 ID 리스트 (예: ["CH_001", "CH_002"])

    public bool IsAvailable(GameStats stats)
    {
        if (stats == null)
        {
            return false;
        }

        return IsFlagConditionMet(stats) && IsStatConditionMet(stats);
    }

    public bool IsFlagConditionMet(GameStats stats)
    {
        if (string.IsNullOrEmpty(ConditionFlag) || ConditionFlag == "None")
        {
            return true;
        }

        return stats.Flags != null && stats.Flags.Contains(ConditionFlag);
    }

    public bool IsStatConditionMet(GameStats stats)
    {
        if (string.IsNullOrEmpty(ConditionStatType) || ConditionStatType == "None")
        {
            return true;
        }

        int currentValue;
        switch (ConditionStatType)
        {
            case "User":
                currentValue = stats.User;
                break;
            case "Public":
                currentValue = stats.Public;
                break;
            case "Server":
                currentValue = stats.Server;
                break;
            case "Dev":
                currentValue = stats.Dev;
                break;
            case "Budget":
                currentValue = stats.Budget;
                break;
            default:
                return false;
        }

        switch (ConditionOperator)
        {
            case "<":
                return currentValue < ConditionValue;
            case "<=":
                return currentValue <= ConditionValue;
            case ">":
                return currentValue > ConditionValue;
            case ">=":
                return currentValue >= ConditionValue;
            case "==":
                return currentValue == ConditionValue;
            default:
                return false;
        }
    }
}

[System.Serializable]
public class EventListData
{
    public List<EventData> events = new List<EventData>();
    public List<ChoiceData> choices = new List<ChoiceData>();
}
