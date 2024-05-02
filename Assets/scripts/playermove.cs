using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermove : MonoBehaviour
{
    [SerializeField] float speed=5f;
    [SerializeField] float jumpForce = 10f;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private int extrajump;
    public float hp;
    public float maxhp;
    public GameObject player1hp;
    public GameObject playerBoom;

    public Color damageColor = new Color32(200,0,0,10); // 设置受伤时的颜色
    public float duration = 0.1f; // 变红的持续时间
    private Color originalColor;
    public player2move player2mpnew;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim= GetComponent<Animator>();
        sprite= GetComponent<SpriteRenderer>();
        extrajump=2;
        maxhp=10;
        hp=0.97f*maxhp;
        originalColor = sprite.color;
    }

    // Update is called once per frame
    void Update()
    {
        float percent=(float)hp/(float)maxhp;
        player1hp.transform.localScale=new Vector3(percent,player1hp.transform.localScale.y,player1hp.transform.localScale.z);
        if(Input.GetKey(KeyCode.A))
        {
            transform.Translate(-speed*Time.deltaTime,0,0);
            float curspeed=-speed*Time.deltaTime;
            flip(curspeed);
            //anim.SetBool("jump",false);
        }
        if(Input.GetKey(KeyCode.D))
        {
            transform.Translate(speed*Time.deltaTime,0,0);
            float curspeed=speed*Time.deltaTime;
            flip(curspeed);
            //anim.SetBool("jump",false);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(extrajump>0)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                extrajump-=1;
            }
            //anim.SetBool("jump",true);
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            rb.AddForce(-Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        if(Input.GetMouseButtonDown(0))
        {
            if(sprite.flipX==false)
            {
                transform.Translate(speed*150*Time.deltaTime,0,0);
            }
            else
            {
                transform.Translate(-speed*150*Time.deltaTime,0,0);
            }
        }
        if(hp<=0)
        {
            hp=0;
        }
        if(hp<9)
        {
            hp+=Time.deltaTime*0.01f;
        }
    }

    IEnumerator ResetColorAfterDelay()//碰撞完等多久回色
    {
        // 等待一段时间
        yield return new WaitForSeconds(duration);
        // 恢复原始颜色
        sprite.color = originalColor;
    }

    void OnCollisionEnter2D(Collision2D coll) 
    {
        if(coll.gameObject.tag=="ground")
        {
            extrajump=2;
        }     
    }

    void OnTriggerEnter2D(Collider2D other) 
    { 
        if(other.gameObject.tag=="player2skill")
        {
            print(other.gameObject.name);
            if(hp>=0.6)
            {
                hp-=1;
            }
            if(hp<0.6)
            {
                hp=0;
            }
            if(hp==0)
            {
                Destroy(this.gameObject);
            }
            Destroy(other.gameObject);
            sprite.color=damageColor;
            StartCoroutine(ResetColorAfterDelay());
        }
    }



    void flip(float dirx)
    {
        if(dirx>0f)
        {
            sprite.flipX=false;
        }
        else if(dirx<0f)
        {
            sprite.flipX=true;
        }
    }
}
