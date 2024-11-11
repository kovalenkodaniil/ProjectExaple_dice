using UnityEngine;

namespace _Core.Scripts.Core.Battle.Dice
{
    public class DiceEdgeSetter : MonoBehaviour
    {
        [SerializeField] public MeshRenderer _meshRenderer;

        public void SetEdgeMaterial(Material edgeMaterial)
        {
            return;
            _meshRenderer.material = edgeMaterial;
        }

        public void SetIcon(Sprite sprite)
        {
            _meshRenderer.material.SetTexture("_MainTex", sprite.texture);
        }

        public void SetColor(Color color)
        {
            SetColors(color, color);
        }

        public void SetColors(Color firstColor, Color secondColor)
        {
            _meshRenderer.material.SetColor("_FirstColor", firstColor);
            _meshRenderer.material.SetColor("_SecondColor", secondColor);
        }
    }
}