using ShootyMood.Scripts.Models.Base;
using UnityEngine;

namespace ShootyMood.Scripts.Handlers.Base
{
    public abstract class BaseCharacterHitHandler<K> : MonoBehaviour where K : BaseCharacterModel
    {
        [SerializeField] private BaseCharacterModel characterModel;
        protected K CharacterModel => characterModel as K;
        
        public virtual void GetHit(int damage)
        {
            characterModel.Health -= damage;
            if (characterModel.Health <= 0)
            {
                Die();
            }
        }
        
        protected virtual void Die()
        {
            //TODO : Call on death!
        }
    }
}