using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Etienne.Animator2D
{
    using AnimationState2D = AnimatorController2D.AnimationState2D;
    public class Animator2D : MonoBehaviour
    {
        public AnimationState2D NextAnimationState => nextAnimationState;
        public AnimationState2D CurrentAnimationState => currentAnimationState;
        public AnimationState2D PreviousAnimationState => previousAnimationState;
        public SpriteRenderer Renderer => renderers[0];
        public int CurrentFrameIndex => currentFrameIndex;

        [SerializeField] private AnimatorController2D controller;
        private AnimationState2D currentAnimationState, nextAnimationState, previousAnimationState;
        private List<SpriteRenderer> renderers = new List<SpriteRenderer>();
        private int currentFrameIndex;
        private Image imageRenderer;
        private Dictionary<string, AnimationState2D> animationStates;
        private Coroutine routine;

        private void Start()
        {
            renderers.AddRange(GetComponentsInChildren<SpriteRenderer>(true));
            if (renderers[0] == null) imageRenderer = GetComponentInChildren<Image>();
            animationStates = new Dictionary<string, AnimationState2D>();
            foreach (AnimationState2D animationState in controller.AnimationStates)
            {
                animationStates.Add(animationState.Name, animationState);
            }
            currentAnimationState = controller.AnimationStates[0];
            RestartUpdateRoutine();
        }

        private void OnEnable()
        {
            if (!Application.isPlaying || renderers.Count == 0) return;
            currentAnimationState = controller.AnimationStates[0];
            RestartUpdateRoutine();
        }

        private void OnDisable()
        {
            if (!Application.isPlaying) return;
            if (routine != null) StopCoroutine(routine);
        }

        private IEnumerator UpdateRoutine()
        {
            int index = 0;
            while (enabled && Application.isPlaying)
            {
                if (Renderer != null) yield return new WaitUntil(() => Renderer.isVisible);
                Animation2D animation = currentAnimationState.Animation;
                bool isLayered = animation is LayeredAnimation layeredAnimation;
                for (int i = 1; i < renderers.Count; i++)
                {
                    renderers[i].sprite = null;
                }
                if (animation == null)
                {
                    Renderer.sprite = null;
                    gameObject.SetActive(false);
                    break;
                }

                if (!currentAnimationState.IsLooping && index + 1 >= animation.Sprites.Length)
                {
                    if (currentAnimationState.OutStateName != string.Empty) SetState(currentAnimationState.OutStateName, true);
                    break;
                }

                float frameTime = 1000 / animation.FPS / 1000f;
                index %= animation.Sprites.Length;
                currentFrameIndex = index;
                if (Renderer == null) imageRenderer.sprite = animation.Sprites[index];
                else
                {
                    SetAnimationToRenderers(renderers, animation, index);
                }
                ++index;
                yield return new WaitForSeconds(frameTime);
            }
        }

        private void SetAnimationToRenderers(List<SpriteRenderer> renderers, Animation2D animation, int index)
        {
            bool isLayered = animation is LayeredAnimation;
            if (!isLayered)
            {
                renderers[0].sprite = animation.Sprites[index];
                for (int i = 1; i < renderers.Count; i++)
                {
                    renderers[i].sprite = null;
                }
            }
            else
            {
                LayeredAnimation layeredAnimation = animation as LayeredAnimation;

                for (int i = renderers.Count; i < layeredAnimation.Animations.Length; i++)
                {
                    Transform go = new GameObject($"AnimationLayer ({i})").transform;
                    go.SetParent(transform);
                    go.rotation = transform.rotation;
                    go.localPosition = Vector3.zero;
                    go.localScale = Vector3.one;
                    renderers.Add(go.gameObject.AddComponent<SpriteRenderer>());
                }
                var orderInLayer = renderers[0].sortingOrder;
                for (int i = 0; i < layeredAnimation.Animations.Length; i++)
                {
                    var sprite = layeredAnimation.Animations[i] == null ? null : layeredAnimation.Animations[i].Sprites[index];
                    renderers[i].sprite = sprite;
                    renderers[i].sortingOrder = orderInLayer + i;
                }
            }

        }

        public bool FlipY(bool? value = null)
        {
            bool oldValue = Renderer.flipY;
            foreach (var renderer in renderers)
            {
                renderer.flipY = value ?? !renderer.flipY;
            }
            return Renderer.flipY != oldValue;
        }
        public bool FlipX(bool? value = null)
        {
            bool oldValue = Renderer.flipX;
            foreach (var renderer in renderers)
            {
                renderer.flipX = value ?? !renderer.flipX;
            }
            return Renderer.flipX != oldValue;
        }

        public void SetState(string stateName, bool force = false)
        {
            if (currentAnimationState.Name == stateName) force = false;
            previousAnimationState = currentAnimationState;
            currentAnimationState = animationStates[stateName];
            nextAnimationState = currentAnimationState.OutStateName == "" ? null : animationStates[currentAnimationState.OutStateName];
            if (force) RestartUpdateRoutine();
        }

        public void SetNextState(string nextStateName)
        {
            if (nextAnimationState.Name == nextStateName) return;
            nextAnimationState = animationStates[nextStateName];
        }

        public string GetState()
        {
            return currentAnimationState.Name;
        }

        private void RestartUpdateRoutine()
        {
            if (routine != null) StopCoroutine(routine);
            routine = StartCoroutine(UpdateRoutine());
        }

#if UNITY_EDITOR
        private async void OnValidate()
        {
            await System.Threading.Tasks.Task.Delay(10);
            if (this == null) return;
            Animation2D animation = controller?[0]?.Animation;
            bool isNull = animation == null
                || animation.Sprites == null
                || animation.Sprites.Length <= 0
                || animation.Sprites[0] == null;

            List<SpriteRenderer> renderers = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>(true));
            Sprite sprite = isNull ? null : animation.Sprites[0];
            if (renderers[0] != null)
            {
                if (isNull)
                {
                    foreach (var renderer in renderers)
                    {
                        renderer.sprite = null;
                    }
                }
                else
                {
                    SetAnimationToRenderers(renderers, animation, 1);
                }
            }
            else GetComponentInChildren<Image>().sprite = sprite;
        }
#endif
    }
}