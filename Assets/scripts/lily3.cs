using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lily3 : MonoBehaviour
{
    public camerashake cameraShaker;
    // Start is called before the first frame update
    void Start()
    {
        cameraShaker = Camera.main.GetComponent<camerashake>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        // 检查碰撞的对象是否为Player1，这里简化假设Player1有一个标签为"Player1"。
        if (other.CompareTag("Player1")) 
        {
            //StartCoroutine(cameraShaker.Shake(0.5f, 0.4f));
            Destroy(gameObject); // 销毁球体
        }
        
    }
}
