using ShootyMood.Scripts.Handlers.Base;
using ShootyMood.Scripts.ShootyGameEvents;
using UnityEngine;
using Zenject;

namespace ShootyMood.Scripts.Handlers
{
    public class EnemyHitHandler : BaseCharacterHitHandler<EnemyModel>
    {
        [SerializeField] private GameObject deathParticle;
        [Inject] private SignalBus signalBus;
        
        protected override void Die()
        {
            CharacterModel.IsDying = true;
            base.Die();

            CharacterModel.IsDead = true;
            var position = gameObject.transform.position;

            if (deathParticle != null)
            {
                Instantiate(deathParticle, position, Quaternion.identity);
            }
            
            signalBus.Fire(new EnemyKilled() { isFriendly = CharacterModel.isFriendly, timeAddition = CharacterModel.TimeAddition });
            Destroy(gameObject);
        }
    }
}