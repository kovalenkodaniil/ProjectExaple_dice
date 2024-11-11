using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Managers
{
    public static class CoroutineManager
    {
        class CoroutineHandler : MonoBehaviour { }
    
        private static CoroutineHandler instance;
    
        static CoroutineManager()
        {
            instance = new GameObject("CoroutineManager").AddComponent<CoroutineHandler>();
            Object.DontDestroyOnLoad(instance.gameObject);
        }

        public static Coroutine Wait(float seconds, Action after) => instance.StartCoroutine(WaitRoutine(seconds, after));
    
        public static Coroutine WaitUntil(Func<bool> predicate, Action after) => instance.StartCoroutine(WaitUntilRoutine(predicate, after));

        public static Coroutine StartCoroutine(IEnumerator routine) => instance.StartCoroutine(routine);

        public static void StopCoroutine(Coroutine coroutine)
        {
            if(coroutine != null)
                instance.StopCoroutine(coroutine);
        }

        private static IEnumerator WaitRoutine(float seconds, Action after)
        {
            yield return new WaitForSeconds(seconds);
            after?.Invoke();
        }
    
        private static IEnumerator WaitUntilRoutine(Func<bool> predicate, Action after)
        {
            yield return new WaitUntil(predicate);
            after?.Invoke();
        }
    }
}