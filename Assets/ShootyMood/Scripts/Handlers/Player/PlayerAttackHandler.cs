using System;
using ShootyMood.Scripts.Handlers.Base;
using ShootyMood.Scripts.Models;
using Unity.VisualScripting;
using UnityEngine;

namespace ShootyMood.Scripts.Handlers
{
    public class PlayerAttackHandler : BaseCharacterAttackHandler<PlayerModel>
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null && hit.collider.CompareTag("Enemy"))
                {
                    var enemyHitHandler = hit.collider.GetComponent<EnemyHitHandler>();
                    if (enemyHitHandler != null)
                    {
                        enemyHitHandler.GetHit(CharacterModel.Damage);
                    }
                }
            }
        }
    }
}