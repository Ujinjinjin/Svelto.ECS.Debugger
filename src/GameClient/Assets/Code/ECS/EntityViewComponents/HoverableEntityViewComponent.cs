using Code.ECS.EntityViewComponents.Components;
using Svelto.ECS;
using Svelto.ECS.Hybrid;

namespace Code.ECS.EntityViewComponents
{
	internal struct HoverableEntityViewComponent : IEntityViewComponent
	{
		public IHoverableComponent HoverableComponent;
		public EGID ID { get; set; }
	}
}
