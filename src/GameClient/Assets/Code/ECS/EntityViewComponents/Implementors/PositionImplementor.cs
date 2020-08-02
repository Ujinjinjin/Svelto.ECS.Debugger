using Code.ECS.EntityViewComponents.Components;
using Svelto.ECS.Hybrid;
using UnityEngine;

namespace Code.ECS.EntityViewComponents.Implementors
{
	internal class PositionImplementor : MonoBehaviour, IImplementor, IPositionComponent
	{
		private Transform _transform;

		public Vector3 Position => _transform.position;

		private void Awake()
		{
			_transform = transform;
		}
	}
}
