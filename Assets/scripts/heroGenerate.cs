using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heroGenerate : MonoBehaviour
{
    public GameObject Player; // 1號角色的引用
    public GameObject Player2; // 2號角色的引用

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerInfo.characterSelected == "allain")
        {
            // 讓1號角色登場
            Player.SetActive(true);
        } 
        else if ( PlayerInfo.characterSelected == "liliana")
        {
            // 讓2號角色登場
            Player2.SetActive(true);
        } 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
