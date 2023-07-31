using DG.Tweening;
using ShootyMood.Scripts.ShootyGameEvents;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.ShootyMood.Scripts.UIScripts
{
    public class UIAlert : MonoBehaviour, IInitializable, IDisposable
    {

        [SerializeField] private Image frienlyKillImage;
        [Inject] private SignalBus signalBus;

        private Color frienlyKillAlertStartingColor;

        private void Start()
        {
            frienlyKillAlertStartingColor = frienlyKillImage.color;
            frienlyKillImage.gameObject.SetActive(false);
        }

        public void Initialize()
        {
            signalBus.Subscribe<EnemyKilled>(OnEnemyKilled);
        }

        public void Dispose()
        {
            signalBus.TryUnsubscribe<EnemyKilled>(OnEnemyKilled);
        }

        private void OnEnemyKilled(EnemyKilled evt)
        {
            Debug.Log("AMK!");
            if(evt.isFriendly)
            {
                Debug.Log("AMKIKI!");
                frienlyKillImage.color = frienlyKillAlertStartingColor;
                frienlyKillImage.gameObject.SetActive(true);
                frienlyKillImage.DOColor(new Color(frienlyKillAlertStartingColor.r, frienlyKillAlertStartingColor.g, frienlyKillAlertStartingColor.b, 0f), 1f).OnComplete(OnFrienlyKillAlertComplete);
            }
        }

        private void OnFrienlyKillAlertComplete()
        {
            frienlyKillImage.gameObject.SetActive(false);
        }
    }
}
