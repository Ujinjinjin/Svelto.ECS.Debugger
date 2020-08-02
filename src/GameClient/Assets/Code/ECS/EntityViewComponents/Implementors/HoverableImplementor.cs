using Code.ECS.EntityViewComponents.Components;
using Svelto.ECS.Hybrid;
using UnityEngine;

namespace Code.ECS.EntityViewComponents.Implementors
{
    public class HoverableImplementor : MonoBehaviour, IImplementor, IHoverableComponent
    {
        private void OnMouseEnter()
        {
            Hovered = true;
        }

        private void OnMouseExit()
        {
            Hovered = false;
        }

        public bool Hovered { get; private set; }
    }
}
