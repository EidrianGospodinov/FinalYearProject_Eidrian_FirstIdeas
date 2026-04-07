using System.ComponentModel;
using _Scripts.New_Folder.Checkpoint;
using _Scripts.Units.Enemy;
using _Scripts.Units.Player.Core;
using Unity.VisualScripting;
using UnityEngine;
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

    }
}