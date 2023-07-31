using ShootyMood.Scripts.Config.Wave;
using UnityEngine;
using Zenject;

namespace ShootyMood.Scripts.Installers
{
    public class ConfigsInstaller : MonoInstaller
    {
        [SerializeField] private SpawnPointConfig spawnPointConfig;
        [SerializeField] private WavesConfig wavesConfig;
        public override void InstallBindings()
        {
            Container.Bind<SpawnPointConfig>().FromInstance(spawnPointConfig).AsSingle();
            Container.Bind<WavesConfig>().FromInstance(wavesConfig).AsSingle();
        }
    }
}