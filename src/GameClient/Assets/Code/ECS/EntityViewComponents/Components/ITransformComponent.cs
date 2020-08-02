using System.Numerics;

namespace Code.ECS.EntityViewComponents.Components
{
	internal interface ITransformComponent : IPositionComponent
	{
		Quaternion Rotation { get; }
	}
}
