using System.Collections.Generic; // Dictionary 같은 컬렉션 자료구조를 사용하기 위해 필요합니다.
using UnityEngine; // MonoBehaviour, SerializeField, Random, Mathf 같은 Unity 기능을 사용하기 위해 필요합니다.
public class GameManager : MonoBehaviour // Unity 오브젝트에 붙일 수 있는 게임 진행 관리자 클래스입니다.
{ // GameManager 클래스의 시작입니다.
    
#region Game State
    [Header("Game Settings")] // Inspector에서 게임 설정 항목을 보기 좋게 구분해 줍니다.
    [SerializeField] private int maxDay = 7; // 게임이 몇 일차까지 진행되는지 정합니다.
    [SerializeField] private CallEnding callEnding; // 엔딩을 보여주는 CallEnding 컴포넌트를 연결하기 위한 변수입니다.
    private int currentDay = 1; // 현재 날짜를 저장합니다.
    private bool isGameOver; // 게임 오버 상태인지 저장합니다.
    public int CurrentDay => currentDay; // 다른 스크립트가 현재 날짜를 읽을 수 있게 합니다.
    public bool IsGameOver => isGameOver; // 다른 스크립트가 게임 오버 여부를 읽을 수 있게 합니다.
    GameStats gamestats = new GameStats(); // 게임의 지표를 관리하는 GameStats 클래스의 인스턴스를 생성합니다.
    public GameStats Stats => gamestats; // 다른 스크립트가 게임 지표를 읽을 수 있게 합니다.

    #endregion
    public void StartGame() // 게임을 처음 시작할 때 호출하는 함수입니다.
    { // StartGame 함수의 시작입니다.
        currentDay = 1; // 날짜를 1일차로 초기화합니다.
        isGameOver = false; // 게임 오버 상태를 해제합니다.
        gamestats.InitializeRandom(); // 게임 지표를 랜덤한 초기값으로 설정합니다.
        Debug.Log("게임 시작"); // 콘솔에 게임 시작 로그를 출력합니다.
        Debug.Log($"{currentDay}일차 시작"); // 콘솔에 현재 날짜 시작 로그를 출력합니다.
    } // StartGame 함수의 끝입니다.

    private void CheckGameOver() // 지표 중 하나라도 최소값에 도달했는지 확인하는 함수입니다.
    { // CheckGameOver 함수의 시작입니다.
        if (gamestats.User <= 0 || gamestats.Public <= 0 || gamestats.Server <= 0 || gamestats.Dev <= 0 || gamestats.Budget <= 0)
        {
            isGameOver = true;
            callEnding.PrintEnding(gamestats); // 게임 오버 상태라면 CallEnding 컴포넌트의 PrintEnding 함수를 호출해서 엔딩을 보여줍니다.
            Debug.Log("게임 오버 조건 충족"); // 콘솔에 게임 오버 조건 충족 로그를 출력합니다.
        }
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
            callEnding.PrintEnding(gamestats); // CallEnding 컴포넌트의 PrintEnding 함수를 호출해서 엔딩을 보여줍니다.
        } // if문의 끝입니다.
        else // 게임 오버 상태가 아니라면 아래 내용을 실행합니다.
        { // else문의 시작입니다.
            Debug.Log("굿 엔딩"); // 생존에 성공했으므로 굿 엔딩 로그를 출력합니다.
            callEnding.PrintEnding(gamestats); // CallEnding 컴포넌트의 ShowEnding_Good 함수를 호출해서 굿 엔딩을 보여줍니다.
        } // else문의 끝입니다.
    } // CheckEnding 함수의 끝입니다.
    public void isthisreal()
    {
        gamestats.User = 0;
        CheckGameOver();
    }
} // GameManager 클래스의 끝입니다.