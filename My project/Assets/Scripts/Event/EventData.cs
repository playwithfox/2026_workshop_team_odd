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
    public string ConditionStat;    // 지표 조건 (예: 커뮤니티 여론 20 이하)
    
    // UI 및 후속 데이터 연결용
    public List<string> ActivePopups; // 강조될 지표 팝업 리스트 (예: ["UserAnalysis", "CommunityBoard"])
    public List<string> ChoiceIDs;    // 이 사건에 달릴 선택지 ID 리스트 (예: ["CH_001", "CH_002"])
}

[System.Serializable]
public class EventListData
{
    public List<EventData> events;
    public List<ChoiceData> choices;
}
