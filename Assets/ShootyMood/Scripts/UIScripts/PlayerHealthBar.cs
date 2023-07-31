using System;
using ShootyMood.Scripts.Models;
using ShootyMood.Scripts.ShootyGameEvents;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ShootyMood.Scripts.UIScripts
{
    public class PlayerHealthBar : MonoBehaviour, IInitializable, IDisposable
    {
        [Inject] private PlayerModel playerModel;
        [Inject] private SignalBus signalBus;
        [SerializeField] private Slider slider;

        public void Initialize()
        {
            SetStartState();
            signalBus.Subscribe<PlayerGotDamage>(OnPlayerGotHit);
        }

        private void SetStartState()
        {
            slider.maxValue = playerModel.Health;
            slider.value = playerModel.Health;
        }

        private void OnPlayerGotHit()
        {
            slider.value = playerModel.Health;
        }

        public void Dispose()
        {
            signalBus.Unsubscribe<PlayerGotDamage>(OnPlayerGotHit);
        }
    }
}