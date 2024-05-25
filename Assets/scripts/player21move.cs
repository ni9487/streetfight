using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun.Demo.PunBasics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class player21move : MonoBehaviour
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
    private float p2s1cooldown;

    private bool isDefending = false; // 是否处于防御状态
    private bool canMove = true; // 是否可以移动
    private float defensecooldown;
    public Color defenseColor = new Color32(32, 0, 0, 10); // 设置防禦时的颜色

    public GameObject targetPrefab;  // Assign the Target prefab in inspector
    public Transform lilyPrefab;    // Assign the Lily1 prefab in inspector

    public bool dizzy;
    public GameObject dizzcirclePrefab;

    public bool isDashing = false; // 表示是否正在冲刺
    public Collider2D dashTrigger;

    public GameObject player2;

    public GameObject lowspeedPrefab;
    public GameObject player12;

    private bool isSpawning = false;
    public GameObject player3skill31;

    public GameObject blueExPrefab;

    private bool canFlip = true; 

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
        dashTrigger.enabled = false;
    }

    //sword qi
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

    IEnumerator FollowPlayer2Dlong(Transform ball2D)
    {
        float existTime = 0.4f; // 圆球存在的最大时间
        Vector3 initialScale = ball2D.localScale; // 初始大小
        while (existTime > 0)
        {
            if (ball2D == null)
            {
                yield break; // 如果ball2D已经被销毁，则退出协程
            }
            // 根据距离调整球体的大小，距离越小，球体越大
            float scale = (0.7f - existTime) / 0.3f;
            ball2D.localScale = initialScale * scale; // 调整大小

            existTime -= Time.deltaTime;
            yield return null;
        }
        Destroy(ball2D.gameObject);
    }

    //lily1
    IEnumerator grow(Transform ball2D)
    {
        float existTime = 0.3f; // 圆球存在的最大时间
        Vector3 initialScale = ball2D.localScale; // 初始大小
        Vector3 iniposition = ball2D.localPosition;
        float timer = 0f; 

        while (existTime > 0)
        {
            if (ball2D == null)
            {
                yield break; // 如果ball2D已经被销毁，则退出协程
            }
            // 根据距离调整球体的大小，距离越小，球体越大
            float scale = (0.3f - existTime) / 0.2f;
            ball2D.localScale = initialScale * scale; // 调整大小
            ball2D.localPosition = new Vector3(iniposition.x, iniposition.y + timer*180, iniposition.z);

            existTime -= Time.deltaTime;
            timer += Time.deltaTime;
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
        isDashing = false;
        dashTrigger.enabled=false;
    }

    //press 2
    IEnumerator SpawnTarget()
    {
        // Instantiate the target prefab at the player's position
        
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y-17 , transform.position.z);
        GameObject target = Instantiate(targetPrefab, targetPosition, Quaternion.identity);
        StartCoroutine(RotateObject(target, 0.4f));
        // Wait for 2 seconds
        yield return new WaitForSeconds(0.4f);
       
        // Destroy the target object
        Destroy(target);
        Vector3 lilyPosition = new Vector3(targetPosition.x, targetPosition.y-20 , targetPosition.z);
        // Instantiate the lily prefab at the target's (player's) last position
        Transform lily = Instantiate(lilyPrefab, lilyPosition, Quaternion.identity);

        StartCoroutine(grow(lily));

        // Wait for 1 second
        yield return new WaitForSeconds(1f);
    }

    IEnumerator RotateObject(GameObject target, float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            // Rotate the object each frame by 90 degrees around the Z axis
            target.transform.Rotate(new Vector3(0, 0, 90) * Time.deltaTime);

            // Increment the time by the time between frames
            time += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }
    }

    void GenerateBall2D()
    {
        if (skillCount < 10) // 检查技能生成次数是否小于七次
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
        if (skillCount < 10) // 检查技能生成次数是否小于七次
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
        if (skillCount < 10) // 检查技能生成次数是否小于七次
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
        if (skillCountbig < 17) // 检查技能生成次数是否小于七次
        {
            skillCountbig++; // 增加技能生成次数

            Transform ball2D = Instantiate(player1skill1, transform.position, Quaternion.identity);
            player1skill1 fireball = ball2D.GetComponent<player1skill1>() as player1skill1;
            if (!sprite.flipX)
            {
                fireball.isright = false;
            }
            StartCoroutine(FollowPlayer2Dlong(ball2D));
            StartCoroutine(DelayedGenerateBall2Ddownbig());
        }
    }

    void GenerateBall2Ddownbig()
    {
        if (skillCountbig < 17) // 检查技能生成次数是否小于七次
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
        if (skillCountbig < 17) // 检查技能生成次数是否小于七次
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

    IEnumerator DisableFlipXForDuration(float duration)
    {
        canFlip = false; // 禁止 flipX 改变
        yield return new WaitForSeconds(duration); // 等待指定时间
        canFlip = true; // 恢复 flipX 改变
    }

    // Update is called once per frame
    void Update()
    {
        float percent = (float)hp / (float)maxhp;
        player1hp.transform.localScale = new Vector3(percent, player1hp.transform.localScale.y, player1hp.transform.localScale.z);
        float percentmp = (float)mp / (float)maxmp;
        player1mp.transform.localScale = new Vector3(percentmp, player1mp.transform.localScale.y, player1mp.transform.localScale.z);
        if (canMove && !isDefending&&!dizzy)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rb.velocity =new Vector2(-speed,rb.velocity.y);
                //transform.Translate(-speed * Time.deltaTime, 0, 0);
                float curspeed = -speed * Time.deltaTime;
                if(canFlip)
                {
                    flip(curspeed);
                }
                //anim.SetBool("jump",false);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                rb.velocity =new Vector2(speed,rb.velocity.y);
                //transform.Translate(speed * Time.deltaTime, 0, 0);
                float curspeed = speed * Time.deltaTime;
                if(canFlip)
                {
                    flip(curspeed);
                }
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

            if (Input.GetKeyDown(KeyCode.Keypad1) && skill1cooldown == 0)
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
                skill1cooldown = 2;
                StartCoroutine(DisableFlipXForDuration(1.2f));
            }

            if (Input.GetKeyDown(KeyCode.Keypad2) && skill2cooldown == 0)
            {
                isDashing = true;
                dashTrigger.enabled=true;
                canMove=false;
                if(rb.velocity.x > 0)
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
                skill2cooldown = 3;
                StartCoroutine(Delaymove());
            }

            if (Input.GetKeyDown(KeyCode.Keypad3) && mp == 24)
            {
                skillCountbig = 0;
                StartCoroutine(DisableFlipXForDuration(1.7f));
                GenerateBall2Dbig();
                mp = 0;
            }
        }

        // //lily1
        // if (Input.GetKeyDown(KeyCode.Keypad2)&&p2s1cooldown==0)
        // {
        //     StartCoroutine(SpawnTarget());
        //     p2s1cooldown=2;
        // }


        if (Input.GetKeyDown(KeyCode.DownArrow) && (defensecooldown == 0))
        {
            // 角色进入防御状态
            isDefending = true;
            canMove = false; // 禁止移动
            sprite.color = defenseColor;
            StartCoroutine(ResetColorAfterDelay2());
            defensecooldown = 5;
        }

        if (Input.GetKeyDown(KeyCode.Y)&&p2s1cooldown==0&&player12.activeSelf)
        {
            StartCoroutine(SpawnTarget());
            p2s1cooldown=2;
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

        if (p2s1cooldown > 0)
        {
            p2s1cooldown -= Time.deltaTime;
            if (p2s1cooldown < 0)
            {
                p2s1cooldown = 0;
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

    IEnumerator dizzdelay()
    {
        // 等待一段时间
        yield return new WaitForSeconds(0.7f);
        dizzy=false;
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

    IEnumerator DizzcircleRoutine(GameObject dizzcircle)
    {
        float existTime = 1.0f;
        Vector3 offset = new Vector3(0, 50f, 0);
        while (existTime > 0)
        {
            if (dizzcircle == null)
            {
                yield break; // 如果 dizzcircle 已经被销毁，则退出协程
            }

            // 让 dizzcircle 跟随目标
            dizzcircle.transform.position = transform.position+offset;

            existTime -= Time.deltaTime;
            yield return null;
        }

        // 1 秒后销毁 dizzcircle
        Destroy(dizzcircle);
    }

    IEnumerator DestroyAfterDelay(GameObject objectToDestroy)
    {
        yield return new WaitForSeconds(0.1f); // 等待1秒
        Destroy(objectToDestroy); // 销毁对象
    }

    IEnumerator SlowDown()
    {
        speed *= 0.3f; // 速度降低90%
        yield return new WaitForSeconds(1.3f); // 维持减速一段时间
        speed /= 0.3f; // 恢复正常速度
    }

    IEnumerator lowspeedRoutine(GameObject low)
    {
        float existTime = 1.3f;
        Vector3 offset = new Vector3(0, 50f, 0);
        while (existTime > 0)
        {
            if (low == null)
            {
                yield break; // 如果 dizzcircle 已经被销毁，则退出协程
            }

            // 让 dizzcircle 跟随目标
            low.transform.position = transform.position+offset;

            existTime -= Time.deltaTime;
            yield return null;
        }

        // 1 秒后销毁 dizzcircle
        Destroy(low);
    }

    IEnumerator downarrowdelay()
    {
        while (isSpawning)
        {
            GameObject skill3 = Instantiate(player3skill31, this.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.3f);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "ground")
        {
            extrajump = 2;
        }
        if (coll.gameObject.tag == "player12skill1")
        {
            if (sprite.color == originalColor||sprite.color == damageColor)
            {
                print(coll.gameObject.name);
                if (hp >= 800)
                {
                    hp -= 800;
                }
                else if (hp < 800)
                {
                    hp = 0;
                }
                if (hp == 0)
                {
                    player1hp.transform.localScale = new Vector3(0, player1hp.transform.localScale.y, player1hp.transform.localScale.z);
                    playerdie();
                }
                sprite.color = damageColor;
                StartCoroutine(ResetColorAfterDelay());
            }
        }
        if (coll.gameObject.tag == "player3skill2")
        {
            if (sprite.color == originalColor||sprite.color == damageColor)
            {
                print(coll.gameObject.name);
                if (hp >= 800)
                {
                    hp -= 800;
                }
                else if (hp < 800)
                {
                    hp = 0;
                }
                if (hp == 0)
                {
                    player1hp.transform.localScale = new Vector3(0, player1hp.transform.localScale.y, player1hp.transform.localScale.z);
                    playerdie();
                }
                sprite.color = damageColor;
                StartCoroutine(ResetColorAfterDelay());
                StartCoroutine(DestroyAfterDelay(coll.gameObject));
            }
            
        }
    }

    IEnumerator SpawnAndExpandBlueEx()
    {
        GameObject blueEx = Instantiate(blueExPrefab, transform.position, Quaternion.identity);
        

        float elapsedTime = 0f;
        float duration = 0.15f; // 持续1秒

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float scale = 60*(elapsedTime+0.3f) / duration; // 比例线性增长
            blueEx.transform.localScale = new Vector3(scale, scale, scale);

            yield return null;
        }

        Destroy(blueEx); // 1秒后摧毁
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "player2skill")
        {
            if (sprite.color == originalColor||sprite.color == damageColor)
            {
                dizzy=true;
                Destroy(other.gameObject);

                Vector3 dizzposition = new Vector3(transform.position.x, transform.position.y+50 , transform.position.z);
                GameObject dizzcircle = Instantiate(dizzcirclePrefab, dizzposition, Quaternion.identity);
            
                // 开始一个协程，控制 dizzcircle 在 1 秒后消失，并让它跟随 player1
                StartCoroutine(DizzcircleRoutine(dizzcircle));
                print(other.gameObject.name);
                if (hp >= 800)
                {
                    hp -= 800;
                }
                else if (hp < 800)
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
                StartCoroutine(dizzdelay());
            }
        }

        if (other.gameObject.tag == "player3skill1")
        {
            if (sprite.color == originalColor||sprite.color == damageColor)
            {
            
                if (hp >= 150)
                {
                    hp -= 150;
                }
                else if (hp < 150)
                {
                    hp = 0;
                }
                if (hp == 0)
                {
                    player1hp.transform.localScale = new Vector3(0, player1hp.transform.localScale.y, player1hp.transform.localScale.z);
                    playerdie();
                }
                sprite.color = damageColor;
                StartCoroutine(ResetColorAfterDelay());
            }
        }

        if (other.gameObject.tag == "player3skill3")
        {
            isSpawning=true;
            StartCoroutine(downarrowdelay());
        }

        if (other.gameObject.tag == "player3skill31")
        {
            Destroy(other.gameObject);
            print(other.gameObject.name);
            if (hp >= 500)
            {
                hp -= 500;
            }
            else if (hp < 500)
            {
                hp = 0;
            }
            if (hp == 0)
            {
                player1hp.transform.localScale = new Vector3(0, player1hp.transform.localScale.y, player1hp.transform.localScale.z);
                playerdie();
            }
            sprite.color = damageColor;
            StartCoroutine(ResetColorAfterDelay());
        }

        
        if (other.gameObject.tag == "player2skill3")
        {
            Destroy(other.gameObject);
            print(other.gameObject.name);
            if (hp >= 3500)
            {
                hp -= 3500;
            }
            else if (hp < 3500)
            {
                hp = 0;
            }
            if (hp == 0)
            {
                player1hp.transform.localScale = new Vector3(0, player1hp.transform.localScale.y, player1hp.transform.localScale.z);
                playerdie();
            }
            sprite.color = damageColor;
            StartCoroutine(ResetColorAfterDelay());
            
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag=="Player1") // 确保是与 Player1 发生碰撞
        {
            if ((sprite.color == originalColor||sprite.color == damageColor)&&!isDefending)
            {
                Vector3 lowspeedposition = new Vector3(transform.position.x, transform.position.y+50 , transform.position.z);
                GameObject lowspeed = Instantiate(lowspeedPrefab, lowspeedposition, Quaternion.identity);
            
                // 开始一个协程，控制 dizzcircle 在 1 秒后消失，并让它跟随 player1
                StartCoroutine(lowspeedRoutine(lowspeed));

                StartCoroutine(SlowDown());
                if (hp >= 400)
                {
                    hp -= 400f;
                }
                else if (hp < 400)
                {
                    hp = 0;
                }
                if (hp == 0)
                {
                    player1hp.transform.localScale = new Vector3(0, player1hp.transform.localScale.y, player1hp.transform.localScale.z);
                    playerdie();
                }
                if(sprite.color!=defenseColor&&!isDefending)
                {
                    sprite.color = damageColor;
                }
                StartCoroutine(ResetColorAfterDelay());
            }
        }

        if (other.gameObject.tag == "player1skill1updown")
        {
            if ((sprite.color == originalColor||sprite.color == damageColor)&&!isDefending)
            {
                if (hp >= 400)
                {
                    hp -= 400f;
                }
                else if (hp < 400)
                {
                    hp = 0;
                }
                if (hp == 0)
                {
                    player1hp.transform.localScale = new Vector3(0, player1hp.transform.localScale.y, player1hp.transform.localScale.z);
                    playerdie();
                }
                if(sprite.color!=defenseColor&&!isDefending)
                {
                    sprite.color = damageColor;
                }
                StartCoroutine(ResetColorAfterDelay());
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
        if (other.gameObject.tag == "player1skill1")
        {
            if (hp >= 600)
            {
                hp -= 600f;
            }
            else if (hp < 600)
            {
                hp = 0;
            }

            if (hp == 0)
            {
                player1hp.transform.localScale = new Vector3(0, player1hp.transform.localScale.y, player1hp.transform.localScale.z);
                playerdie();
            }
            
            sprite.color = damageColor;
            StartCoroutine(ResetColorAfterDelay());
        }

        if (other.gameObject.tag == "player12skill")
        {
            if (sprite.color == originalColor||sprite.color == damageColor)
            {
                dizzy=true;
                Destroy(other.gameObject);

                Vector3 dizzposition = new Vector3(transform.position.x, transform.position.y+50 , transform.position.z);
                GameObject dizzcircle = Instantiate(dizzcirclePrefab, dizzposition, Quaternion.identity);
            
                // 开始一个协程，控制 dizzcircle 在 1 秒后消失，并让它跟随 player1
                StartCoroutine(DizzcircleRoutine(dizzcircle));
                print(other.gameObject.name);
                if (hp >= 600)
                {
                    hp -= 600;
                }
                else if (hp < 600)
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
                StartCoroutine(dizzdelay());
            }
        }
        
        if (other.gameObject.tag == "player12skill3")
        {
            StartCoroutine(SpawnAndExpandBlueEx());
            Destroy(other.gameObject);
            print(other.gameObject.name);
            if (hp >= 2500)
            {
                hp -= 2500;
            }
            else if (hp < 2500)
            {
                hp = 0;
            }
            if (hp == 0)
            {
                player1hp.transform.localScale = new Vector3(0, player1hp.transform.localScale.y, player1hp.transform.localScale.z);
                playerdie();
            }
            sprite.color = damageColor;
            StartCoroutine(ResetColorAfterDelay());
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "player3skill3")
        {
            // 停止生成箭矢
            isSpawning = false;
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

