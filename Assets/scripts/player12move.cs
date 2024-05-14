using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Photon.Pun.Demo.PunBasics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class player12move : MonoBehaviour
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
    public float mp;
    public float maxmp;
    public GameObject player1mp;

    public Color damageColor = new Color32(200,0,0,10); // 设置受伤时的颜色
    public float duration = 0.1f; // 变红的持续时间
    private Color originalColor;
    public player12move player12mpnew;

    public GameObject lily2;
    public Image player2skill2shader;
    private float skill2cooldown;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim= GetComponent<Animator>();
        sprite= GetComponent<SpriteRenderer>();
        extrajump=2;
        maxhp=10;
        maxmp=10;
        hp=0.97f*maxhp;
        originalColor = sprite.color;
        mp=0;
    }

    // Update is called once per frame
    void Update()
    {
        float percent=(float)mp/(float)maxmp;
        player1mp.transform.localScale=new Vector3(percent,player1mp.transform.localScale.y,player1mp.transform.localScale.z);
        float percenthp=(float)hp/(float)maxhp;
        player1hp.transform.localScale=new Vector3(percenthp,player1hp.transform.localScale.y,player1hp.transform.localScale.z);
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

        //招式2
        if(Input.GetKeyDown(KeyCode.Keypad7))
        {
            if(skill2cooldown==0)
            {
                GameObject skill2 = Instantiate(lily2,this.transform.position,quaternion.identity);
                lily2 fireball = skill2.GetComponent<lily2>() as lily2;
                if(!sprite.flipX)
                {
                    fireball.isright=false;
                }
                if(mp>=9.4f&&mp<9.7f)
                {
                    mp=9.7f;
                }
                if(mp<9.4)
                { 
                    mp+=0.5f;
                }
                skill2cooldown=3;
            }
        }
        if(skill2cooldown>0)
        {
            skill2cooldown-=Time.deltaTime;
            if(skill2cooldown<0)
            {
                skill2cooldown=0;
            }
        }

        mp-=Time.deltaTime*0.02f;

        if(mp<=0)
        {
            mp=0;
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
                player1hp.transform.localScale=new Vector3(0,player1hp.transform.localScale.y,player1hp.transform.localScale.z);
                playerdie();
            }
            Destroy(other.gameObject);
            sprite.color=damageColor;
            StartCoroutine(ResetColorAfterDelay());
        }
    }

    public void playerdie()
    {
        Instantiate(playerBoom, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
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

