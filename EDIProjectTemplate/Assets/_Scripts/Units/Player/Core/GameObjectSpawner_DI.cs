using PixPlays.ElementalVFX;
using UnityEngine;
using Zenject;

namespace _Scripts.Units.Player.Core
{
    public class GameObjectSpawner_DI
    {
        [Inject] private DiContainer _container;
        //I can make the spawn generic but not really needed with only 2 cases
        public GameObject Spawn(GameObject prefab, Transform parent = null)
        {
            return _container.InstantiatePrefab(prefab, parent);
        }
        public GameObject Spawn(BaseVfx baseVfx, Transform parent = null)
        {
            return _container.InstantiatePrefab(baseVfx.gameObject, parent);
        }
        public T AddComponent<T>(GameObject gameObject) where T : Component
        {
            return _container.InstantiateComponent<T>(gameObject);
        }
        
        
    }
}