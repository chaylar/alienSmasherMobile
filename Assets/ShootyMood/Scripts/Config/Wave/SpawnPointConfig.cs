using UnityEngine;
using Zenject;

namespace ShootyMood.Scripts.Config.Wave
{
    [CreateAssetMenu(fileName = "SpawnPointConfig", menuName = "SlimeHunter/Configs/SpawnPointConfig")]
    public class SpawnPointConfig : ScriptableObjectInstaller
    {
        [SerializeField] private float enemyModelWidth;
        [SerializeField] private float enemyModelHeight;
        [SerializeField] private float spawnPosDeviation = 0f;
        [SerializeField] private float spawnDuration = .1f;

        public float EnemyModelWidth => enemyModelWidth;

        public float EnemyModelHeight => enemyModelHeight;

        public float SpawnPosDeviation => spawnPosDeviation;

        public float SpawnDuration => spawnDuration;
    }
}