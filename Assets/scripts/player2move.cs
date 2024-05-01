using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class player2move : MonoBehaviour
{
    [SerializeField] float speed=5f;
    [SerializeField] float jumpForce = 10f;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private int extrajump;
    public GameObject lily2;
    float mp;
    public float maxmp;
    public GameObject player2mp;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim= GetComponent<Animator>();
        sprite= GetComponent<SpriteRenderer>();
        extrajump=2;
        maxmp=10;
        mp=0.97f*maxmp;
    }

    // Update is called once per frame
    void Update()
    {
        float percent=(float)mp/(float)maxmp;
        player2mp.transform.localScale=new Vector3(percent,player2mp.transform.localScale.y,player2mp.transform.localScale.z);
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-speed*Time.deltaTime,0,0);
            float curspeed=-speed*Time.deltaTime;
            flip(curspeed);
            //anim.SetBool("jump",false);
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(speed*Time.deltaTime,0,0);
            float curspeed=speed*Time.deltaTime;
            flip(curspeed);
            //anim.SetBool("jump",false);
        }
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(extrajump>0)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                extrajump-=1;
            }
            //anim.SetBool("jump",true);
        }
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            rb.AddForce(-Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        //招式2
        if(Input.GetKeyDown(KeyCode.Keypad1))
        {
            if(mp>=1)
            {
                GameObject skill2 = Instantiate(lily2,this.transform.position,quaternion.identity);
                lily2 fireball = skill2.GetComponent<lily2>() as lily2;
                if(!sprite.flipX)
                {
                    fireball.isright=false;
                }
                mp-=1;
            }
        }
        if(mp<9.7)
        {
            mp+=Time.deltaTime*0.2f;
        }

    }

    void OnCollisionEnter2D(Collision2D coll) 
    { 
        if(coll.gameObject.tag=="enemy")
        {
            //anim.SetBool("hurt",true);
        } 
        if(coll.gameObject.tag=="ground")
        {
            extrajump=2;
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

