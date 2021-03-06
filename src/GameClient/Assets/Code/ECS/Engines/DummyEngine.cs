﻿using Code.ECS.EntityViewComponents;
using Code.EditorTools.Attributes;
using Code.Infrastructure;
using Svelto;
using Svelto.ECS;
using Svelto.Tasks.ExtraLean;
using System.Collections;

namespace Code.ECS.Engines
{
	[ObservableEngine(typeof(HoverableEntityViewComponent), typeof(PositionEntityViewComponent), typeof(EcsGroups), nameof(EcsGroups.GridGroup))]
	[ObservableEngine(typeof(HoverableEntityViewComponent), typeof(EcsGroups), nameof(EcsGroups.GridGroup))]
	[ObservableEngine(typeof(PositionEntityViewComponent), typeof(EcsGroups), nameof(EcsGroups.GridGroup))]
	internal class DummyEngine : IQueryingEntitiesEngine
	{
		public EntitiesDB entitiesDB { get; set; }

		public void Ready()
		{
			Tick().RunOn(SveltoRunners.MainThreadRunner);
		}

		private IEnumerator Tick()
		{
			while (true)
			{
				var (hoverableComponents, positionComponents, count) = entitiesDB.QueryEntities<HoverableEntityViewComponent, PositionEntityViewComponent>(EcsGroups.GridGroup);

				for (var i = 0; i < count; i++)
				{
					if (hoverableComponents[i].HoverableComponent.Hovered)
					{
						Console.Log(positionComponents[i].PositionComponent.Position.ToString());
					}
				}
				
				yield return null;
			}

			// ReSharper disable once IteratorNeverReturns
		}
	}
}

