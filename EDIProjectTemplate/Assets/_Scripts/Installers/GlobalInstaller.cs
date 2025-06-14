using System.ComponentModel;
using Unity.VisualScripting;
using Zenject;

public class GlobalInstaller : MonoInstaller<GlobalInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<PlayerState>().To<PlayerState>().AsSingle();
        //Container.Bind<IEventBus>().To<EventBusSystem<>>().AsSingle();
        
    }
}