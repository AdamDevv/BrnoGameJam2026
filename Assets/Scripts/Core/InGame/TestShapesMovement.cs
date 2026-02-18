using APX.Util.OdinAttributes;
using Shapes;
using UnityEngine;

namespace APGame.InGame
{
    public class TestShapesMovement : MonoBehaviour
    {
        [SerializeField]
        [RequiredChild]
        private Rectangle _Rectangle;
        
        private void Update()
        {
            _Rectangle.DashOffset += Time.deltaTime * 1.5f;
            transform.localScale = Vector3.one * (1 + Mathf.Sin(Time.time * 12) * 0.015f);
        }
    }
}
