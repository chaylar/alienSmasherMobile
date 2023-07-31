using ShootyMood.Scripts.Config.Base;
using ShootyMood.Scripts.Models.Base;
using UnityEngine;

namespace ShootyMood.Scripts.Handlers.Base
{
    public abstract class BaseCharacterAttackHandler<K> : MonoBehaviour where K : BaseCharacterModel
    {
        [SerializeField] private BaseCharacterModel characterModel;
        protected K CharacterModel => characterModel as K;
    }
}