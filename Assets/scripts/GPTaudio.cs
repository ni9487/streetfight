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
    public AudioClip wavesword;

    public AudioClip lily3boom;
    public AudioClip lily3release;
    public AudioClip lily2release;
    public AudioClip ice;

    public AudioClip shootarrow1;
    public AudioClip shootarrow2;
    public AudioClip magiccircle;
    public AudioClip yornkeepshoot;

    void Awake()
    {
        // 确保只有一个AudioManager实例
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject); // 不要在加载新场景时销毁
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

    public void Playlily3boom()
    {
        if (lily3boom != null && audioSource != null)
        {
            audioSource.PlayOneShot(lily3boom);
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

    public void Playlily3release()
    {
        if (lily3release != null && audioSource != null)
        {
            audioSource.PlayOneShot(lily3release);
        }
    }

    public void Playshootarrow1()
    {
        if (shootarrow1 != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootarrow1);
        }
    }

    public void Playmagiccircle()
    {
        if (magiccircle != null && audioSource != null)
        {
            audioSource.PlayOneShot(magiccircle);
        }
    }

    public void Playlily2release()
    {
        if (lily2release != null && audioSource != null)
        {
            audioSource.PlayOneShot(lily2release);
        }
    }

    public void Playice()
    {
        if (ice != null && audioSource != null)
        {
            audioSource.PlayOneShot(ice);
        }
    }

    public void Playyornkeepshoot()
    {
        if (yornkeepshoot != null && audioSource != null)
        {
            audioSource.PlayOneShot(yornkeepshoot);
        }
    }

    public void Playshootarrow2()
    {
        if (shootarrow2 != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootarrow2);
        }
    }

    public void Playwavesword()
    {
        if (wavesword != null && audioSource != null)
        {
            audioSource.PlayOneShot(wavesword);
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
