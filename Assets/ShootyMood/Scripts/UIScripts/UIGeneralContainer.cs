using System;
using Assets.ShootyMood.Scripts.Managers;
using Assets.ShootyMood.Scripts.UIScripts;
using DG.Tweening;
using ShootyMood.Scripts.Managers;
using ShootyMood.Scripts.ShootyGameEvents;
using TMPro;
using UnityEngine;
using Zenject;

namespace ShootyMood.Scripts.UIScripts
{
    public class UIGeneralContainer : MonoBehaviour, IInitializable, IDisposable
    {
        [SerializeField] private CanvasGroup playerKilledInfoPanel;
        [SerializeField] private TextMeshProUGUI bestScoreText;
        [SerializeField] private TextMeshProUGUI lastScoreText;

        [SerializeField] private UITimerBackwards uiTimer;
        [SerializeField] private UIScore uiScore;
        [SerializeField] private MainMenu mainMenu;

        [SerializeField] private GameObject infoPopup;

        [Inject] private SignalBus signalBus;
        
        public void Initialize()
        {
            SetToMenuState();
            signalBus.Subscribe<PlayerKilled>(OnPLayerKilled);
            signalBus.Subscribe<PlayButtonClickEvent>(OnPlayButtonClick);
        }

        public void Dispose()
        {
            signalBus.TryUnsubscribe<PlayerKilled>(OnPLayerKilled);
            signalBus.TryUnsubscribe<PlayButtonClickEvent>(OnPlayButtonClick);
        }

        private void PlayerKilled()
        {
            playerKilledInfoPanel.alpha = 1f;
            playerKilledInfoPanel.gameObject.SetActive(true);
            playerKilledInfoPanel.DOFade(0f, 3.6f).OnComplete(DisableKilledInfoPanel);
        }

        private void DisableKilledInfoPanel()
        {
            playerKilledInfoPanel.gameObject.SetActive(false);
        }

        private void SetToMenuState()
        {
            HideInfoPopup();
            uiTimer.gameObject.SetActive(false);
            uiScore.gameObject.SetActive(false);

            int lastScore = SaveManager.GetLastScore();
            int bestScore = SaveManager.GetBestScore();

            lastScoreText.text = string.Format("Last : {0}" , lastScore.ToString());
            bestScoreText.text = string.Format("Best : {0}", bestScore.ToString());

            lastScoreText.gameObject.SetActive(lastScore > 0);
            bestScoreText.gameObject.SetActive(bestScore > 0);

            mainMenu.gameObject.SetActive(true);
            
            GameStateManager.Instance.SetState(GameStateManager.GameState.MENU);
        }

        private void SetToPlayState()
        {
            HideInfoPopup();
            uiTimer.gameObject.SetActive(true);
            uiScore.gameObject.SetActive(true);

            mainMenu.gameObject.SetActive(false);
            
            signalBus.Fire(new GameStarted());
            GameStateManager.Instance.SetState(GameStateManager.GameState.PLAY);
        }

        private void OnPLayerKilled(PlayerKilled evy)
        {
            PlayerKilled();
            SetToMenuState();
        }

        private void OnPlayButtonClick()
        {
            SetToPlayState();
        }

        public void ShowInfoPopup()
        {
            infoPopup.SetActive(true);
        }

        public void HideInfoPopup()
        {
            infoPopup.SetActive(false);
        }
    }
}