using UnityEngine;
using System.Collections;  // 引入IEnumerator的命名空间

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private AudioSource audioSource;

    public AudioClip damageSound;
    public AudioClip swordSound;
    public AudioClip dash;
    public AudioClip charge;

    void Awake()
    {
        // 确保只有一个AudioManager实例
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 不要在加载新场景时销毁
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayDamageSound()
    {
        if (damageSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(damageSound);
        }
    }

    public void PlaydashSound()
    {
        if (dash != null && audioSource != null)
        {
            audioSource.PlayOneShot(dash);
        }
    }

    public void PlaychargeSound()
    {
        if (charge != null && audioSource != null)
        {
            audioSource.PlayOneShot(charge);
        }
    }

    public void PlaySwordSoundRepeatedly(int times, float interval)
    {
        StartCoroutine(PlaySwordSoundCoroutine(times, interval));
    }

    private IEnumerator PlaySwordSoundCoroutine(int times, float interval)
    {
        for (int i = 0; i < times; i++)
        {
            if (swordSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(swordSound,0.35f);
            }
            yield return new WaitForSeconds(interval);
        }
    }
}
