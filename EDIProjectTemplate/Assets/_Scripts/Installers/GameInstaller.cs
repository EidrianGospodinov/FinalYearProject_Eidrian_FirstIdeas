using System.ComponentModel;
using _Scripts.Units.Enemy;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Debug.LogError("Installer is actually running!");
       
        Container.BindInterfacesAndSelfTo<CurrencyManager>().AsSingle().NonLazy();
        Container.Bind<RespawnService>().AsSingle();
        Container.Bind<EnemyManager>().AsSingle().NonLazy();

    }
}