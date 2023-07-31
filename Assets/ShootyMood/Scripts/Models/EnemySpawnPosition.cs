using UnityEngine;

namespace ShootyMood.Scripts.Models
{
    public class EnemySpawnPosition
    {
        private float x;
        private float y;
        
        private EnemyModel activeEnemy;

        public float X
        {
            get => x;
            set => x = value;
        }

        public float Y
        {
            get => y;
            set => y = value;
        }

        public EnemyModel ActiveEnemy
        {
            get => activeEnemy;
            set => activeEnemy = value;
        }
    }
}