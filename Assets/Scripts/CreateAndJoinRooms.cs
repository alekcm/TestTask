using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    //public InputField roomName;
   //public TMPro.TextMeshPro textMeshPro;
    public TMPro.TMP_InputField createRoomName;
    public TMPro.TMP_InputField joinRoomName;

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createRoomName.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinRoomName.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}
