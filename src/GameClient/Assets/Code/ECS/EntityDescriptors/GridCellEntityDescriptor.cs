using Code.ECS.EntityViewComponents;
using Svelto.ECS;

namespace Code.ECS.EntityDescriptors
{
	internal class GridCellEntityDescriptor : IEntityDescriptor
	{
		private static readonly IComponentBuilder[] _componentsToBuild =
		{
			new ComponentBuilder<HoverableEntityViewComponent>(), 
			new ComponentBuilder<PositionEntityViewComponent>(), 
		};

		public IComponentBuilder[] componentsToBuild => _componentsToBuild;
	}
}

