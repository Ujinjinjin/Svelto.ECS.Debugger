using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.Factories
{
    internal class GameObjectFactory : IGameObjectFactory
    {
        private readonly Dictionary<string, GameObject> _prefabs;

        public GameObjectFactory()
        {
            _prefabs = new Dictionary<string, GameObject>();
        }

        public IEnumerator<GameObject> Build(string prefabName)
        {
            if (_prefabs.TryGetValue(prefabName, out var go) == false)
            {
                var load = Addressables.LoadAssetAsync<GameObject>(prefabName);

                while (load.IsDone == false) yield return null;

                go = load.Result;
                
                _prefabs.Add(prefabName, go);
            }

            yield return Object.Instantiate(go);
        }

        public IEnumerator<GameObject> Load(string prefabName)
        {
            if (_prefabs.TryGetValue(prefabName, out var go) == false)
            {
                var load = Addressables.LoadAssetAsync<GameObject>(prefabName);

                while (load.IsDone == false) yield return null;

                go = load.Result;
                
                _prefabs.Add(prefabName, go);
            }

            yield return go;
        }

        public GameObject Instantiate(GameObject gameObject)
        {
            return Object.Instantiate(gameObject);
        }
    }
}