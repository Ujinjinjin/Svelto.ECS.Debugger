using Code.Infrastructure;
using Svelto;
using Svelto.ECS;
using Svelto.Tasks.ExtraLean;
using System.Collections;

namespace Code.ECS.Engines
{
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
				Console.LogDebug("Dummy engine");
				yield return null;
			}

			// ReSharper disable once IteratorNeverReturns
		}
	}
}

