// n일차 시작 화면에서 이벤트 화면으로 넘어가는 애니메이션
using DG.Tweening;
using UnityEngine;

  public class DayToEventTransitionUI : MonoBehaviour
  {
      [Header("Panels")]
      [SerializeField] private GameObject dayPanel;
      [SerializeField] private GameObject eventPanel;
      [SerializeField] private CanvasGroup dayPanelGroup;
      [SerializeField] private CanvasGroup eventPanelGroup;

      [Header("Day")]
      [SerializeField] private RectTransform dayFrom;
      [SerializeField] private RectTransform dayTo;

      [Header("Stat Icons")]
      [SerializeField] private RectTransform userIconFrom;
      [SerializeField] private RectTransform userIconTo;

      [SerializeField] private RectTransform publicIconFrom;
      [SerializeField] private RectTransform publicIconTo;

      [SerializeField] private RectTransform serverIconFrom;
      [SerializeField] private RectTransform serverIconTo;

      [SerializeField] private RectTransform devIconFrom;
      [SerializeField] private RectTransform devIconTo;

      [SerializeField] private RectTransform budgetIconFrom;
      [SerializeField] private RectTransform budgetIconTo;

      [Header("Option")]
      [SerializeField] private float autoTransitionDelay = 3f;
      [SerializeField] private float duration = 0.8f;
      [SerializeField] private Ease moveEase = Ease.InOutCubic;

      private Vector3 dayStartPosition;
      private Vector3 userIconStartPosition;
      private Vector3 publicIconStartPosition;
      private Vector3 serverIconStartPosition;
      private Vector3 devIconStartPosition;
      private Vector3 budgetIconStartPosition;

      private Sequence transitionSequence;
      private Tween autoTransitionTween;
      private bool isTransitioning;

      private void Awake()
      {
          SaveStartPositions();

          if (eventPanel != null)
          {
              eventPanel.SetActive(false);
          }
      }

      private void Start()
      {
          ScheduleAutoTransition();
      }

      private void OnDestroy()
      {
          if (autoTransitionTween != null && autoTransitionTween.IsActive())
          {
              autoTransitionTween.Kill();
          }

          if (transitionSequence != null && transitionSequence.IsActive())
          {
              transitionSequence.Kill();
          }
      }

      private void ScheduleAutoTransition()
      {
          if (autoTransitionTween != null && autoTransitionTween.IsActive())
          {
              autoTransitionTween.Kill();
          }

          autoTransitionTween = DOVirtual.DelayedCall(autoTransitionDelay, PlayTransition);
      }

      public void PlayTransition()
      {
          if (isTransitioning)
          {
              return;
          }

          isTransitioning = true;

          if (autoTransitionTween != null && autoTransitionTween.IsActive())
          {
              autoTransitionTween.Kill();
          }

          if (transitionSequence != null && transitionSequence.IsActive())
          {
              transitionSequence.Kill();
          }

          if (eventPanel != null)
          {
              eventPanel.SetActive(true);
          }

          Canvas.ForceUpdateCanvases();

          if (eventPanelGroup != null)
          {
              eventPanelGroup.alpha = 0f;
              eventPanelGroup.blocksRaycasts = false;
              eventPanelGroup.interactable = false;
          }

          transitionSequence = DOTween.Sequence();

          transitionSequence.Join(dayFrom.DOMove(dayTo.position, duration).SetEase(moveEase));
          transitionSequence.Join(userIconFrom.DOMove(userIconTo.position, duration).SetEase(moveEase));
          transitionSequence.Join(publicIconFrom.DOMove(publicIconTo.position, duration).SetEase(moveEase));
          transitionSequence.Join(serverIconFrom.DOMove(serverIconTo.position, duration).SetEase(moveEase));
          transitionSequence.Join(devIconFrom.DOMove(devIconTo.position, duration).SetEase(moveEase));
          transitionSequence.Join(budgetIconFrom.DOMove(budgetIconTo.position, duration).SetEase(moveEase));

          if (dayPanelGroup != null)
          {
              transitionSequence.Join(dayPanelGroup.DOFade(0f, duration * 0.8f));
          }

          if (eventPanelGroup != null)
          {
              transitionSequence.Insert(duration * 0.4f, eventPanelGroup.DOFade(1f, duration * 0.6f));
          }

          transitionSequence.OnComplete(() =>
          {
              if (dayPanel != null)
              {
                  dayPanel.SetActive(false);
              }

              if (eventPanelGroup != null)
              {
                  eventPanelGroup.alpha = 1f;
                  eventPanelGroup.blocksRaycasts = true;
                  eventPanelGroup.interactable = true;
              }

              ResetDayPanelObjects();

              isTransitioning = false;
          });
      }

      private void SaveStartPositions()
      {
          dayStartPosition = dayFrom.position;
          userIconStartPosition = userIconFrom.position;
          publicIconStartPosition = publicIconFrom.position;
          serverIconStartPosition = serverIconFrom.position;
          devIconStartPosition = devIconFrom.position;
          budgetIconStartPosition = budgetIconFrom.position;
      }

      private void ResetDayPanelObjects()
      {
          dayFrom.position = dayStartPosition;
          userIconFrom.position = userIconStartPosition;
          publicIconFrom.position = publicIconStartPosition;
          serverIconFrom.position = serverIconStartPosition;
          devIconFrom.position = devIconStartPosition;
          budgetIconFrom.position = budgetIconStartPosition;

          if (dayPanelGroup != null)
          {
              dayPanelGroup.alpha = 1f;
          }
      }
  }
