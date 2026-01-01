using UnityEngine;

namespace Game.Unity.Combat.Views
{
    [RequireComponent(typeof(Transform))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class CellSelectionView : MonoBehaviour
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

            Debug.Log($"[CellSelectionView] Created");
        }

        private void Start()
        {
            Hide();
        }

        public void Set(Vector2 position, bool isActiveColor)
        {
            Show();
            thisTransform.position = position;
            spriteRenderer.color = isActiveColor ? activeColor : invalidColor;
        }

        public void Show() => spriteRenderer.enabled = true;

        public void Hide() => spriteRenderer.enabled = false;
    }
}