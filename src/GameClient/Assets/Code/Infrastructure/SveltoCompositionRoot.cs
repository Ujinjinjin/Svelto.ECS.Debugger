using Code.ECS.Engines;
using Svelto.Context;
using Svelto.ECS;
using Svelto.ECS.Schedulers.Unity;

// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace Code.Infrastructure
{
	internal class SveltoCompositionRoot : ICompositionRoot
	{
		private EnginesRoot _enginesRoot; 
		
		public void OnContextInitialized<T>(T contextHolder)
		{
			InitializeCompositionRoot(contextHolder as UnityContext);
		}

		private void InitializeCompositionRoot(UnityContext contextHolder)
		{
			var unityEntitiesSubmissionScheduler = new UnityEntitiesSubmissionScheduler();
			_enginesRoot = new EnginesRoot(unityEntitiesSubmissionScheduler);

			var entityFactory = _enginesRoot.GenerateEntityFactory();
			var entityFunctions = _enginesRoot.GenerateEntityFunctions();
			var entityStreamConsumerFactory = _enginesRoot.GenerateConsumerFactory();
			
			// Create engines
			var dummyEngine = new DummyEngine();
			
			// Register engines
			_enginesRoot.AddEngine(dummyEngine);
		}

		public void OnContextDestroyed()
		{
			_enginesRoot.Dispose();
			SveltoRunners.StopAndCleanupAllRunners();
		}

		public void OnContextCreated<T>(T contextHolder)
		{
		}
	}
}
