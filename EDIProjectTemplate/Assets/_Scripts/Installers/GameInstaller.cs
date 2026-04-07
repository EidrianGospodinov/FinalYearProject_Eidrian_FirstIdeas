using System.ComponentModel;
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

    }
}