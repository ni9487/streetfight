using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Photon.Pun.Demo.PunBasics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class playermove : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 10f;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private int extrajump;
    public float hp;
    public float maxhp;
    public GameObject player1hp;
    public GameObject playerBoom;
    public static float mp;
    public float maxmp;
    public GameObject player1mp;
    public Transform player1skill1;
    public Transform player1skill1down;
    public Transform player1skill1up;
    public Transform player1skill1mid;

    public Color damageColor = new Color32(200, 0, 0, 10); // 设置受伤时的颜色
    public float duration = 0.1f; // 变红的持续时间

    private Color originalColor;
    public playermove playermpnew;

    private int skillCount = 0; //1skill times
    private int skillCountbig = 0; //bigskill times
    public Image player1skill1shader;

    private float skill1cooldown;
    private float skill2cooldown;

    private bool isDefending = false; // 是否处于防御状态
    private bool canMove = true; // 是否可以移动
    private float defensecooldown;
    public Color defenseColor = new Color32(32, 0, 0, 10); // 设置防禦时的颜色

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        extrajump = 2;
        maxhp = 10000;
        hp = 0.97f * maxhp;
        maxmp = 25;
        mp = 0;
        originalColor = sprite.color;
    }

    IEnumerator FollowPlayer2D(Transform ball2D)
    {
        float existTime = 0.2f; // 圆球存在的最大时间
        Vector3 initialScale = ball2D.localScale; // 初始大小
        while (existTime > 0)
        {
            if (ball2D == null)
            {
                yield break; // 如果ball2D已经被销毁，则退出协程
            }
            // 根据距离调整球体的大小，距离越小，球体越大
            float scale = (0.27f - existTime) / 0.2f;
            ball2D.localScale = initialScale * scale; // 调整大小

            existTime -= Time.deltaTime;
            yield return null;
        }
        Destroy(ball2D.gameObject);
    }

    IEnumerator DelayedGenerateBall2Ddown()
    {
        yield return new WaitForSeconds(0.1f); // 等待0.5秒
        GenerateBall2Ddown();
    }

    IEnumerator DelayedGenerateBall2Dup()
    {
        yield return new WaitForSeconds(0.1f); // 等待0.5秒
        GenerateBall2Dup();
    }

    IEnumerator DelayedGenerateBall2D()
    {
        yield return new WaitForSeconds(0.1f); // 等待0.5秒
        GenerateBall2D();
    }

    //big skill
    IEnumerator DelayedGenerateBall2Ddownbig()
    {
        yield return new WaitForSeconds(0.1f); // 等待0.5秒
        GenerateBall2Ddownbig();
    }

    IEnumerator DelayedGenerateBall2Dupbig()
    {
        yield return new WaitForSeconds(0.1f); // 等待0.5秒
        GenerateBall2Dupbig();
    }

    IEnumerator DelayedGenerateBall2Dbig()
    {
        yield return new WaitForSeconds(0.1f); // 等待0.5秒
        GenerateBall2Dbig();
    }

    //press 8
    IEnumerator Delaymove()
    {
        yield return new WaitForSeconds(0.25f);
        canMove=true;
    }

    void GenerateBall2D()
    {
        if (skillCount < 4) // 检查技能生成次数是否小于七次
        {
            skillCount++; // 增加技能生成次数

            Transform ball2D = Instantiate(player1skill1mid, transform.position, Quaternion.identity);
            player1skill1mid fireball = ball2D.GetComponent<player1skill1mid>() as player1skill1mid;
            if (!sprite.flipX)
            {
                fireball.isright = false;
            }
            StartCoroutine(FollowPlayer2D(ball2D));
            StartCoroutine(DelayedGenerateBall2Ddown());
        }
    }

    void GenerateBall2Ddown()
    {
        if (skillCount < 4) // 检查技能生成次数是否小于七次
        {
            skillCount++; // 增加技能生成次数
            Transform ball2D = Instantiate(player1skill1down, transform.position, Quaternion.identity);
            player1skill1down fireball = ball2D.GetComponent<player1skill1down>() as player1skill1down;
            if (!sprite.flipX)
            {
                fireball.isright = false;
            }
            StartCoroutine(FollowPlayer2D(ball2D));
            StartCoroutine(DelayedGenerateBall2Dup());
        }
    }

    void GenerateBall2Dup()
    {
        if (skillCount < 4) // 检查技能生成次数是否小于七次
        {
            skillCount++; // 增加技能生成次数
            Transform ball2D = Instantiate(player1skill1up, transform.position, Quaternion.identity);
            player1skill1up fireball = ball2D.GetComponent<player1skill1up>() as player1skill1up;
            if (!sprite.flipX)
            {
                fireball.isright = false;
            }
            StartCoroutine(FollowPlayer2D(ball2D));
            StartCoroutine(DelayedGenerateBall2D());
        }
    }

    //big skill
    void GenerateBall2Dbig()
    {
        if (skillCountbig < 7) // 检查技能生成次数是否小于七次
        {
            skillCountbig++; // 增加技能生成次数

            Transform ball2D = Instantiate(player1skill1, transform.position, Quaternion.identity);
            player1skill1 fireball = ball2D.GetComponent<player1skill1>() as player1skill1;
            if (!sprite.flipX)
            {
                fireball.isright = false;
            }
            StartCoroutine(FollowPlayer2D(ball2D));
            StartCoroutine(DelayedGenerateBall2Ddownbig());
        }
    }

    void GenerateBall2Ddownbig()
    {
        if (skillCountbig < 7) // 检查技能生成次数是否小于七次
        {
            skillCountbig++; // 增加技能生成次数
            Transform ball2D = Instantiate(player1skill1down, transform.position, Quaternion.identity);
            player1skill1down fireball = ball2D.GetComponent<player1skill1down>() as player1skill1down;
            if (!sprite.flipX)
            {
                fireball.isright = false;
            }
            StartCoroutine(FollowPlayer2D(ball2D));
            StartCoroutine(DelayedGenerateBall2Dupbig());
        }
    }

    void GenerateBall2Dupbig()
    {
        if (skillCountbig < 7) // 检查技能生成次数是否小于七次
        {
            skillCountbig++; // 增加技能生成次数
            Transform ball2D = Instantiate(player1skill1up, transform.position, Quaternion.identity);
            player1skill1up fireball = ball2D.GetComponent<player1skill1up>() as player1skill1up;
            if (!sprite.flipX)
            {
                fireball.isright = false;
            }
            StartCoroutine(FollowPlayer2D(ball2D));
            StartCoroutine(DelayedGenerateBall2Dbig());
        }
    }

    // Update is called once per frame
    void Update()
    {
        float percent = (float)hp / (float)maxhp;
        player1hp.transform.localScale = new Vector3(percent, player1hp.transform.localScale.y, player1hp.transform.localScale.z);
        float percentmp = (float)mp / (float)maxmp;
        player1mp.transform.localScale = new Vector3(percentmp, player1mp.transform.localScale.y, player1mp.transform.localScale.z);
        if (canMove && !isDefending)
        {
            if (Input.GetKey(KeyCode.A))
            {
                rb.velocity =new Vector2(-speed,rb.velocity.y);
                //transform.Translate(-speed * Time.deltaTime, 0, 0);
                float curspeed = -speed * Time.deltaTime;
                flip(curspeed);
                //anim.SetBool("jump",false);
            }
            if (Input.GetKey(KeyCode.D))
            {
                rb.velocity =new Vector2(speed,rb.velocity.y);
                //transform.Translate(speed * Time.deltaTime, 0, 0);
                float curspeed = speed * Time.deltaTime;
                flip(curspeed);
                //anim.SetBool("jump",false);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (extrajump > 0)
                {
                    rb.velocity =new Vector2(rb.velocity.x,jumpForce);
                    //rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                    extrajump -= 1;
                }
                //anim.SetBool("jump",true);
            }

            if (Input.GetKeyDown(KeyCode.Keypad7) && skill1cooldown == 0)
            {
                skillCount = 0;
                GenerateBall2D();
                if (mp >= 20f && mp < 24f)
                {
                    mp = 24f;
                }
                if (mp < 20)
                {
                    mp += 4f;
                }
                skill1cooldown = 3;
            }

            if (Input.GetKeyDown(KeyCode.Keypad8) && skill2cooldown == 0)
            {
                canMove=false;
                if(sprite.flipX==false)
                {
                    rb.velocity=new Vector2(rb.velocity.x+jumpForce,rb.velocity.y);
                    //transform.Translate(speed*15*Time.deltaTime,0,0);
                    //rb.AddForce(Vector2.right * jumpForce*6, ForceMode2D.Impulse);
                }
                else
                {
                    rb.velocity=new Vector2(rb.velocity.x-jumpForce,rb.velocity.y);
                    //transform.Translate(-speed*15*Time.deltaTime,0,0);
                    //rb.AddForce(Vector2.left * jumpForce*6, ForceMode2D.Impulse);
                }
                if (mp >= 20f && mp < 24f)
                {
                    mp = 24f;
                }
                if (mp < 20)
                {
                    mp += 4f;
                }
                skill2cooldown = 4;
                StartCoroutine(Delaymove());
            }

            if (Input.GetKeyDown(KeyCode.Keypad9) && mp == 24)
            {
                skillCountbig = 0;
                GenerateBall2Dbig();
                mp = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.S) && (defensecooldown == 0))
        {
            // 角色进入防御状态
            isDefending = true;
            canMove = false; // 禁止移动
            sprite.color = defenseColor;
            StartCoroutine(ResetColorAfterDelay2());
            defensecooldown = 5;
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

        if (skill1cooldown > 0)
        {
            skill1cooldown -= Time.deltaTime;
            if (skill1cooldown < 0)
            {
                skill1cooldown = 0;
            }
        }
        if (skill2cooldown > 0)
        {
            skill2cooldown -= Time.deltaTime;
            if (skill2cooldown < 0)
            {
                skill2cooldown = 0;
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
        if (coll.gameObject.tag == "ground")
        {
            extrajump = 2;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "player2skill")
        {
            Destroy(other.gameObject);

            if (sprite.color == originalColor)
            {
                print(other.gameObject.name);
                if (hp >= 800)
                {
                    hp -= 800;
                }
                if (hp < 800)
                {
                    hp = 0;
                }
                if (hp == 0)
                {
                    player1hp.transform.localScale = new Vector3(0, player1hp.transform.localScale.y, player1hp.transform.localScale.z);
                    playerdie();
                }
                Destroy(other.gameObject);
                sprite.color = damageColor;
                StartCoroutine(ResetColorAfterDelay());
            }
        }
        if (other.gameObject.tag == "player2skill3")
        {
            Destroy(other.gameObject);

            if (sprite.color == originalColor)
            {
                if (hp >= 4000)
                {
                    hp -= 4000;
                }
                if (hp < 4000)
                {
                    hp = 0;

                }
                if (hp == 0)
                {
                    player1hp.transform.localScale = new Vector3(0, player1hp.transform.localScale.y, player1hp.transform.localScale.z);
                    playerdie();
                    Destroy(other.gameObject);
                    sprite.color = damageColor;
                    StartCoroutine(ResetColorAfterDelay());
                }
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

