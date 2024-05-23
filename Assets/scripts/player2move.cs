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
    public bool dizzy;
    public GameObject dizzcirclePrefab;
    public GameObject player3skill31;

    public Color damageColor = new Color32(200, 0, 0, 10); // 设置受伤时的颜色
    public float duration = 0.1f; // 变红的持续时间
    public Color defenseColor = new Color32(32, 0, 0, 10); // 设置防禦时的颜色
    private Color originalColor;
    public player2move player2mpnew;

    public Image player2skill2shader;

    private float skill2cooldown;
    public static float skill1cooldown=0;
    private float defensecooldown;

    private bool isDefending = false; // 是否处于防御状态
    private bool canMove = true; // 是否可以移动

    //大招球
    public Transform lily3;
    public GameObject player1;
    public GameObject player12;
    public GameObject player3;

    public GameObject lowspeedPrefab;

    private bool isSpawning = false;

    private float p2s1cooldown;
    public GameObject targetPrefab; 
    public Transform lilyPrefab;

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
        float existTime = 4.0f; // 圆球存在的最大时间
        Vector3 initialScale = ball2D.localScale; // 初始大小
        while (existTime > 0)
        {
            if (ball2D == null)
            {
                yield break; // 如果ball2D已经被销毁，则退出协程
            }
            if(player1.activeSelf)
            {
                ball2D.position = Vector2.MoveTowards(ball2D.position, player1.transform.position, Time.deltaTime * speed * 0.5f);
            }
            if(player12.activeSelf)
            {
                ball2D.position = Vector2.MoveTowards(ball2D.position, player12.transform.position, Time.deltaTime * speed * 0.5f);
            }
            if(player3.activeSelf)
            {
                ball2D.position = Vector2.MoveTowards(ball2D.position, player3.transform.position, Time.deltaTime * speed * 0.5f);
            }
            

            //ball2D.position = Vector2.MoveTowards(ball2D.position, player12.position, Time.deltaTime * speed*0.5f);

            // 根据距离调整球体的大小，距离越小，球体越大
            float scale = (7.0f - existTime) / 5.0f;
            ball2D.localScale = initialScale * scale; // 调整大小

            existTime -= Time.deltaTime;
            yield return null;
        }
        Destroy(ball2D.gameObject);
    }

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
        if (canMove && !isDefending && !dizzy)
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
                    skill1cooldown = 2;
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

        if (Input.GetKeyDown(KeyCode.Y)&&p2s1cooldown==0&&player12.activeSelf)
        {
            StartCoroutine(SpawnTarget());
            p2s1cooldown=2;
        }

        if (p2s1cooldown > 0)
        {
            p2s1cooldown -= Time.deltaTime;
            if (p2s1cooldown < 0)
            {
                p2s1cooldown = 0;
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

    IEnumerator DestroyAfterDelay(GameObject objectToDestroy)
    {
        yield return new WaitForSeconds(0.1f); // 等待1秒
        Destroy(objectToDestroy); // 销毁对象
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
                    player2hp.transform.localScale = new Vector3(0, player2hp.transform.localScale.y, player2hp.transform.localScale.z);
                    playerdie();
                }
                sprite.color = damageColor;
                StartCoroutine(ResetColorAfterDelay());
                StartCoroutine(DestroyAfterDelay(coll.gameObject));
            }
            
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
                    player2hp.transform.localScale = new Vector3(0, player2hp.transform.localScale.y, player2hp.transform.localScale.z);
                    playerdie();
                }
                sprite.color = damageColor;
                StartCoroutine(ResetColorAfterDelay());
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
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
                    player2hp.transform.localScale = new Vector3(0, player2hp.transform.localScale.y, player2hp.transform.localScale.z);
                    playerdie();
                }
                if(sprite.color!=defenseColor&&!isDefending)
                {
                    sprite.color = damageColor;
                }
                StartCoroutine(ResetColorAfterDelay());
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
                    player2hp.transform.localScale = new Vector3(0, player2hp.transform.localScale.y, player2hp.transform.localScale.z);
                    playerdie();
                }
                sprite.color = damageColor;
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
                    player2hp.transform.localScale = new Vector3(0, player2hp.transform.localScale.y, player2hp.transform.localScale.z);
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
                player2hp.transform.localScale = new Vector3(0, player2hp.transform.localScale.y, player2hp.transform.localScale.z);
                playerdie();
            }
            
            sprite.color = damageColor;
            StartCoroutine(ResetColorAfterDelay());
        }
        if (other.gameObject.tag == "player3skill2")
        {
            if (sprite.color == originalColor || sprite.color == damageColor)
            {
                // 获取 player3 的位置
                Transform player3Transform = GameObject.FindGameObjectWithTag("Player3").transform;

                // 计算 player2 和 player3 之间的距离
                float distanceToPlayer3 = Vector2.Distance(transform.position, player3Transform.position);

                // 如果距离小于等于 3 公分
                if (distanceToPlayer3 <= 0.03f) // Unity 使用米作为单位，所以 3 公分是 0.03 米
                {
                    // 计算后退方向
                    Vector2 knockbackDirection = (transform.position - player3Transform.position).normalized;

                    // 计算后退距离为 6 公分（0.06 米）
                    Vector2 knockbackDistance = knockbackDirection * 0.06f;

                    // 更新 player2 的位置
                    transform.position += new Vector3(knockbackDistance.x, knockbackDistance.y, 0);
                }
                Destroy(other.gameObject);
                sprite.color = damageColor;
                StartCoroutine(ResetColorAfterDelay());
            }
        }
        if (other.gameObject.tag == "player3skill3")
        {
            isSpawning=true;
            StartCoroutine(downarrowdelay());
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
                    player2hp.transform.localScale = new Vector3(0, player2hp.transform.localScale.y, player2hp.transform.localScale.z);
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
                player2hp.transform.localScale = new Vector3(0, player2hp.transform.localScale.y, player2hp.transform.localScale.z);
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

    IEnumerator downarrowdelay()
    {
        while (isSpawning)
        {
            GameObject skill3 = Instantiate(player3skill31, this.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator dizzdelay()
    {
        // 等待一段时间
        yield return new WaitForSeconds(1f);
        dizzy = false;
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
            dizzcircle.transform.position = transform.position + offset;

            existTime -= Time.deltaTime;
            yield return null;
        }

        // 1 秒后销毁 dizzcircle
        Destroy(dizzcircle);
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



