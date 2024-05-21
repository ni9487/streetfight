using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Realtime;
using System.Text;

public class lobbymanager : MonoBehaviourPunCallbacks
{
    [SerializeField] InputField inputroomname;
    [SerializeField] InputField inputplayername;
    [SerializeField] Text textroomlist;
    // Start is called before the first frame update
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
    public override void OnJoinedLobby()
    {
        print("lobby joined");
    }

    public string getroomname()
    {
        string roomname = inputroomname.text;
        return roomname.Trim();
    }

    public string getplayername()
    {
        string playername = inputplayername.text;
        return playername.Trim();
    }

    public void onclickcreateroom()
    {
        string roomname=getroomname();
        string playername=getplayername();
        if(roomname.Length>0&&playername.Length>0)
        {
            PhotonNetwork.CreateRoom(roomname);
            PhotonNetwork.LocalPlayer.NickName=playername;
        }
        else
        {
            print("name can't be empty");
        }
    }

    public void onclickjoinroom()
    {
        string roomname=getroomname();
        string playername=getplayername();
        if(roomname.Length>0&&playername.Length>0)
        {
            PhotonNetwork.JoinRoom(roomname);
            PhotonNetwork.LocalPlayer.NickName=playername;
        }
        else
        {
            print("invalid roomname");
        }
    }

    public override void OnJoinedRoom()
    {
        print("room joined");
        SceneManager.LoadScene("startscene");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        print("update");
        StringBuilder sb=new StringBuilder();
        foreach(RoomInfo roominfo in roomList)
        {
            if(roominfo.PlayerCount>0)
            {
                sb.AppendLine("→ "+ roominfo.Name );
            }
        }
        textroomlist.text = sb.ToString();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
