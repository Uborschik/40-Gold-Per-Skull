using UnityEngine;

namespace Game.Combat.Entities.Selector
{
    [RequireComponent(typeof(Transform))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class SelectionView : MonoBehaviour
    {
        [SerializeField] private Color unitColor = Color.white;
        [SerializeField] private Color activeColor = Color.cyan;
        [SerializeField] private Color invalidColor = Color.red;

        private Transform thisTransform;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            thisTransform = transform;
            spriteRenderer = GetComponent<SpriteRenderer>();

            Debug.Log($"[CellSelectionView] Created");
        }

        private void Start()
        {
            Hide();
        }

        public void Set(Vector2 position, SelectionType type)
        {
            Show();
            thisTransform.position = position;

            switch (type)
            {
                case SelectionType.Unit:
                    spriteRenderer.color = unitColor;
                    break;
                case SelectionType.Free:
                    spriteRenderer.color = activeColor;
                    break;
                case SelectionType.Blocked:
                    spriteRenderer.color = invalidColor;
                    break;
            }
        }

        public void Show() => spriteRenderer.enabled = true;

        public void Hide() => spriteRenderer.enabled = false;
    }
}