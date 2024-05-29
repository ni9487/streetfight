using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class p2s2cool : MonoBehaviour
{
    public Text numberText; // 用於顯示數字的Text組件
    public float count=3;

    void Update()
    {
        // 按下數字鍵1時觸發
        if (Input.GetKeyDown(KeyCode.Keypad1)&&numberText.text == ""&&player2move.canMove==true)
        {
            numberText.text = "3";
            StartCoroutine(CountdownRoutine());
        }
    }

    IEnumerator CountdownRoutine()
    {
        // 每秒倒數一次，直到達到0
        for (int i = 3; i > 0; i--)
        {
            // 更新Text顯示的數字
            numberText.text = i.ToString();
            
            // 等待一秒
            yield return new WaitForSeconds(1f);
        }

        // 倒數結束，將數字隱藏
        numberText.text = "";
    }
}

