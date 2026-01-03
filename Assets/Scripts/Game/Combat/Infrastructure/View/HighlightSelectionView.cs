using Game.Combat.Core.Entities;
using Game.Combat.Infrastructure.Input;
using UnityEngine;

namespace Game.Combat.Infrastructure.View
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class HighlightSelectionView : MonoBehaviour, IMovableObject
    {
        [SerializeField] private Color activeColor = Color.cyan;
        [SerializeField] private Color invalidColor = Color.red;

        private Transform thisTransform;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            thisTransform = transform;
            spriteRenderer = GetComponent<SpriteRenderer>();

            if (spriteRenderer == null)
            {
                Debug.LogError("SpriteRenderer not found on " + gameObject.name, gameObject);
                return;
            }

            Debug.Log($"[HighlightView] Created");
        }

        private void Start()
        {
            Hide();
        }

        public void Set(Vector2 position, HighlightType type)
        {
            Show();

            SetPosition(position);
            spriteRenderer.color = type == HighlightType.Free ? activeColor : invalidColor;
        }

        public void SetPosition(Vector2 position) => thisTransform.position = position;

        public void Show() => spriteRenderer.enabled = true;

        public void Hide() => spriteRenderer.enabled = false;
    }
}