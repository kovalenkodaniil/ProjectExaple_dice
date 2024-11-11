using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GlitchTest : MonoBehaviour
{
    [SerializeField] private Image img;

    private void Start()
    {
        StartCoroutine(Routine());
    }

    private IEnumerator Routine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.01f, .9f));

            var value = Random.Range(0.1f, 0.3f);
            img.material.SetFloat("_Value", value);

            yield return new WaitForSeconds(Random.Range(0.05f, 0.5f));
            
            float timeBack = 0.05F;
            while ((timeBack -= Time.deltaTime) > 0)
            {
                img.material.SetFloat("_Value", Mathf.Lerp(value, 0, (value - timeBack) / value));   
                yield return null;
            }
        }
    }
}
