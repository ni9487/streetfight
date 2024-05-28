using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class vectoryShift : MonoBehaviour
{
    public void Playgame()
    {
        // ©I¥s²Ä¤@³õ´º
        SceneManager.LoadScene("pickhero");

    }
    public void Quitgame()
    {
        SceneManager.LoadScene("st");
    }
}
