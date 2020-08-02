using Code.ECS.EntityViewComponents.Components;
using Svelto.ECS;
using Svelto.ECS.Hybrid;

namespace Code.ECS.EntityViewComponents
{
	internal struct PositionEntityViewComponent : IEntityViewComponent
	{
		public IPositionComponent PositionComponent;
		public EGID ID { get; set; }
	}
}

