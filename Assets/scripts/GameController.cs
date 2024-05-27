using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    void Awake()
    {
        // 单例模式
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ExecuteAfterDelay(float delay, System.Action action)
    {
        StartCoroutine(ExecuteAfterDelayCoroutine(delay, action));
    }

    private IEnumerator ExecuteAfterDelayCoroutine(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action();
    }
}
