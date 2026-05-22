using System.Collections.Generic; // Dictionary, IReadOnlyList 같은 컬렉션 자료형을 사용하기 위해 필요합니다.
using UnityEngine; // Unity의 Random, Mathf 기능을 사용하기 위해 필요합니다.

public class GameStats // 게임 진행 중 지표들의 현재 값을 관리하는 클래스입니다.
{
    private readonly Dictionary<StatData, int> currentValues = new(); // 각 지표 데이터와 현재 수치를 함께 저장하는 Dictionary입니다.

    public void Initialize(IReadOnlyList<StatData> statDataList) // 게임 시작 시 모든 지표의 시작 값을 설정하는 함수입니다.
    {
        currentValues.Clear(); // 이전 게임의 지표 값이 남아있지 않도록 모두 비웁니다.

        foreach (StatData statData in statDataList) // 등록된 모든 지표 데이터를 하나씩 반복합니다.
        {
            int startValue = Random.Range( // 지표의 시작 값을 랜덤으로 정합니다.
                statData.StartMinValue, // 이 지표가 시작할 수 있는 최소값입니다.
                statData.StartMaxValue + 1 // 이 지표가 시작할 수 있는 최대값입니다. int Random.Range는 끝값을 포함하지 않아서 +1 합니다.
            );

            currentValues[statData] = startValue; // 현재 지표 값 목록에 랜덤으로 정한 시작 값을 저장합니다.
        }
    }

    public int GetValue(StatData statData) // 특정 지표의 현재 값을 가져오는 함수입니다.
    {
        return currentValues.TryGetValue( // Dictionary 안에 해당 지표가 있는지 확인합니다.
            statData, // 찾고 싶은 지표 데이터입니다.
            out int value // 찾았다면 현재 값이 value 변수에 들어갑니다.
        ) ? value : 0; // 값이 있으면 value를 반환하고, 없으면 0을 반환합니다.
    }

    public void AddValue(StatData statData, int amount) // 특정 지표의 현재 값을 증가하거나 감소시키는 함수입니다.
    {
        int currentValue = GetValue(statData); // 현재 지표 값을 가져옵니다.

        int nextValue = currentValue + amount; // 현재 값에 변화량을 더해서 다음 값을 계산합니다.

        currentValues[statData] = Mathf.Clamp( // 계산된 값을 최소값과 최대값 사이로 제한해서 저장합니다.
            nextValue, // 제한하기 전의 계산된 값입니다.
            statData.MinValue, // 이 지표가 가질 수 있는 최소값입니다.
            statData.MaxValue // 이 지표가 가질 수 있는 최대값입니다.
        );
    }

    public bool IsZeroOrBelow(StatData statData) // 특정 지표가 게임 오버 조건에 도달했는지 확인하는 함수입니다.
    {
        return GetValue(statData) <= statData.MinValue; // 현재 값이 최소값 이하라면 true를 반환합니다.
    }
}