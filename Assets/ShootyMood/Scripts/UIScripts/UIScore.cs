using Assets.ShootyMood.Scripts.Managers;
using ShootyMood.Scripts.Config.Wave;
using ShootyMood.Scripts.ShootyGameEvents;
using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.ShootyMood.Scripts.UIScripts
{
    public class UIScore : MonoBehaviour, IInitializable, IDisposable
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [Inject] private WavesConfig wavesConfig;
        [Inject] private SignalBus signalBus;

        private int score = 0;

        public void Initialize()
        {
            signalBus.Subscribe<PlayerKilled>(OnPlayerKilled);
            signalBus.Subscribe<EnemyKilled>(OnEnemyKilled);
            signalBus.Subscribe<GameStarted>(OnGameStarted);
        }

        public void Dispose()
        {
            signalBus.TryUnsubscribe<PlayerKilled>(OnPlayerKilled);
            signalBus.TryUnsubscribe<EnemyKilled>(OnEnemyKilled);
            signalBus.TryUnsubscribe<GameStarted>(OnGameStarted);
        }

        private void OnEnemyKilled(EnemyKilled evt)
        {
            if (evt.isFriendly)
            {
                score -= wavesConfig.ScoreAddition * 2;
            }
            else
            {
                score += wavesConfig.ScoreAddition;
            }

            score = score < 0 ? 0 : score;
            scoreText.text = score.ToString();
        }

        private void OnGameStarted(GameStarted evt)
        {
            score = 0;
            scoreText.text = score.ToString();
        }

        private void OnPlayerKilled(PlayerKilled evt)
        {
            SaveManager.SaveScore(score);
        }
        
    }
}
