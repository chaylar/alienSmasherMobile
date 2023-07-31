using UnityEngine;
using Zenject;

namespace ShootyMood.Scripts.Installers
{
    public class PrefabsInstaller : MonoInstaller
    {
        [SerializeField] private EnemyModel enemyPrefab;
        
        public override void InstallBindings()
        {
            Container.Bind<EnemyModel>().FromComponentInNewPrefab(enemyPrefab).AsTransient();
        }
    }
}