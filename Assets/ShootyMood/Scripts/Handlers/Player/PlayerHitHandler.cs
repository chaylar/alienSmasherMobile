using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using ShootyMood.Scripts.Handlers.Base;
using ShootyMood.Scripts.Models;
using ShootyMood.Scripts.ShootyGameEvents;
using UnityEngine;
using Zenject;

namespace ShootyMood.Scripts.Handlers
{
    public class PlayerHitHandler : BaseCharacterHitHandler<PlayerModel>
    {
        [Inject] private SignalBus signalBus;

        private int maxHealth = 0;
        private void Start()
        {
            maxHealth = CharacterModel.Health;
        }
        
        public void ResetHealth()
        {
            CharacterModel.Health = maxHealth;
        }

        public override void GetHit(int damage)
        {
            base.GetHit(damage);
            signalBus.Fire(new PlayerGotDamage() { damage = damage });
        }

        protected override void Die()
        {
            base.Die();
            signalBus.Fire(new PlayerKilled());
        }
    }
}