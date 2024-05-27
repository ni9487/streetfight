using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class timer : MonoBehaviour
{
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
    }

    
}

