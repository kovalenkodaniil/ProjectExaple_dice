using UnityEngine;

namespace Managers
{
    public class CanvasGlobalCameraSetter : MonoBehaviour
    {
        void Awake()
        {
            var canvas = GetComponent<Canvas>();
            canvas.worldCamera = GlobalCamera.Camera;
            canvas.planeDistance = 20;
        }
    }
}