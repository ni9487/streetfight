using UnityEngine;
using TMPro;

public class EndSceneManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player12;
    public GameObject player21;
    public GameObject player23;

    public TMP_Text resultText; // 请在 Unity 编辑器中将此文本对象链接到你的 UI 文本组件

    void Start()
    {
        resultText.alignment = TextAlignmentOptions.Center;

        float firstHealth = PlayerInfo.firstCharacterHealth;
        float secondHealth = PlayerInfo.secondCharacterHealth;

        if (firstHealth > secondHealth)
        {
            resultText.text = "Player1 Wins!";
            if(PlayerInfo.firstCharacterSelected=="allain")
            {
                Instantiate(player1, new Vector3(471, 250, 0), Quaternion.identity);
            }
            if(PlayerInfo.firstCharacterSelected=="liliana")
            {
                Instantiate(player12, new Vector3(471, 250, 0), Quaternion.identity);
            }
            if(PlayerInfo.firstCharacterSelected=="yorn")
            {
                Instantiate(player3, new Vector3(471, 250, 0), Quaternion.identity);
            }
        }
        else if (secondHealth > firstHealth)
        {
            resultText.text = "Player2 Wins!";
            if(PlayerInfo.secondCharacterSelected=="allain2")
            {
                Instantiate(player21, new Vector3(471, 250, 0), Quaternion.identity);
            }
            if(PlayerInfo.secondCharacterSelected=="liliana2")
            {
                Instantiate(player2, new Vector3(471, 250, 0), Quaternion.identity);
            }
            if(PlayerInfo.secondCharacterSelected=="yorn2")
            {
                Instantiate(player23, new Vector3(471, 250, 0), Quaternion.identity);
            }
        }
        else
        {
            resultText.text = "It's a Draw!";
        }
    }
}