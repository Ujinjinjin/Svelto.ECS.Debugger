using Svelto.Tasks.ExtraLean.Unity;

namespace Code.Infrastructure
{
	internal static class SveltoRunners
	{
		public static readonly UpdateMonoRunner MainThreadRunner = new UpdateMonoRunner("MainThreadRunner");

		public static void StopAndCleanupAllRunners()
		{
			MainThreadRunner.Dispose();
		}
	}
}
