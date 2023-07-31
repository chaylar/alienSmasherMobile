using Assets.ShootyMood.Scripts.Managers;
using ShootyMood.Scripts.Config.Wave;
using ShootyMood.Scripts.ShootyGameEvents;
using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.ShootyMood.Scripts.UIScripts
{
    public class UIScore : MonoBehaviour, IInitializable, IDisposable
    {
        [SerializeField] private ParticleSystem scoreParticle;
        [SerializeField] private TextMeshProUGUI scoreText;

        [SerializeField] private Image effectPositonImage;

        [Inject] private WavesConfig wavesConfig;
        [Inject] private SignalBus signalBus;

        private int score = 0;

        private void Start()
        {
            scoreParticle.gameObject.SetActive(false);
            var starPosition = Camera.main.ScreenToWorldPoint(effectPositonImage.transform.position);
            scoreParticle.transform.position = new Vector3(starPosition.x, starPosition.y, 0f);
        }

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

            if(score >= 100)
            {
                if(!scoreParticle.gameObject.activeSelf)
                    scoreParticle.gameObject.SetActive(true);
            }
            else
            {
                if(scoreParticle.gameObject.activeSelf)
                    scoreParticle.gameObject.SetActive(false);
            }
        }

        private void OnGameStarted(GameStarted evt)
        {
            score = 0;
            scoreText.text = score.ToString();
        }

        private void OnPlayerKilled(PlayerKilled evt)
        {
            SaveManager.SaveScore(score);
            scoreParticle.gameObject.SetActive(false);
        }
        
    }
}
