using Code.ECS.EntityDescriptors;
using Code.Factories;
using Code.Infrastructure;
using Code.Other.Constants;
using Svelto.ECS;
using Svelto.ECS.Hybrid;
using Svelto.Tasks.ExtraLean;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.ECS.Engines
{
	internal class GridSpawnEngine : IQueryingEntitiesEngine
	{
		public EntitiesDB entitiesDB { get; set; }
		
		private readonly IEntityFactory _entityFactory;
		private readonly IGameObjectFactory _gameObjectFactory;
		
		private const int FieldLength = 20;
		private const int FieldWidth = 20;

		public GridSpawnEngine(IGameObjectFactory gameObjectFactory, IEntityFactory entityFactory)
		{
			_gameObjectFactory = gameObjectFactory;
			_entityFactory = entityFactory;
		}

		public void Ready()
		{
			Tick().RunOn(SveltoRunners.MainThreadRunner);
		}

		private IEnumerator Tick()
		{
			var loadingAsync = _gameObjectFactory.Load(AddressablePrefab.GridCell);
			while (loadingAsync.MoveNext()) yield return null;
			var gridCell = loadingAsync.Current;
			
			for (var i = 0; i < FieldLength; i++)
			{
				for (var j = 0; j < FieldWidth; j++)
				{
					var instantiatedGridCell = _gameObjectFactory.Instantiate(gridCell);
					var implementors = new List<IImplementor>();
					instantiatedGridCell.GetComponentsInChildren(true, implementors);
			
					_entityFactory.BuildEntity<GridCellEntityDescriptor>((uint) instantiatedGridCell.GetInstanceID(), EcsGroups.GridGroup, implementors);

					instantiatedGridCell.transform.position = new Vector3(-FieldLength / 2 + i, 0, -FieldWidth / 2 + j);
				}
			}
		}
	}
}
