using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class p2s3darken : MonoBehaviour
{
    public Image targetImage; // 要變暗的圖片

    void Update()
    {
        // 按下數字鍵1時觸發
        if (Input.GetKeyDown(KeyCode.Keypad3)&&targetImage.color == Color.white)
        {
            StartCoroutine(DarkenImageCoroutine());
        }
    }

    IEnumerator DarkenImageCoroutine()
    {
        // 將圖片顏色變暗
        targetImage.color = new Color32( 100, 100, 100, 200 );

        // 等待三秒
        yield return new WaitForSeconds(9f);

        // 恢復圖片原始顏色
        targetImage.color = Color.white;
    }
}
