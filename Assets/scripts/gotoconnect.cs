using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Realtime;
using System.Text;

public class gotoconnect : MonoBehaviourPunCallbacks
{
    void Start()
    {
        if(PhotonNetwork.IsConnected==false)
        {
            SceneManager.LoadScene("connectscene");
        }
        else
        {
            PhotonNetwork.JoinLobby();
        }
    }
}