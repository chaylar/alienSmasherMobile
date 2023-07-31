using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ShootyMood.Scripts.Config.Wave;
using ShootyMood.Scripts.Models;
using UnityEngine;
using Zenject;

namespace ShootyMood.Scripts.Handlers
{
    public class PositionSpawnService : MonoBehaviour, IInitializable
    {
        [Inject] private SpawnPointConfig spConfig;
        private List<EnemySpawnPosition> spawnPositions;

        public void Initialize()
        {
            //TODO : Get scene 
            //TODO : Generate EnemySpawnPosition & Start spawning
            GenerateSpawnPoints();
        }

        private void GenerateSpawnPoints()
        {
            spawnPositions = new List<EnemySpawnPosition>();
            Vector2 worldBoundary = Camera.main.ScreenToWorldPoint( new Vector2( Screen.width, Screen.height ));
            for (float x = (spConfig.EnemyModelWidth - worldBoundary.x); x < (worldBoundary.x - spConfig.EnemyModelWidth); x += spConfig.EnemyModelWidth)
            {
                for (float y = (spConfig.EnemyModelHeight - worldBoundary.y); y <= (worldBoundary.y - spConfig.EnemyModelHeight); y += spConfig.EnemyModelHeight)
                {
                    EnemySpawnPosition spPos = new EnemySpawnPosition() { X = x, Y = y };
                    spawnPositions.Add(spPos);
                }
            }
        }

        public void SpawnOnPos(ref EnemySpawnPosition spawnPos, EnemyModel enemyModel)
        {
            spawnPos.ActiveEnemy = enemyModel;
            SummonComplete(enemyModel);
        }
        
        public void ResetSpawns()
        {
            for (int i = 0; i < spawnPositions.Count; i++)
            {
                if(spawnPositions[i].ActiveEnemy != null)
                {
                    Destroy(spawnPositions[i].ActiveEnemy.gameObject);
                    spawnPositions[i].ActiveEnemy = null;
                }
            }
        }

        private void SummonComplete(EnemyModel enemyModel)
        {
            SpriteRenderer goSprite = enemyModel.GetComponent<SpriteRenderer>();
            Color origColor = goSprite.color;
            goSprite.color = new Color(origColor.r, origColor.g, origColor.b, 0f);
            enemyModel.gameObject.SetActive(true);

            goSprite.DOColor(origColor, spConfig.SpawnDuration);
        }

        public List<EnemySpawnPosition> GetAvailablePositions()
        {
            if (spawnPositions == null)
            {
                return new List<EnemySpawnPosition>();
            }
            
            return spawnPositions.Where(x => x.ActiveEnemy == null || x.ActiveEnemy.IsDead).ToList();
        }
    }
}