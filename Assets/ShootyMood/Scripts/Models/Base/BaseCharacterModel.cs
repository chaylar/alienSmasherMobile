using System;
using ShootyMood.Scripts.Config.Base;
using UnityEngine;

namespace ShootyMood.Scripts.Models.Base
{
    public abstract class BaseCharacterModel : MonoBehaviour
    {
        [SerializeField] private int health;
        [SerializeField] protected int damage;

        public int Health
        {
            get => health;
            set => health = value;
        }

        public int Damage
        {
            get => damage;
            set => damage = value;
        }
    }
}