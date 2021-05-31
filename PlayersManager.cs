using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class PlayersManager : MonoBehaviour, IOnEventCallback
{
    #region SINGLETON

    private static PlayersManager _instance;
    public static PlayersManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    [SerializeField]
    string nameBase = "Player";
    private int _nbPlayers = 0;
    public List<PlayerStat> playerStats = new List<PlayerStat>();
    private PhotonView photonView;

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == 1)
        {
            Debug.Log("Adding player");

            object[] data = (object[])photonEvent.CustomData;

            int viewHead = (int)data[0];
            int viewLhand = (int)data[1];
            int viewRhand = (int)data[2];

            photonView.RPC("AddPlayerRPC", RpcTarget.AllBuffered, viewHead, viewLhand, viewRhand);
        }
        else if (eventCode == 2)
        {
            Debug.Log("Becoming cat");

            object[] data = (object[])photonEvent.CustomData;

            int viewHead = (int)data[0];

            photonView.RPC("BecomeCat", RpcTarget.AllBuffered, viewHead, true);
        }
        else if (eventCode == 3)
        {
            Debug.Log("Becoming mouse");

            object[] data = (object[])photonEvent.CustomData;

            int viewHead = (int)data[0];

            photonView.RPC("BecomeCat", RpcTarget.AllBuffered, viewHead, false);
        }
        else if (eventCode == 4)
        {
            Debug.Log("Getting caught");

            object[] data = (object[])photonEvent.CustomData;

            int viewHead = (int)data[0];

            photonView.RPC("IncreaseScore", RpcTarget.AllBuffered, viewHead);
        }
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    

    private void Start()
    {
        photonView = gameObject.GetComponent<PhotonView>();
    }

    [PunRPC]
    public void AddPlayerRPC(int viewHead, int viewLhand, int viewRhand)
    {
        GameObject head = PhotonView.Find(viewHead).gameObject;
        GameObject Lhand = PhotonView.Find(viewLhand).gameObject;
        GameObject Rhand = PhotonView.Find(viewRhand).gameObject;

        PlayerStat player = head.GetComponent<PlayerStat>();

        _nbPlayers++;
        player.id = _nbPlayers;
        player.playerName = nameBase + _nbPlayers;

        player.head = head;
        player.leftHand = Lhand;
        player.rightHand = Rhand;

        playerStats.Add(player);
        UIScore.Instance.AddPlayer(player);
    }

    [PunRPC]
    public void RemovePlayer(PlayerStat player)
    {
        UIScore.Instance.RemovePlayer(player);

        _nbPlayers--;
    }

    [PunRPC]
    public void BecomeCat(int viewHead, bool becomeCat)
    {
        GameObject head = PhotonView.Find(viewHead).gameObject;

        PlayerStat player = head.GetComponent<PlayerStat>();

        player.isCat = becomeCat;

        UIScore.Instance.UpdateStatus(player, becomeCat);
    }

    [PunRPC]
    public void IncreaseScore(int viewHead)
    {
        GameObject head = PhotonView.Find(viewHead).gameObject;

        PlayerStat player = head.GetComponent<PlayerStat>();

        player.score++;
        UIScore.Instance.UpdateScore(player);
    }
}
