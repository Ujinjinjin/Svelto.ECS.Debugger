using Svelto.Context;
using Svelto.ECS;

namespace Code.EditorTools
{
	public static class ObservableEnginesSingletonContext
	{
		public static ICompositionRoot CompositionRoot { get; set; }
		public static EnginesRoot EnginesRoot { get; set; }

		public static ICompositionRoot EnableSveltoDebugWindow(this ICompositionRoot compositionRoot, EnginesRoot enginesRoot)
		{
			CompositionRoot = compositionRoot;
			EnginesRoot = enginesRoot;

			return compositionRoot;
		}
	}
}
