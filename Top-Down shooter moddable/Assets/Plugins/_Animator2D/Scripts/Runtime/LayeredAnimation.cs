using UnityEngine;

namespace Etienne.Animator2D
{
    //TODO: create from animation selection
    [CreateAssetMenu(menuName = "Etienne/2D/Animator/Layered animation")]
    public class LayeredAnimation : Animation2D
    {
        public override Sprite[] Sprites => animationLayers[0].Sprites;
        public Animation2D[] Animations => animationLayers;
        [SerializeField] Animation2D[] animationLayers;
    }
}
