using System.Collections.Generic;
using UnityEngine;

namespace Code.Factories
{
	internal interface IGameObjectFactory
	{
		IEnumerator<GameObject> Build(string prefabName);
		IEnumerator<GameObject> Load(string prefabName);
		GameObject Instantiate(GameObject gameObject);
	}
}
