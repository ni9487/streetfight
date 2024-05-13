using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class pickCharacter : MonoBehaviour
{
    public Button button1; //需要手動在Unity把對應的按鈕拖到這裡
    public Button button2; //需要手動在Unity把對應的按鈕拖到這裡

    private Color defaultColor = Color.white; //這裡可以設定你的默認顏色
    private Color selectedColor = new Color32(255,0,0,150); //這裡可以設定你的選中顏色

    public void Choose(string character) 
    {
        button1.image.color = defaultColor;
        button2.image.color = defaultColor;

        //Next, let's color the selected button in red and update our character variable.
        switch(character) 
        {
            case "allain":
                button1.image.color = selectedColor;
                break;
            case "liliana":
                button2.image.color = selectedColor;
                break;
        }
    
        PlayerInfo.characterSelected = character;
    }
}
