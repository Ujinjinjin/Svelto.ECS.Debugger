using Svelto.ECS;

namespace Code.ECS
{
	public static class EcsGroups
	{
		public static readonly ExclusiveGroup Camera = new ExclusiveGroup();
		
		public static readonly ExclusiveGroupStruct PlayersGroup = new ExclusiveGroup();
		public static readonly ExclusiveGroupStruct GridGroup = new ExclusiveGroup();
	}
}
