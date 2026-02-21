using UnityEngine;

namespace APGame.InGame
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class CreateShadow : MonoBehaviour
    {
        private readonly Vector2 SHADOW_OFFSET = new(0.1f, -0.1f);
        private const float SHADOW_ALPHA = 0.5f;
        
        [SerializeField] private float _ShadowOffsetMultiplier = 1;

        private GameObject _shadowObject;
        private SpriteRenderer _shadowRenderer;
        private SpriteRenderer _parentRenderer;

        void Start()
        {
            _parentRenderer = GetComponent<SpriteRenderer>();
            SetupShadow();
        }

        void SetupShadow()
        {
            _shadowObject = new GameObject(gameObject.name + "_Shadow");
            _shadowObject.transform.SetParent(transform);

            _shadowRenderer = _shadowObject.AddComponent<SpriteRenderer>();
            _shadowRenderer.sprite = _parentRenderer.sprite;
            _shadowRenderer.color = new Color(0, 0, 0, SHADOW_ALPHA);
            _shadowRenderer.sortingLayerID = _parentRenderer.sortingLayerID;
            _shadowRenderer.sortingOrder = _parentRenderer.sortingOrder - 3;

            UpdateShadowTransform();
        }

        void LateUpdate()
        {
            UpdateShadowTransform();
        }

        public void SetVisible(bool visible) => _shadowRenderer.enabled = visible;
        
        void UpdateShadowTransform()
        {
            _shadowObject.transform.position = transform.position + (Vector3)SHADOW_OFFSET * _ShadowOffsetMultiplier;
            _shadowObject.transform.rotation = transform.rotation;
            _shadowObject.transform.localScale = Vector3.one;
        }
    }
}
