using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Realtime;
using System.Text;

public class roomscenemanager : MonoBehaviourPunCallbacks
{
    [SerializeField] Text textroomname;
    [SerializeField] Text textPlayerList;
    void Start()
    {
        if(PhotonNetwork.CurrentRoom == null)
        {
            SceneManager. LoadScene ("LobbyScene");
        }
        else
        {
            textroomname. text = PhotonNetwork.CurrentRoom. Name;
            UpdatePlayerList();
        }
    }
    public void UpdatePlayerList()
    {
        StringBuilder sb = new StringBuilder();
        foreach(var kvp in PhotonNetwork.CurrentRoom.Players)
        {
            sb. AppendLine("→ "+kvp.Value.NickName);
        }
        textPlayerList.text = sb. ToString();
    }
    public override void OnPlayerEnteredRoom(Player newplayer)
    {
        UpdatePlayerList();
        Debug.Log("Player entered: " + newplayer.NickName); 
    }
    public override void OnPlayerLeftRoom(Player otherplayer)
    {
        UpdatePlayerList();
        Debug.Log("Player left: " + otherplayer.NickName); 
    }
}
