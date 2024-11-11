using UnityEngine;

namespace Core.Data
{
    [CreateAssetMenu(menuName = "Data/Assets/MaterialAsset")]
    public class MaterialAsset : ScriptableObject
    {
        [SerializeField] public Material outline;
        [SerializeField] public Material blink;
        [SerializeField] public Shader twoColors;
    }
}