using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Photon.Pun;
using ExitGames.Client.Photon;

public class NetManager : MonoBehaviourPunCallbacks
{
    /// <summary>Connect automatically? If false you can set this to true later on or call ConnectUsingSettings in your own scripts.</summary>
    public bool AutoConnect = true;

    public GameObject headPrefab;
    public GameObject LhandPrefab;
    public GameObject RhandPrefab;

    public const byte ADD_PLAYER_EVENT = 1;

    /// <summary>Used as PhotonNetwork.GameVersion.</summary>
    public byte Version = 1;

    /// <summary>Max number of players allowed in room. Once full, a new room will be created by the next connection attemping to join.</summary>
    [Tooltip("The max number of players allowed in room. Once full, a new room will be created by the next connection attemping to join.")]
    public byte MaxPlayers = 4;

    public int playerTTL = -1;

    public void Start()
    {
        if (this.AutoConnect)
        {
            this.ConnectNow();
        }
    }

    public void ConnectNow()
    {
        Debug.Log("ConnectAndJoinRandom.ConnectNow() will now call: PhotonNetwork.ConnectUsingSettings().");


        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = this.Version + "." + SceneManagerHelper.ActiveSceneBuildIndex;

    }


    // below, we implement some callbacks of the Photon Realtime API.
    // Being a MonoBehaviourPunCallbacks means, we can override the few methods which are needed here.


    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN. This client is now connected to Master Server in region [" + PhotonNetwork.CloudRegion +
            "] and can join a room. Calling: PhotonNetwork.JoinRandomRoom();");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby(). This client is now connected to Relay in region [" + PhotonNetwork.CloudRegion + "]. This script now calls: PhotonNetwork.JoinRandomRoom();");
        PhotonNetwork.JoinRandomRoom();
        //PhotonNetwork.NickName = "Player" + PhotonNetwork.PlayerList.Length.ToString();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed() was called by PUN. No random room available in region [" + PhotonNetwork.CloudRegion + "], so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");

        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = this.MaxPlayers };
        if (playerTTL >= 0)
            roomOptions.PlayerTtl = playerTTL;

        PhotonNetwork.CreateRoom(null, roomOptions, null);
    }

    // the following methods are implemented to give you some context. re-implement them as needed.
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected(" + cause + ")");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room in region [" + PhotonNetwork.CloudRegion + "]. Game is now running.");
        GameObject head = (GameObject)PhotonNetwork.Instantiate(headPrefab.name, ViveManager.Instance.head.transform.position, ViveManager.Instance.head.transform.rotation, 0);
        GameObject Lhand = (GameObject) PhotonNetwork.Instantiate(LhandPrefab.name, ViveManager.Instance.leftHand.transform.position, ViveManager.Instance.leftHand.transform.rotation, 0);
        GameObject Rhand = (GameObject) PhotonNetwork.Instantiate(RhandPrefab.name, ViveManager.Instance.rightHand.transform.position, ViveManager.Instance.rightHand.transform.rotation, 0);

        int viewHead = head.GetComponent<PhotonView>().ViewID;
        int viewLhand = Lhand.GetComponent<PhotonView>().ViewID;
        int viewRhand = Rhand.GetComponent<PhotonView>().ViewID;

        object[] data = new object[] { viewHead, viewLhand, viewRhand };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient }; 
        PhotonNetwork.RaiseEvent(ADD_PLAYER_EVENT, data, raiseEventOptions, SendOptions.SendReliable);
    }
}
