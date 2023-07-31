using System;
using Assets.ShootyMood.Scripts.Managers;
using ShootyMood.Scripts.ShootyGameEvents;
using UnityEngine;
using Zenject;

namespace ShootyMood.Scripts.Handlers
{
    public class AudioService : MonoBehaviour, IInitializable, IDisposable
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioSource killAudioSource;
        [SerializeField] private AudioSource enemyAttackAudioSource;
        [SerializeField] private AudioSource musicAudioSource;

        [SerializeField] private AudioClip enemySpawnClip;
        [SerializeField] private AudioClip enemyKilledClip;
        [SerializeField] private AudioClip friendlyKilled;
        [SerializeField] private AudioClip enemyAttackAudioClip;
        
        [Inject] private SignalBus signalBus;
        
        public void Initialize()
        {
            signalBus.Subscribe<EnemyKilled>(OnEnemyKilled);
            signalBus.Subscribe<EnemySpawned>(OnEnemySpawned);
            signalBus.Subscribe<PlayerGotDamage>(OnPlayerGotDamage);
            signalBus.Subscribe<AudioOptionChanged>(OnAudioOptionChanged);
        }
        
        public void Dispose()
        {
            signalBus.TryUnsubscribe<EnemyKilled>(OnEnemyKilled);
            signalBus.TryUnsubscribe<EnemySpawned>(OnEnemySpawned);
            signalBus.TryUnsubscribe<PlayerGotDamage>(OnPlayerGotDamage);
            signalBus.TryUnsubscribe<AudioOptionChanged>(OnAudioOptionChanged);
        }

        private void OnEnemyKilled(EnemyKilled evt)
        {
            if(!AudioOptionManager.Instance.AUDIO_ON)
            {
                return;
            }

            if (evt.isFriendly)
            {
                killAudioSource.clip = friendlyKilled;
                killAudioSource.time = .54f;
            }
            else
            {
                killAudioSource.clip = enemyKilledClip;
                killAudioSource.time = .45f;
            }
            
            killAudioSource.Play();
        }

        private void OnEnemySpawned(EnemySpawned evt)
        {
            if (!AudioOptionManager.Instance.AUDIO_ON)
            {
                return;
            }

            audioSource.clip = enemySpawnClip; 
            audioSource.Play();
        }

        private void OnPlayerGotDamage(PlayerGotDamage pg)
        {
            if (!AudioOptionManager.Instance.AUDIO_ON)
            {
                return;
            }

            enemyAttackAudioSource.clip = enemyAttackAudioClip; 
            enemyAttackAudioSource.Play();
        }

        private void OnAudioOptionChanged(AudioOptionChanged evt)
        {
            if(!AudioOptionManager.Instance.AUDIO_ON)
            {
                musicAudioSource.Stop();
            }
            else
            {
                musicAudioSource.Play();
            }
        }
    }
}