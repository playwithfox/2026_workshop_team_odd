using System.Collections.Generic; // Dictionary 같은 컬렉션 자료구조를 사용하기 위해 필요합니다.
using UnityEngine; // MonoBehaviour, SerializeField, Random, Mathf 같은 Unity 기능을 사용하기 위해 필요합니다.
public class GameManager : MonoBehaviour // Unity 오브젝트에 붙일 수 있는 게임 진행 관리자 클래스입니다.
{ // GameManager 클래스의 시작입니다.
    
#region Game State
    [Header("Game Settings")] // Inspector에서 게임 설정 항목을 보기 좋게 구분해 줍니다.
    [SerializeField] private int maxDay = 7; // 게임이 몇 일차까지 진행되는지 정합니다.
    [Header("Stats")] // Inspector에서 지표 관련 항목을 보기 좋게 구분해 줍니다.
    [SerializeField] private StatData[] stats; // Unity Inspector에서 연결할 지표 데이터 목록입니다.
    [SerializeField] private StatData activeuser;
    [SerializeField] private StatData community_trust;
    [SerializeField] private StatData serverstability;
    [SerializeField] private StatData devstamina;
    [SerializeField] private StatData operationbudget;
    private readonly Dictionary<StatData, int> currentStatValues = new(); // 각 지표의 현재 수치를 저장하는 딕셔너리입니다.
    private int currentDay = 1; // 현재 날짜를 저장합니다.
    private bool isGameOver; // 게임 오버 상태인지 저장합니다.
    public int CurrentDay => currentDay; // 다른 스크립트가 현재 날짜를 읽을 수 있게 합니다.
    public bool IsGameOver => isGameOver; // 다른 스크립트가 게임 오버 여부를 읽을 수 있게 합니다.
    #endregion
    public void StartGame() // 게임을 처음 시작할 때 호출하는 함수입니다.
    { // StartGame 함수의 시작입니다.
        currentDay = 1; // 날짜를 1일차로 초기화합니다.
        isGameOver = false; // 게임 오버 상태를 해제합니다.

        Initialize(stats); // 지표 수치를 무작위 시작값으로 초기화합니다.

        Debug.Log("게임 시작"); // 콘솔에 게임 시작 로그를 출력합니다.
        Debug.Log($"{currentDay}일차 시작"); // 콘솔에 현재 날짜 시작 로그를 출력합니다.
        LogCurrentStatValues();
    } // StartGame 함수의 끝입니다.
    private void LogCurrentStatValues()
    {
        foreach (var pair in currentStatValues)
        {
            StatData stat = pair.Key;
            string statName = string.IsNullOrWhiteSpace(stat.DisplayName) ? stat.name : stat.DisplayName;

            Debug.Log($"{statName}: {pair.Value}");
        }
    }
    public void printlog()
    {
        Debug.Log("버튼 할당이 되었습니다.");
    }
    private void CheckGameOver() // 지표 중 하나라도 최소값에 도달했는지 확인하는 함수입니다.
    { // CheckGameOver 함수의 시작입니다.
        foreach (var pair in currentStatValues) // 저장된 모든 지표 수치를 하나씩 확인합니다.
        { // foreach 반복문의 시작입니다.
            StatData stat = pair.Key; // 현재 확인 중인 지표 데이터입니다.
            int value = pair.Value; // 현재 확인 중인 지표의 수치입니다.

            if (value <= stat.MinValue) // 지표 수치가 최소값 이하인지 확인합니다.
            { // if문의 시작입니다.
                isGameOver = true; // 게임 오버 상태로 변경합니다.
                Debug.Log($"{stat.DisplayName} 수치가 0이 되어 게임 오버"); // 콘솔에 게임 오버 원인을 출력합니다.
                return; // 더 확인하지 않고 함수를 종료합니다.
            } // if문의 끝입니다.
        } // foreach 반복문의 끝입니다.
    } // CheckGameOver 함수의 끝입니다.

