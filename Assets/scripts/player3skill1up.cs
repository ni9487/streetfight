using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player3skill1up : MonoBehaviour
{
    public float timer;
    public bool isright = true;
    private SpriteRenderer spr;
    // Start is called before the first frame update
    void Start()
    {
        spr = this.gameObject.GetComponent<SpriteRenderer>();
        timer = 1f;

        if (isright != true)
        {
            spr.flipX = false;
        }
        if (isright != false)
        {
            spr.flipX = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isright)
        {
            this.gameObject.transform.position += new Vector3(-10f * Time.deltaTime * 80, 2f * Time.deltaTime * 80, 0);
        }
        else
        {
            this.gameObject.transform.position += new Vector3(10f * Time.deltaTime * 80, 2f * Time.deltaTime * 80, 0);
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
