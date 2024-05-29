using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class p23s1darken : MonoBehaviour
{
    public Image targetImage; // 要變暗的圖片

    void Update()
    {
        // 按下數字鍵1時觸發
        if (Input.GetKeyDown(KeyCode.Keypad1)&&targetImage.color == Color.white&&player23move.canMove==true)
        {
            StartCoroutine(DarkenImageCoroutine());
        }
    }

    IEnumerator DarkenImageCoroutine()
    {
        // 將圖片顏色變暗
        targetImage.color = new Color32( 100, 100, 100, 200 );

        // 等待三秒
        yield return new WaitForSeconds(1f);

        // 恢復圖片原始顏色
        targetImage.color = Color.white;
    }
}
