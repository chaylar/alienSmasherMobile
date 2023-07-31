using System;
using ShootyMood.Scripts.Managers;
using ShootyMood.Scripts.ShootyGameEvents;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using IInitializable = Zenject.IInitializable;

namespace ShootyMood.Scripts.UIScripts
{
    public class UITimer : MonoBehaviour, IInitializable, IDisposable
    {
        [SerializeField] private TextMeshProUGUI timerText;
        [Inject] private SignalBus signalBus;
        private float latestGameStartTime = 0f;

        private void Update()
        {
            if (GameStateManager.Instance.GetState() != GameStateManager.GameState.PLAY)
            {
                return;
            }
            
            UpdateTimer();
        }

        private void UpdateTimer()
        {
            float timerValue = Time.realtimeSinceStartup - latestGameStartTime;

            if (timerValue < 0)
            {
                timerValue = 0;
            }
            
            TimeSpan timeSpan = TimeSpan.FromSeconds(timerValue);
            string timeText = timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00");

            timerText.text = timeText;
        }

        private void OnGameStarted()
        {
            latestGameStartTime = Time.realtimeSinceStartup;
        }

        public void Initialize()
        {
            signalBus.Subscribe<GameStarted>(OnGameStarted);
        }

        public void Dispose()
        {
            signalBus.Unsubscribe<GameStarted>(OnGameStarted);
        }
    }
}