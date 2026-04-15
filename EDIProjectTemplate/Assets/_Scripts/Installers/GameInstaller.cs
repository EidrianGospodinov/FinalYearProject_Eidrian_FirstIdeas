using System.ComponentModel;
using _Scripts;
using _Scripts.New_Folder.Checkpoint;
using _Scripts.Units.Enemy;
using _Scripts.Units.Player.Core;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Debug.Log("Installer is actually running!");
       
        Container.BindInterfacesAndSelfTo<CurrencyManager>().AsSingle().NonLazy();
        Container.Bind<RespawnService>().AsSingle();
        Container.Bind<EnemyManager>().AsSingle().NonLazy();
        Container.Bind<TeleporterService>().AsSingle();
        Container.Bind<GameManager>().AsSingle();
        Container.Bind<DynamicTextServices>().AsSingle().NonLazy();
        Container.Bind<GameObjectSpawner_DI>().AsSingle();
        Container.Bind<PlayerServices>().AsSingle();
        Container.Bind<ICameraService>()
            .To<CameraManager>()
            .FromComponentsInHierarchy()
            .AsSingle();
        Container.Bind<IDeathMenu>()
            .To<RespawnPanel>()
            .FromComponentInHierarchy()
            .AsSingle();
        Container.Bind<QuestManager>()
            .AsSingle()
            .NonLazy();

    }
}