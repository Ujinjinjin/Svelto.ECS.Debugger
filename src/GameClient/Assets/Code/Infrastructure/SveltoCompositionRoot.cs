﻿using Code.ECS;
using Code.ECS.Engines;
using Svelto.Context;
using Svelto.ECS;
using Svelto.ECS.Hybrid;
using Svelto.ECS.Schedulers.Unity;
using System;
using UnityEngine;

// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace Code.Infrastructure
{
	internal class SveltoCompositionRoot : ICompositionRoot
	{
		private EnginesRoot _enginesRoot;
		private UnityEntitiesSubmissionScheduler _unityEntitiesSubmissionScheduler;
		
		public void OnContextInitialized<T>(T contextHolder)
		{
			InitializeCompositionRoot(contextHolder as UnityContext);
		}

		private void InitializeCompositionRoot(UnityContext contextHolder)
		{
			_unityEntitiesSubmissionScheduler  = new UnityEntitiesSubmissionScheduler();
			_enginesRoot = new EnginesRoot(_unityEntitiesSubmissionScheduler);

			var entityFactory = _enginesRoot.GenerateEntityFactory();
			var entityFunctions = _enginesRoot.GenerateEntityFunctions();
			var entityStreamConsumerFactory = _enginesRoot.GenerateConsumerFactory();
			
			// Create engines
			var dummyEngine = new DummyEngine();
			
			// Register engines
			_enginesRoot.AddEngine(dummyEngine);
			
			BuildGridFromScene(contextHolder, entityFactory);
		}		
		
		/// <summary> Create grid cell entities from scene </summary>
		[Obsolete("Grid should be created from engine and this method will be deleted")]
		private void BuildGridFromScene(UnityContext unityContext, IEntityFactory entityFactory)
		{
			var entities = unityContext.GetComponentsInChildren<IEntityDescriptorHolder>();

			foreach (var entityHolder in entities)
			{
				entityFactory.BuildEntity(
					new EGID((uint) ((MonoBehaviour) entityHolder).gameObject.GetInstanceID(), EcsGroups.GridGroup),
					entityHolder.GetDescriptor(),
					((MonoBehaviour) entityHolder).GetComponents<IImplementor>()
				);
			}
		}

		public void OnContextDestroyed()
		{
			_enginesRoot.Dispose();
			_unityEntitiesSubmissionScheduler.Dispose();
			SveltoRunners.StopAndCleanupAllRunners();
		}

		public void OnContextCreated<T>(T contextHolder)
		{
		}
	}
}
