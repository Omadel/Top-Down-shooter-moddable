using UnityEngine;

namespace Etienne.Animator2D
{
    public class Animation2D : ScriptableObject
    {
        public int FPS => fps;
        public virtual Sprite[] Sprites => sprites;

        [SerializeField, Range(1, 90), Delayed] int fps = 12;
        [SerializeField] protected Sprite[] sprites;

        public void SetSprites(Sprite[] sprites) => this.sprites = sprites;
    }
}
