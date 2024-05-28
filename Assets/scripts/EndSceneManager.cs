using UnityEngine;
using TMPro;

public class EndSceneManager : MonoBehaviour
{
    public TMP_Text resultText; // 请在 Unity 编辑器中将此文本对象链接到你的 UI 文本组件

    void Start()
    {
        resultText.alignment = TextAlignmentOptions.Center;

        float firstHealth = PlayerInfo.firstCharacterHealth;
        float secondHealth = PlayerInfo.secondCharacterHealth;

        if (firstHealth > secondHealth)
        {
            resultText.text = "Player1 Wins!";
        }
        else if (secondHealth > firstHealth)
        {
            resultText.text = "Player2 Wins!";
        }
        else
        {
            resultText.text = "It's a Draw!";
        }
    }
}