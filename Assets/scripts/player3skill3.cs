using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player3skill3 : MonoBehaviour
{
    public float timer;
    public bool isright = true;
    private SpriteRenderer spr;

    public GameObject goldlightPrefab;
    private GameObject goldlightInstance;
    public Vector3 goldlightScale = new Vector3(0.5f, 0.5f, 0.5f); // goldlight 的缩放比例

    // Start is called before the first frame update
    void Start()
    {
        spr = this.gameObject.GetComponent<SpriteRenderer>();
        timer = 7f;
        goldlightInstance = Instantiate(goldlightPrefab, transform.position, Quaternion.identity, this.transform);
        //goldlightInstance.transform.localScale = goldlightScale; // 设置goldlight的缩放比例
        if (isright != true)
        {
            spr.flipX = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isright)
        {
            this.gameObject.transform.position += new Vector3(-2f * Time.deltaTime * 60, 0, 0);
        }
        else
        {
            this.gameObject.transform.position += new Vector3(2f * Time.deltaTime * 60, 0, 0);
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
