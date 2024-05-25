using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lily3copy : MonoBehaviour
{
    public GameObject blueExPrefab; // Drag your blueEx Prefab here in the Inspector

    void OnTriggerEnter(Collider other)
    {
        // 检测碰撞，并且不是与自身碰撞
        if (other.gameObject.tag == "player2"||other.gameObject.tag == "player21"||other.gameObject.tag == "player23")
        {
            // 在 lily3 的位置生成 blueex
            StartCoroutine(SpawnAndExpandBlueEx());
        }
    }

    IEnumerator SpawnAndExpandBlueEx()
    {
        GameObject blueEx = Instantiate(blueExPrefab, transform.position, Quaternion.identity);
        blueEx.transform.localScale = Vector3.zero; // 从0开始

        float elapsedTime = 0f;
        float duration = 1f; // 持续1秒

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float scale = elapsedTime / duration; // 比例线性增长
            blueEx.transform.localScale = new Vector3(scale, scale, scale);

            yield return null;
        }

        Destroy(blueEx); // 1秒后摧毁
    }
}