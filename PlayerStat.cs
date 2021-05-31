using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class PlayerStat : MonoBehaviourPunCallbacks
{
    public GameObject head;
    public GameObject leftHand;
    public GameObject rightHand;

    public int id;
    public string playerName;
    public int score;

    public bool isCat = false;
    public bool isDead = false;

    public UIPlayerScore uiPlayerScore;

    public const byte GET_CAUGHT_EVENT = 4;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            List<PlayerStat> players = PlayersManager.Instance.playerStats;

            foreach (PlayerStat player in players)
            {
                if (player.rightHand == other.gameObject || player.leftHand == other.gameObject)
                {
                    if(player.isCat && player != this)
                    {
                        int viewHead = player.head.GetComponent<PhotonView>().ViewID;

                        object[] data = new object[] { viewHead };
                        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
                        PhotonNetwork.RaiseEvent(GET_CAUGHT_EVENT, data, raiseEventOptions, SendOptions.SendReliable);

                        //PlayersManager.Instance.IncreaseScore(player);
                    }
                    break;
                }
            }
        }
    }
}
