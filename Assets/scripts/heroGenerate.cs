using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heroGenerate : MonoBehaviour
{
    public GameObject Player; // 1號角色的引用
    public GameObject Player2; // 2號角色的引用
    public GameObject Player3;

    public GameObject Player21;
    public GameObject Player22;
    public GameObject Player23;

    public GameObject p1skill; // 1號角色的引用
    public GameObject p2skill; // 2號角色的引用
    public GameObject p3skill;

    public GameObject p21skill; // 1號角色的引用
    public GameObject p22skill; // 2號角色的引用
    public GameObject p23skill;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerInfo.firstCharacterSelected == "allain")
        {
            // 讓1號角色登場
            Player.SetActive(true);
            p1skill.SetActive(true);
        } 
        if ( PlayerInfo.firstCharacterSelected == "liliana")
        {
            // 讓2號角色登場
            Player2.SetActive(true);
            p2skill.SetActive(true);
        } 
        if ( PlayerInfo.firstCharacterSelected == "yorn")
        {
            // 讓2號角色登場
            Player3.SetActive(true);
            p3skill.SetActive(true);
        } 

        if ( PlayerInfo.secondCharacterSelected == "allain2")
        {
            // 讓2號角色登場
            Player21.SetActive(true);
            p21skill.SetActive(true);
        } 
        if ( PlayerInfo.secondCharacterSelected == "liliana2")
        {
            // 讓2號角色登場
            Player22.SetActive(true);
            p22skill.SetActive(true);
        } 
        if ( PlayerInfo.secondCharacterSelected == "yorn2")
        {
            // 讓2號角色登場
            Player23.SetActive(true);
            p23skill.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
