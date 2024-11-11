using UnityEngine;

namespace Utils
{
    public class MonoBehaviourDontDestroy : MonoBehaviour
    {
        public virtual void OnAwake()
        {
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }

        private void Awake() => OnAwake();
    }
}