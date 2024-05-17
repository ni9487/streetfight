using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun.Demo.PunBasics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class player2move : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 10f;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private int extrajump;
    public GameObject lily2;
    public float hp;
    public float maxhp;
    public GameObject player2hp;
    public static float mp;
    public float maxmp;
    public GameObject player2mp;
    public GameObject playerBoom;

    public Color damageColor = new Color32(200, 0, 0, 10); // 设置受伤时的颜色
    public float duration = 0.1f; // 变红的持续时间
    public Color defenseColor = new Color32(32, 0, 0, 10); // 设置防禦时的颜色
    private Color originalColor;
    public player2move player2mpnew;

    public Image player2skill2shader;

    private float skill2cooldown;
    private float skill1cooldown;
    private float defensecooldown;

    private bool isDefending = false; // 是否处于防御状态
    private bool canMove = true; // 是否可以移动

    //大招球
    public Transform lily3;
    public Transform player1;
    public Transform player12;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        extrajump = 2;
        maxmp = 25;
        mp = 0;
        maxhp = 10000;
        hp = 0.97f * maxhp;
        originalColor = sprite.color;
    }

    IEnumerator FollowPlayer2D(Transform ball2D)
    {
        float existTime = 5.0f; // 圆球存在的最大时间
        Vector3 initialScale = ball2D.localScale; // 初始大小
        while (existTime > 0)
        {
            if (ball2D == null)
            {
                yield break; // 如果ball2D已经被销毁，则退出协程
            }
            ball2D.position = Vector2.MoveTowards(ball2D.position, player1.position, Time.deltaTime * speed * 0.5f);

            //ball2D.position = Vector2.MoveTowards(ball2D.position, player12.position, Time.deltaTime * speed*0.5f);

            // 根据距离调整球体的大小，距离越小，球体越大
            float scale = (7.0f - existTime) / 5.0f;
            ball2D.localScale = initialScale * scale; // 调整大小

            existTime -= Time.deltaTime;
            yield return null;
        }
        Destroy(ball2D.gameObject);
    }

    void GenerateBall2D()
    {
        Transform ball2D = Instantiate(lily3, transform.position, Quaternion.identity);
        StartCoroutine(FollowPlayer2D(ball2D));
    }

    // Update is called once per frame
    void Update()
    {
        float percent = (float)mp / (float)maxmp;
        player2mp.transform.localScale = new Vector3(percent, player2mp.transform.localScale.y, player2mp.transform.localScale.z);
        float percenthp = (float)hp / (float)maxhp;
        player2hp.transform.localScale = new Vector3(percenthp, player2hp.transform.localScale.y, player2hp.transform.localScale.z);
        if (canMove && !isDefending)
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

            //招式2
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                if (skill2cooldown == 0)
                {
                    GameObject skill2 = Instantiate(lily2, this.transform.position, quaternion.identity);
                    lily2 fireball = skill2.GetComponent<lily2>() as lily2;
                    if (!sprite.flipX)
                    {
                        fireball.isright = false;
                    }
                    if (mp >= 20 && mp < 24)
                    {
                        mp = 24;
                    }
                    if (mp < 20)
                    {
                        mp += 4;
                    }
                    skill2cooldown = 3;
                }
            }

            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                if (skill1cooldown == 0)
                {
                    if (mp >= 20 && mp < 24)
                    {
                        mp = 24;
                    }
                    if (mp < 20)
                    {
                        mp += 4;
                    }
                    skill1cooldown = 5;
                }
            }

            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                if (mp >= 24f)
                {
                    // 生成圆球
                    GenerateBall2D();
                    mp = 0;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && (defensecooldown == 0))
        {
            // 角色进入防御状态
            isDefending = true;
            canMove = false; // 禁止移动
            sprite.color = defenseColor;
            StartCoroutine(ResetColorAfterDelay2());
            defensecooldown = 5;
        }

        if (skill2cooldown > 0)
        {
            skill2cooldown -= Time.deltaTime;
            if (skill2cooldown < 0)
            {
                skill2cooldown = 0;
            }
        }

        if (skill1cooldown > 0)
        {
            skill1cooldown -= Time.deltaTime;
            if (skill1cooldown < 0)
            {
                skill1cooldown = 0;
            }
        }

        if (defensecooldown > 0)
        {
            defensecooldown -= Time.deltaTime;
            if (defensecooldown < 0)
            {
                defensecooldown = 0;
            }
        }

        if (mp <= 0)
        {
            mp = 0;
        }
        if (mp >= 24)
        {
            mp = 24;
        }

        if (hp <= 0)
        {
            hp = 0;
        }
        if (hp < 9800)
        {
            hp += Time.deltaTime;
        }
    }

    IEnumerator ResetColorAfterDelay()//碰撞完等多久回色
    {
        // 等待一段时间
        yield return new WaitForSeconds(duration);
        // 恢复原始颜色
        sprite.color = originalColor;
    }

    IEnumerator ResetColorAfterDelay2()//碰撞完等多久回色
    {
        yield return new WaitForSeconds(1f); // 等待一秒

        // 恢复移动
        canMove = true;

        // 角色离开防御状态
        isDefending = false;

        // 恢复原始颜色
        sprite.color = originalColor;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "enemy")
        {
            //anim.SetBool("hurt",true);
        }
        if (coll.gameObject.tag == "ground")
        {
            extrajump = 2;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "player1skill1updown")
        {
            if (sprite.color == originalColor)
            {
                if (hp >= 400)
                {
                    hp -= 400f;
                }
                if (hp < 400)
                {
                    hp = 0;
                }
                if (hp == 0)
                {
                    player2hp.transform.localScale = new Vector3(0, player2hp.transform.localScale.y, player2hp.transform.localScale.z);
                    playerdie();
                }
                sprite.color = damageColor;
                StartCoroutine(ResetColorAfterDelay());
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
        if (other.gameObject.tag == "player1skill1")
        {
            if (sprite.color == originalColor)
            {
                if (hp >= 600)
                {
                    hp -= 600f;
                }
                if (hp < 600)
                {
                    hp = 0;
                }

                if (hp == 0)
                {
                    player2hp.transform.localScale = new Vector3(0, player2hp.transform.localScale.y, player2hp.transform.localScale.z);
                    playerdie();
                }
                
                sprite.color = damageColor;
                StartCoroutine(ResetColorAfterDelay());

            }
            else
            {
                Destroy(other.gameObject);
            }
        }
    }

    public void playerdie()
    {
        Instantiate(playerBoom, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
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
}



