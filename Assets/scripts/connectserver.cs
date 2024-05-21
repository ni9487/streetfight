using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class connectserver : MonoBehaviourPunCallbacks
{
    public void onclickstart()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene=true;
        print("success");
    }
    public override void OnConnectedToMaster()
    {
        print("connected success");
        SceneManager.LoadScene("lobby");
    }
}
