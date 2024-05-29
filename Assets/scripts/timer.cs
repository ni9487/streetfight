using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class timer : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player12;
    public GameObject player21;
    public GameObject player23;

    public static int draw=0;

    public int countdownTime = 10;
    private TMP_Text countdownDisplay;

    private void Start()
    {
        // 获取 TMP_Text 组件
        countdownDisplay = GetComponent<TMP_Text>();

        // 开始倒计时协程
        StartCoroutine(CountdownToStart());
    }

    IEnumerator CountdownToStart()
    {
        while (countdownTime > 0)
        {
            // 更新倒计时文本
            countdownDisplay.text = countdownTime.ToString();

            // 调整对齐方式
            if (countdownTime < 10)
            {
                countdownDisplay.alignment = TextAlignmentOptions.Center;
            }

            // 每秒更新一次
            yield return new WaitForSeconds(1f);

            countdownTime--;
        }
        countdownDisplay.text = "0";
        //LoadNewScene();
        StartCoroutine(delayvictory());
    }

    void LoadNewScene()
    {
        // 确保在构建设置中添加了你要加载的场景
        // 使用场景的名称进行加载
        SceneManager.LoadScene("New Scene 1");  // 替换为你的新场景名称
    }

    IEnumerator delayvictory()
    {
        if (player1.activeSelf)
            PlayerInfo.firstCharacterHealth = player1.GetComponent<playermove>().hp;
        if (player12.activeSelf)
            PlayerInfo.firstCharacterHealth = player12.GetComponent<player12move>().hp;
        if (player3.activeSelf)
            PlayerInfo.firstCharacterHealth = player3.GetComponent<player3move>().hp;

        if (player21.activeSelf)
            PlayerInfo.secondCharacterHealth = player21.GetComponent<player21move>().hp;
        if (player2.activeSelf)
            PlayerInfo.secondCharacterHealth = player2.GetComponent<player2move>().hp;
        if (player23.activeSelf)
            PlayerInfo.secondCharacterHealth = player23.GetComponent<player23move>().hp;


        float firstHealth = PlayerInfo.firstCharacterHealth;
        float secondHealth = PlayerInfo.secondCharacterHealth;

        if (firstHealth < secondHealth)
        {
            if(PlayerInfo.firstCharacterSelected=="allain")
            {
                playermove.died=1;
            }
            if(PlayerInfo.firstCharacterSelected=="liliana")
            {
                player12move.died=1;
            }
            if(PlayerInfo.firstCharacterSelected=="yorn")
            {
                player3move.died=1;
            }
        }
        else if (secondHealth < firstHealth)
        {
            if(PlayerInfo.secondCharacterSelected=="allain2")
            {
                player21move.died=1;
            }
            if(PlayerInfo.secondCharacterSelected=="liliana2")
            {
                player2move.died=1;
            }
            if(PlayerInfo.secondCharacterSelected=="yorn2")
            {
                player23move.died=1;
            }
        }
        else
        {
            draw=1;
        }

        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("New Scene 1"); 
    }
}

