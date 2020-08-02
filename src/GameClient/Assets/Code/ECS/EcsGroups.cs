using Svelto.ECS;

namespace Code.ECS
{
	internal class EcsGroups
	{
		public static readonly ExclusiveGroup Camera = new ExclusiveGroup();
		
		public static readonly ExclusiveGroupStruct PlayersGroup = new ExclusiveGroup();
	}
}
