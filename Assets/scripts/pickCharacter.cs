using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class pickCharacter : MonoBehaviour
{
    public Button button1; //需要手動在Unity把對應的按鈕拖到這裡
    public Button button2; //需要手動在Unity把對應的按鈕拖到這裡
    public Button button3; //需要手動在Unity把對應的按鈕拖到這裡

    public Button button21; //需要手動在Unity把對應的按鈕拖到這裡
    public Button button22; //需要手動在Unity把對應的按鈕拖到這裡
    public Button button23; //需要手動在Unity把對應的按鈕拖到這裡

    private Color defaultColor = new Color32(255, 255, 255, 230); //這裡可以設定你的默認顏色
    private Color selectedColor = new Color32(0, 100, 220, 240); //這裡可以設定你的選中顏色

    private string firstCharacter; // 保存第一隻角色
    private string secondCharacter; // 保存第二隻角色

    void Start()
    {
        // 假設這些按鍵在Unity編輯器裡已經指派
        button1.onClick.AddListener(() => ChooseFirstCharacter("allain"));
        button2.onClick.AddListener(() => ChooseFirstCharacter("liliana"));
        button3.onClick.AddListener(() => ChooseFirstCharacter("yorn"));

        button21.onClick.AddListener(() => ChooseSecondCharacter("allain2"));
        button22.onClick.AddListener(() => ChooseSecondCharacter("liliana2"));
        button23.onClick.AddListener(() => ChooseSecondCharacter("yorn2"));
    }

    void ChooseFirstCharacter(string character)
    {
        firstCharacter = character;
        UpdateUI();
    }

    void ChooseSecondCharacter(string character)
    {
        secondCharacter = character;
        UpdateUI();
    }

    void UpdateUI()
    {
        // Reset all button colors
        button1.image.color = defaultColor;
        button2.image.color = defaultColor;
        button3.image.color = defaultColor;
        button21.image.color = defaultColor;
        button22.image.color = defaultColor;
        button23.image.color = defaultColor;

        // Update first character button color
        switch (firstCharacter)
        {
            case "allain":
                button1.image.color = selectedColor;
                break;
            case "liliana":
                button2.image.color = selectedColor;
                break;
            case "yorn":
                button3.image.color = selectedColor;
                break;
        }

        // Update second character button color
        switch (secondCharacter)
        {
            case "allain2":
                button21.image.color = selectedColor;
                break;
            case "liliana2":
                button22.image.color = selectedColor;
                break;
            case "yorn2":
                button23.image.color = selectedColor;
                break;
        }

        PlayerInfo.firstCharacterSelected = firstCharacter;
        PlayerInfo.secondCharacterSelected = secondCharacter;

    }
}