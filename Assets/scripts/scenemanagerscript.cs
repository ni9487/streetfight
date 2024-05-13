using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scenemanagerscript : MonoBehaviour
{
    public void loadscene(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }
}

public static class PlayerInfo {
    public static string characterSelected;
}
