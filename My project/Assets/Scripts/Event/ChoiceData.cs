[System.Serializable]
public class ChoiceData
{
    public string ChoiceID;         // 선택지 고유 번호 (예: CH_001)
    public string ChoiceName;       // 버튼에 들어갈 텍스트 (예: 즉시 사과)
    public string Description;      // 선택지 부연 설명 (유추할 힌트)
    
    // 5대 지표 변화량 (기본값 0, 깎이면 마이너스 입력)
    public int StatChange_User;     // 유저 수 변화
    public int StatChange_Public;   // 커뮤니티 여론 변화
    public int StatChange_Server;   // 서버 안정도 변화
    public int StatChange_Dev;      // 개발팀 체력 변화
    public int StatChange_Budget;   // 운영 예산 변화
    
    // 결과 처리용
    public string ResultFlag;       // 새로 생성할 내부 플래그 (예: 삭제글 박제)
    public string ResultComment;    // 선택 후 화면에 띄울 유저 반응/댓글 텍스트
}