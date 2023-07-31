using System.Collections;
using System.Collections.Generic;
using ShootyMood.Scripts.Models.Base;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyModel : BaseCharacterModel
{
    [SerializeField] private float attackDelay;
    [SerializeField] private float readyDelay;

    [SerializeField] private float escapeTime;
    [SerializeField] private float timeAddition;

    private bool isDead;
    private bool isDying;

    [DoNotSerialize] public float AttackDelayDecrementRatio = 0f;
    [DoNotSerialize] public float EscapeDelayDecrementRatio = 0f;
    [DoNotSerialize] public bool isFriendly;

    public float AttackDelay =>
        AttackDelayDecrementRatio < 1 && AttackDelayDecrementRatio >= 0 
            ? attackDelay - (attackDelay * AttackDelayDecrementRatio)
            : attackDelay;

    public float ReadyDelay => readyDelay;

    public float EscapeTime => EscapeDelayDecrementRatio < 1 && EscapeDelayDecrementRatio >= 0
            ? escapeTime - (escapeTime * EscapeDelayDecrementRatio)
            : escapeTime;

    public float TimeAddition
    {
        get => timeAddition;
        set => timeAddition = value;
    }

    public bool IsDying
    {
        get => isDying;
        set => isDying = value;
    }

    public bool IsDead
    {
        get => isDead;
        set => isDead = value;
    }
}
