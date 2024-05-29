using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class p23s3darken : MonoBehaviour
{
    public Image targetImage; // 要變暗的圖片

    void Update()
    {
        // 按下數字鍵1時觸發
        if (Input.GetKeyDown(KeyCode.Keypad3) && targetImage.color == Color.white && player23move.mp >= 24&&player23move.canMove==true)
        {
            DarkenImageCoroutine();
        }
        if (player23move.mp < 24)
        {
            DarkenImageCoroutine();
        }
        if (player23move.mp >= 24)
        {
            targetImage.color = Color.white;
        }
    }

    void DarkenImageCoroutine()
    {
        // 將圖片顏色變暗
        targetImage.color = new Color32(100, 100, 100, 200);
    }
}

