# Codex Handoff Notes

이 문서는 다른 Codex가 이 Unity 프로젝트와 이전 대화 맥락을 빠르게 이어받기 위한 요약입니다.

## 먼저 할 일

새 Codex는 이 파일을 읽은 뒤 사용자에게 먼저 물어보는 것이 좋습니다.

> 지금 작업자는 팀원 A, B, C 중 누구인가요?

역할에 따라 수정해야 하는 파일 범위가 다릅니다. Git 충돌을 줄이기 위해 담당 범위 밖의 파일은 되도록 수정하지 않습니다.

## 프로젝트 개요

- 게임 제목: `운영자님, 저희 게임 터졌어요`
- 장르: Unity 2D 기반 운영 시뮬레이션 / 선택형 카드 게임
- 상황: 라이브 서비스 게임 운영자가 7일 동안 여러 사건에 대응하며 지표를 관리하는 게임
- 핵심 루프:
  - 인트로
  - 시작 화면
  - n일차 시작
  - 사건 카드 등장
  - 선택지 선택
  - 결과 및 커뮤니티 반응 표시
  - 다음 날짜 진행
  - 7일차 종료 또는 지표 0 도달 시 엔딩

## 개발 조건

- 프로그래밍 팀원 3명
- Unity 프로젝트 경험 없음
- 협업 경험 없음
- 2주 단기 프로젝트
- 따라서 구조는 단순하게 유지하고, Git 충돌 방지가 매우 중요함

## 팀 역할

### 팀원 A: 프로젝트 관리자 / 게임 흐름 담당

담당 범위:

- Unity 프로젝트 기본 세팅
- GitHub 저장소 관리
- `main` 브랜치 보호 역할
- `GameManager` 구현
- 날짜 진행 구현
- 지표 데이터 및 현재값 관리
- 게임오버 판정
- 생존/실패 엔딩 판정
- B, C 작업물 최종 통합

주요 파일 계획:

```text
Assets/Scripts/Core/GameManager.cs
Assets/Scripts/Core/DayManager.cs
Assets/Scripts/Core/GameStatus.cs
Assets/Scripts/Core/EndingManager.cs
```

현재 프로젝트에는 아래 A 관련 파일이 존재합니다.

```text
Assets/Scripts/Core/GameManager.cs
Assets/Scripts/Core/GameStats.cs
Assets/Scripts/Data/StatData.cs
Assets/Resources/Stats/
```

### 팀원 B: 카드 데이터 / 선택지 결과 담당

담당 범위:

- 사건 카드 구조 구현
- 선택지 구조 구현
- 카드 목록 제작
- MVP 카드 7~10장 또는 8~12장 작성
- 선택지별 지표 변화량 입력
- 선택 후 출력할 커뮤니티 반응 문구 작성

주요 파일 계획:

```text
Assets/Scripts/Cards/EventCardData.cs
Assets/Scripts/Cards/ActionOptionData.cs
Assets/Scripts/Cards/CardManager.cs
Assets/Resources/CardData/
```

주의:

- `GameManager` 내부 로직은 직접 수정하지 않는 것이 좋음
- 선택지 효과는 A의 지표 시스템과 연결될 수 있도록 데이터 형태로 작성하는 것이 좋음

### 팀원 C: UI / 화면 표시 담당

담당 범위:

- 메인 게임 화면 구성
- 날짜, 아이콘, 지표 표시
- 사건 제목과 설명 표시
- 선택지 버튼 3~4개 표시
- 커뮤니티 반응 출력
- 엔딩 화면 UI
- UI 팝업

주요 파일 계획:

```text
Assets/Scripts/UI/StatusBarUI.cs
Assets/Scripts/UI/EventPanelUI.cs
Assets/Scripts/UI/ChoicePanelUI.cs
Assets/Scripts/UI/ReactionPanelUI.cs
Assets/Scripts/UI/EndingUI.cs
Assets/Prefabs/UI/
```

주의:

- 팀원 C는 UI 관련 파일만 수정하는 것이 좋음
- `GameManager`나 `CardManager`의 내부 로직은 직접 수정하지 않는 것이 좋음
- Unity 씬 파일은 충돌이 자주 나므로, 씬 수정 담당자를 한 명으로 고정하는 것이 안전함

