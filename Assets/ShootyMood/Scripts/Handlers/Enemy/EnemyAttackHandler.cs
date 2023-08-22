using System;
using DG.Tweening;
using ShootyMood.Scripts.Handlers.Base;
using ShootyMood.Scripts.Models.Base;
using UnityEngine;
using Zenject;

namespace ShootyMood.Scripts.Handlers
{
    public class EnemyAttackHandler : BaseCharacterAttackHandler<EnemyModel>
    {
        [SerializeField] private Animator attackAnimator;

        [SerializeField] private float readyScaleMultiplier = 1.1f;
        [SerializeField] private float attackScaleMultiplier = 1.2f;
        
        [Inject] private PlayerHitHandler playerHitHandler;

        private float attackTimer = 0;
        private bool isReady = false;
        private bool attackCompleted = false;
        private float totalAttackDelay = float.MaxValue;
        private readonly string attackAnimTriggerParam = "Attack";
        private readonly string readyAnimTriggerParam = "ReadyAttack";
        
        private Vector3 startingScale;

        private void Start()
        {
            startingScale = transform.localScale;
            totalAttackDelay = CharacterModel.AttackDelay + CharacterModel.ReadyDelay;
        }

        void FixedUpdate()
        {
            if (CharacterModel.IsDead || CharacterModel.IsDying)
            {
                return;
            }

            if (attackCompleted && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                attackCompleted = false;
                transform.DOKill();
                transform.localScale = startingScale;
            }
            
            if (attackTimer >= totalAttackDelay && isReady)
            {
                attackAnimator.SetTrigger(attackAnimTriggerParam);
                attackTimer = 0f;
                isReady = false;
                attackCompleted = true;
                transform.DOKill();
                transform.localScale = startingScale * attackScaleMultiplier;
                
                playerHitHandler.GetHit(CharacterModel.Damage);
            }
            else if (attackTimer >= CharacterModel.ReadyDelay && !isReady)
            {
                isReady = true;
                attackAnimator.SetTrigger(readyAnimTriggerParam);
                transform.DOScale(startingScale * readyScaleMultiplier, CharacterModel.AttackDelay);
                transform.DOShakePosition(CharacterModel.AttackDelay, .06f);
            }

            attackTimer += Time.deltaTime;
        }
    }
}