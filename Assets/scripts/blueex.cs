// BlueExBehavior.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueExBehavior : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GrowAndDestroy());
    }

    IEnumerator GrowAndDestroy()
    {
        float elapsedTime = 0f;
        float duration = 1f; // 持续1秒

        Vector3 initialScale = Vector3.zero; 
        Vector3 finalScale = new Vector3(1f, 1f, 1f); // 最终大小

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(initialScale, finalScale, elapsedTime / duration);
            yield return null;
        }

        Destroy(gameObject); // 1秒后摧毁
    }
}
