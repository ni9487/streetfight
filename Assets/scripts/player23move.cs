using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun.Demo.PunBasics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class player23move : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 10f;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private int extrajump;
    public Transform player3skill1;
    public Transform player3skill1down;
    public Transform player3skill1up;
    public Transform player3skill1mid;
    public float hp;
    public float maxhp;
    public GameObject player3hp;
    public static float mp;
    public float maxmp;
    public GameObject player3mp;
    public GameObject playerBoom;
    public GameObject player3skill2;
    public GameObject player3skill3;
    public GameObject player3skill31;

    public Color damageColor = new Color32(200, 0, 0, 10); // 设置受伤时的颜色
    public float duration = 0.1f; // 变红的持续时间
    public Color defenseColor = new Color32(32, 0, 0, 10); // 设置防禦时的颜色
    private Color originalColor;
    public player3move player3mpnew;

    public Image player3skill2shader;

    private float skill2cooldown;
    public static float skill1cooldown = 0;
    private float defensecooldown;

    private bool isDefending = false; // 是否处于防御状态
    private bool canMove = true; // 是否可以移动

    private int skillCount = 0;
    public bool dizzy;
    public GameObject dizzcirclePrefab;

    public GameObject lowspeedPrefab;
    private bool isSpawning = false;

    // Start is called before the first frame update
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
        float existTime = 0.7f; // 圆球存在的最大时间
        Vector3 initialScale = ball2D.localScale; // 初始大小
        while (existTime > 0)
        {
            if (ball2D == null)
            {
                yield break; // 如果ball2D已经被销毁，则退出协程
            }
            // 根据距离调整球体的大小，距离越小，球体越大
            float scale = (1.2f - existTime) / 0.8f;
            ball2D.localScale = initialScale * scale; // 调整大小

            existTime -= Time.deltaTime;
            yield return null;
        }
        if (ball2D != null)
        {
            Destroy(ball2D.gameObject);
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
            ball2D.localPosition = new Vector3(iniposition.x, iniposition.y + timer * 180, iniposition.z);

            existTime -= Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
        if (ball2D != null)
        {
            Destroy(ball2D.gameObject);
        }
    }


    IEnumerator dizzdelay()
    {
        // 等待一段时间
        yield return new WaitForSeconds(0.7f);
        dizzy = false;
    }

    IEnumerator DelayedGenerateBall2Ddown()
    {
        yield return new WaitForSeconds(0.05f); // 等待0.5秒
        GenerateBall2Ddown();
    }
    void GenerateBall2D()
    {
        if (skillCount < 7) // 检查技能生成次数是否小于七次
        {
            skillCount++; // 增加技能生成次数

            Transform ball2D = Instantiate(player3skill1mid, transform.position, Quaternion.identity);
            if (ball2D != null)
            {
                player3skill1mid fireball = ball2D.GetComponent<player3skill1mid>() as player3skill1mid;
                if (!sprite.flipX)
                {
                    fireball.isright = false;
                }
                StartCoroutine(FollowPlayer2D(ball2D));
                StartCoroutine(DelayedGenerateBall2Ddown());
            }
        }
    }
    void GenerateBall2Ddown()
    {
        if (skillCount < 7) // 检查技能生成次数是否小于七次
        {
            skillCount++; // 增加技能生成次数
            Transform ball2D = Instantiate(player3skill1down, transform.position, Quaternion.identity);
            if (ball2D != null)
            {
                player3skill1down fireball = ball2D.GetComponent<player3skill1down>() as player3skill1down;
                if (!sprite.flipX)
                {
                    fireball.isright = false;
                }
                StartCoroutine(FollowPlayer2D(ball2D));
                StartCoroutine(DelayedGenerateBall2Dup());
            }
        }
    }
    IEnumerator DelayedGenerateBall2Dup()
    {
        yield return new WaitForSeconds(0.05f); // 等待0.5秒
        GenerateBall2Dup();
    }
    void GenerateBall2Dup()
    {
        if (skillCount < 7) // 检查技能生成次数是否小于七次
        {
            skillCount++; // 增加技能生成次数
            Transform ball2D = Instantiate(player3skill1up, transform.position, Quaternion.identity);
            if (ball2D != null)
            {
                player3skill1up fireball = ball2D.GetComponent<player3skill1up>() as player3skill1up;
                if (!sprite.flipX)
                {
                    fireball.isright = false;
                }
                StartCoroutine(FollowPlayer2D(ball2D));
                StartCoroutine(DelayedGenerateBall2D());
            }
        }
    }
    IEnumerator DelayedGenerateBall2D()
    {
        yield return new WaitForSeconds(0.05f); // 等待0.5秒
        GenerateBall2D();
    }

    // Update is called once per frame
    void Update()
    {
        float percent = (float)mp / (float)maxmp;
        player3mp.transform.localScale = new Vector3(percent, player3mp.transform.localScale.y, player3mp.transform.localScale.z);
        float percenthp = (float)hp / (float)maxhp;
        player3hp.transform.localScale = new Vector3(percenthp, player3hp.transform.localScale.y, player3hp.transform.localScale.z);
        if (canMove && !isDefending && !dizzy)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
                //transform.Translate(-speed * Time.deltaTime, 0, 0);
                float curspeed = -speed * Time.deltaTime;
                flip(curspeed);
                //anim.SetBool("jump",false);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
                //transform.Translate(speed * Time.deltaTime, 0, 0);
                float curspeed = speed * Time.deltaTime;
                flip(curspeed);
                //anim.SetBool("jump",false);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (extrajump > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    //rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                    extrajump -= 1;
                }
                //anim.SetBool("jump",true);
            }
            if (Input.GetKeyDown(KeyCode.Keypad4) && skill1cooldown == 0)
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
                skill1cooldown = 0.5f;
            }
            if (Input.GetKeyDown(KeyCode.Keypad5) && skill2cooldown == 0)
            {
                GameObject skill2 = Instantiate(player3skill2, this.transform.position, Quaternion.identity);
                player3skill2 fireball = skill2.GetComponent<player3skill2>();
                fireball.isright = !sprite.flipX; // Set direction based on the player's facing direction
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
            if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y - 30, transform.position.z);
                GameObject skill3 = Instantiate(player3skill3, targetPosition, Quaternion.identity);
                player3skill3 fireball = skill3.GetComponent<player3skill3>();
                fireball.isright = sprite.flipX;
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
            low.transform.position = transform.position + offset;

            existTime -= Time.deltaTime;
            yield return null;
        }

        // 1 秒后销毁 dizzcircle
        Destroy(low);
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
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "player2skill1")
        {
            if (sprite.color == originalColor || sprite.color == damageColor)
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
                    player3hp.transform.localScale = new Vector3(0, player3hp.transform.localScale.y, player3hp.transform.localScale.z);
                    playerdie();
                }
                sprite.color = damageColor;
                StartCoroutine(ResetColorAfterDelay());
            }
        }
        if (coll.gameObject.tag == "ground")
        {
            extrajump = 2;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player1") // 确保是与 Player1 发生碰撞
        {
            if ((sprite.color == originalColor || sprite.color == damageColor) && !isDefending)
            {
                Vector3 lowspeedposition = new Vector3(transform.position.x, transform.position.y + 50, transform.position.z);
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
                    player3hp.transform.localScale = new Vector3(0, player3hp.transform.localScale.y, player3hp.transform.localScale.z);
                    playerdie();
                }
                if (sprite.color != defenseColor && !isDefending)
                {
                    sprite.color = damageColor;
                }
                StartCoroutine(ResetColorAfterDelay());
            }
        }

        if (other.gameObject.tag == "player21skill1updown")
        {
            if ((sprite.color == originalColor || sprite.color == damageColor) && !isDefending)
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
                    player3hp.transform.localScale = new Vector3(0, player3hp.transform.localScale.y, player3hp.transform.localScale.z);
                    playerdie();
                }
                if (sprite.color != defenseColor && !isDefending)
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
        if (other.gameObject.tag == "player21skill1")
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
                player3hp.transform.localScale = new Vector3(0, player3hp.transform.localScale.y, player3hp.transform.localScale.z);
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
                player3hp.transform.localScale = new Vector3(0, player3hp.transform.localScale.y, player3hp.transform.localScale.z);
                playerdie();
            }
            sprite.color = damageColor;
            StartCoroutine(ResetColorAfterDelay());

            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "player2skill")
        {
            if (sprite.color == originalColor || sprite.color == damageColor)
            {
                dizzy = true;
                Destroy(other.gameObject);

                Vector3 dizzposition = new Vector3(transform.position.x, transform.position.y + 50, transform.position.z);
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
                    player3hp.transform.localScale = new Vector3(0, player3hp.transform.localScale.y, player3hp.transform.localScale.z);
                    playerdie();
                }
                Destroy(other.gameObject);
                sprite.color = damageColor;
                StartCoroutine(ResetColorAfterDelay());
                StartCoroutine(dizzdelay());
            }
        }
        if (other.gameObject.tag == "player3skill3")
        {
            isSpawning = true;
            StartCoroutine(downarrowdelay());
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
