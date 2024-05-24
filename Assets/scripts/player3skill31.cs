using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player3skill31 : MonoBehaviour
{
    // �U���t��
    [SerializeField]
    private float fallSpeed = 10;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, 500f, transform.position.z);
        transform.rotation = Quaternion.Euler(0, 0, -90);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player2") || collider.gameObject.CompareTag("ground")||collider.gameObject.CompareTag("player21")||collider.gameObject.CompareTag("player23")  )
        {
            Destroy(gameObject); // Destroy the arrow
        }
    }
}
