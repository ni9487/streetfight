using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class shifttt : MonoBehaviour
{
    public void Playgame()
    {
     // 呼叫第一場景
        SceneManager.LoadScene("pickhero");

    }
    public void Quitgame()
    {
        Application.Quit();
    }
}
