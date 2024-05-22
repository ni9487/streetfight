using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player3skill2 : MonoBehaviour
{
    public float timer;
    public bool isright = true;
    private SpriteRenderer spr;

    void Start()
    {
        spr = this.gameObject.GetComponent<SpriteRenderer>();
        timer = 4f;

        if (!isright)
        {
            spr.flipX = true;
        }
    }

    void Update()
    {
        float moveSpeed = 15f * Time.deltaTime * 60;
        if (isright)
        {
            this.gameObject.transform.position += new Vector3(moveSpeed, 0, 0);
        }
        else
        {
            this.gameObject.transform.position += new Vector3(-moveSpeed, 0, 0);
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