## 현재 파일 상태 요약

마지막 확인 기준:

- Unity 버전: `6000.4.6f1`
- 렌더링/템플릿: Unity 2D + URP
- Git 상태: 마지막 확인 시 깨끗했음

현재 확인된 주요 파일:

```text
Assets/Scripts/Data/StatData.cs
Assets/Scripts/Core/GameStats.cs
Assets/Scripts/Core/GameManager.cs
Assets/Resources/Stats/active_user.asset
Assets/Resources/Stats/community_trust.asset
Assets/Resources/Stats/dev_stamina.asset
Assets/Resources/Stats/operation_budget.asset
Assets/Resources/Stats/server_stability.asset
```

## 지표 데이터

현재 실제 지표 asset은 5개입니다.

```text
active_user          유저 수
community_trust      커뮤니티 신뢰도
dev_stamina          개발팀 체력
operation_budget     운영 예산
server_stability     서버 안정성
```

각 지표의 현재 공통 설정:

```text
minValue: 0
maxValue: 100
startMinValue: 45
startMaxValue: 65
```

`StatData.cs`는 ScriptableObject 정의 파일입니다. Unity에서 아래 메뉴로 새 지표 asset을 만들 수 있습니다.

```text
Create > Game Data > Stat Data
```

실제 지표 데이터는 `.cs`가 아니라 `.asset` 파일입니다.

## 현재 코드 구조상 주의점

`GameStats.cs`가 지표 현재값을 관리합니다.

기능:

- 게임 시작 시 지표별 랜덤 초기화
- 현재 지표 값 조회
- 지표 값 증가/감소
- 최소값 이하 판정

그런데 `GameManager.cs` 안에도 별도의 `currentStatValues` 딕셔너리가 있습니다.

이 상태에서는 지표 값 관리가 둘로 나뉘어 꼬일 수 있습니다. 다음 정리 작업에서는 `GameManager`가 `GameStats`만 사용하도록 통합하는 것이 좋습니다.

## Git 충돌 방지 규칙

팀원별 수정 범위를 강하게 지키는 것이 좋습니다.

```text
A: Assets/Scripts/Core/
A: Assets/Scripts/Data/StatData.cs
A: Assets/Resources/Stats/

B: Assets/Scripts/Cards/
B: Assets/Resources/CardData/

C: Assets/Scripts/UI/
C: Assets/Prefabs/UI/
C: UI 담당 씬 또는 UI 프리팹
```

공통 파일이나 씬 파일은 한 명만 수정합니다. 특히 Unity의 `.unity`, `.prefab`, `.asset`, `.meta` 파일은 충돌이 나면 초보자가 해결하기 어렵습니다.

## 작업 전 Codex에게 권장되는 질문

다른 Codex는 작업 시작 전에 아래 질문을 먼저 하는 것이 좋습니다.

```text
지금 사용자는 팀원 A, B, C 중 누구인가요?
이번에 수정하려는 범위는 어디인가요?
씬 파일을 수정해도 되나요, 아니면 스크립트만 수정할까요?
```

## 이전 대화에서 나온 설계 방향

- 2주 프로젝트이므로 과한 구조보다 작고 명확한 구조가 좋음
- 카드/선택지/지표는 가능하면 ScriptableObject 기반 데이터로 관리
- 초반 목표는 "하루 진행 루프" 완성
- 최소 구현 목표:
  - 지표 4~5개
  - 사건 카드 7~12개
  - 선택지 2~4개
  - 7일 진행
  - 지표 0 이하 실패 엔딩
  - 7일 생존 성공 엔딩
  - 결과/댓글 반응 표시

## Codex 작업 원칙

- 사용자에게 코드 설명을 할 때는 Unity 초보 기준으로 설명
- 파일을 만들기 전 현재 파일 상태를 확인
- 담당 범위 밖 파일은 함부로 수정하지 않기
- `.meta` 파일은 Unity 협업에서 중요하므로 삭제하거나 무시하지 않기
- 사용자가 명시하지 않으면 씬 파일 수정은 조심하기