    public void GoToNextDay() // 다음 날짜로 넘어갈 때 호출하는 함수입니다.
    { // GoToNextDay 함수의 시작입니다.
        if (isGameOver) // 이미 게임 오버 상태인지 확인합니다.
        { // if문의 시작입니다.
            Debug.Log("게임 오버 상태라 다음 날짜로 갈 수 없음"); // 콘솔에 다음 날짜로 갈 수 없다는 로그를 출력합니다.
            CheckEnding();
            return; // 함수 실행을 중단합니다.
        } // if문의 끝입니다.

        currentDay++; // 현재 날짜를 1 증가시킵니다.

        if (currentDay > maxDay) // 현재 날짜가 최대 날짜를 넘었는지 확인합니다.
        { // if문의 시작입니다.
            Debug.Log("7일차 종료. 엔딩으로 이동"); // 콘솔에 엔딩 이동 로그를 출력합니다.
            CheckEnding(); // 엔딩을 판정합니다.
            return; // 함수 실행을 중단합니다.
        } // if문의 끝입니다.

        Debug.Log($"{currentDay}일차 시작"); // 콘솔에 새 날짜 시작 로그를 출력합니다.
    } // GoToNextDay 함수의 끝입니다.

    private void CheckEnding() // 최종 엔딩을 판정하는 함수입니다.
    { // CheckEnding 함수의 시작입니다.
        if (isGameOver) // 게임 오버 상태인지 확인합니다.
        { // if문의 시작입니다.
            Debug.Log("실패 엔딩"); // 게임 오버 상태라면 실패 엔딩 로그를 출력합니다.
        } // if문의 끝입니다.
        else // 게임 오버 상태가 아니라면 아래 내용을 실행합니다.
        { // else문의 시작입니다.
            Debug.Log("굿 엔딩"); // 생존에 성공했으므로 굿 엔딩 로그를 출력합니다.
        } // else문의 끝입니다.
    } // CheckEnding 함수의 끝입니다.
    public void Initialize(IReadOnlyList<StatData> statDataList) // 게임 시작 시 모든 지표의 시작 값을 설정하는 함수입니다.
    {
        currentStatValues.Clear(); // 이전 게임의 지표 값이 남아있지 않도록 모두 비웁니다.

        foreach (StatData statData in statDataList) // 등록된 모든 지표 데이터를 하나씩 반복합니다.
        {
            int startValue = Random.Range( // 지표의 시작 값을 랜덤으로 정합니다.
                statData.StartMinValue, // 이 지표가 시작할 수 있는 최소값입니다.
                statData.StartMaxValue + 1 // 이 지표가 시작할 수 있는 최대값입니다. int Random.Range는 끝값을 포함하지 않아서 +1 합니다.
            );

            currentStatValues[statData] = startValue; // 현재 지표 값 목록에 랜덤으로 정한 시작 값을 저장합니다.
        }
    }
        public int GetStatValue(StatData stat) // 특정 지표의 현재 수치를 가져오는 함수입니다.
    { // GetStatValue 함수의 시작입니다.
        if (currentStatValues.TryGetValue(stat, out int value)) // 딕셔너리에 해당 지표가 있으면 값을 가져옵니다.
        { // if문의 시작입니다.
            return value; // 찾은 현재 수치를 반환합니다.
        } // if문의 끝입니다.

        return 0; // 해당 지표가 없으면 기본값 0을 반환합니다.
    } // GetStatValue 함수의 끝입니다.

    public void ChangeStatValue(StatData stat, int amount) // 특정 지표의 수치를 변경하는 함수입니다.
    { // ChangeStatValue 함수의 시작입니다.
        if (!currentStatValues.ContainsKey(stat)) // 딕셔너리에 해당 지표가 없으면 처리하지 않습니다.
        { // if문의 시작입니다.
            return; // 함수 실행을 중단합니다.
        } // if문의 끝입니다.

        int changedValue = currentStatValues[stat] + amount; // 현재 수치에 변화량을 더합니다.

        changedValue = Mathf.Clamp( // 수치가 최소값과 최대값 사이에 머물도록 제한합니다.
            changedValue, // 제한할 대상 값입니다.
            stat.MinValue, // 지표의 최소값입니다.
            stat.MaxValue // 지표의 최대값입니다.
        ); // Mathf.Clamp 호출의 끝입니다.

        currentStatValues[stat] = changedValue; // 제한된 값을 현재 지표 수치로 저장합니다.

        Debug.Log($"{stat.DisplayName}: {changedValue}"); // 콘솔에 변경된 지표 수치를 출력합니다.

        CheckGameOver(); // 수치 변경 후 게임 오버 여부를 확인합니다.
    } // ChangeStatValue 함수의 끝입니다.
} // GameManager 클래스의 끝입니다.