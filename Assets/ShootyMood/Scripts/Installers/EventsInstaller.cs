using ShootyMood.Scripts.ShootyGameEvents;
using Zenject;

namespace ShootyMood.Scripts.Installers
{
    public class EventsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            InstallEvents();
        }

        private void InstallEvents()
        {
            Container.DeclareSignal<EnemyKilled>().OptionalSubscriber();
            Container.DeclareSignal<EnemyEscaped>().OptionalSubscriber();
            Container.DeclareSignal<EnemySpawned>().OptionalSubscriber();
            Container.DeclareSignal<PlayerKilled>().OptionalSubscriber();
            Container.DeclareSignal<PlayerGotDamage>().OptionalSubscriber();
            Container.DeclareSignal<GameStarted>().OptionalSubscriber();
            Container.DeclareSignal<PlayButtonClickEvent>().OptionalSubscriber();
            Container.DeclareSignal<AudioOptionChanged>().OptionalSubscriber();
        }
    }
}