using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endmove1 : MonoBehaviour
{
    public float speed = 5f;       // 移动速度
    public float jumpForce = 10f;      // 跳跃力度
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private int extrajump = 2;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity =new Vector2(-speed,rb.velocity.y);
            //transform.Translate(-speed * Time.deltaTime, 0, 0);
            float curspeed = -speed * Time.deltaTime;
            flip(curspeed);
            //anim.SetBool("jump",false);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity =new Vector2(speed,rb.velocity.y);
            //transform.Translate(speed * Time.deltaTime, 0, 0);
            float curspeed = speed * Time.deltaTime;
            flip(curspeed);
            //anim.SetBool("jump",false);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (extrajump > 0)
            {
                rb.velocity =new Vector2(rb.velocity.x,jumpForce);
                //rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                extrajump -= 1;
            }
            //anim.SetBool("jump",true);
        }
    }

    void flip(float dirx)
    {
        if (dirx > 0f)
        {
            sprite.flipX = false;
        }
        else if (dirx < 0f)
        {
            sprite.flipX = true;
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "ground")
        {
            extrajump = 2;
        }

    }
}