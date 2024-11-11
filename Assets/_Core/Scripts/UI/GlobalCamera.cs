using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Managers
{
    [DefaultExecutionOrder(-1)]
    public class GlobalCamera : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Volume volume;

        public static Camera Camera { get; private set; }
        private static GlobalCamera instance;
        public static GlobalCamera Instance => instance;

        private UniversalAdditionalCameraData cameraData;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
            
            _camera.opaqueSortMode = OpaqueSortMode.FrontToBack;
            _camera.transparencySortMode = TransparencySortMode.Orthographic;
            Camera = _camera;

            cameraData = _camera.GetComponent<UniversalAdditionalCameraData>();
        }

        public void SetActiveBloom(bool value)
        {
            volume.enabled = value;
            cameraData.renderPostProcessing = value;
        }
    }
}