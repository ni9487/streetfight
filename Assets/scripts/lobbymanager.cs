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

    public void onclickcreateroom()
    {
        string roomname=getroomname();
        if(roomname.Length>0)
        {
            PhotonNetwork.CreateRoom(roomname);
        }
        else
        {
            print("name can't be empty");
        }
    }

    public override void OnJoinedRoom()
    {
        print("room joined");
        SceneManager.LoadScene("roomscene");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        StringBuilder sb=new StringBuilder();
        foreach(RoomInfo roominfo in roomList)
        {
            sb.AppendLine("→ "+ roominfo.Name);
        }
        textroomlist.text = sb.ToString();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
