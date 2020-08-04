using Code.Infrastructure;

namespace Code
{
	public static class CompositionRootHolder
	{
		public static SveltoCompositionRoot CompositionRoot { get; set; }

		public static void SetCompositionRoot(SveltoCompositionRoot compositionRoot)
		{
			CompositionRoot = compositionRoot;
		}
	}
}
