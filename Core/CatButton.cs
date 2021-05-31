using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

namespace HNS.Core
{
    /// <summary>
    /// The player that presses this button becomes the cat.
    /// </summary>
    public class CatButton : MonoBehaviourPunCallbacks
    {
        public const byte BECOME_CAT_EVENT = 2;
        public const byte BECOME_MOUSE_EVENT = 3;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Hand"))
            {
                //if the hand of a player touches the button
                List<PlayerStat> players = PlayersManager.Instance.playerStats;

                foreach (PlayerStat player in players)
                {
                    if (player.rightHand == other.gameObject || player.leftHand == other.gameObject)
                    {
                        int viewHead = player.head.GetComponent<PhotonView>().ViewID;

                        object[] data = new object[] { viewHead };
                        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
                        PhotonNetwork.RaiseEvent(BECOME_CAT_EVENT, data, raiseEventOptions, SendOptions.SendReliable);
                    }
                    else
                    {
                        int viewHead = player.head.GetComponent<PhotonView>().ViewID;

                        object[] data = new object[] { viewHead };
                        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
                        PhotonNetwork.RaiseEvent(BECOME_MOUSE_EVENT, data, raiseEventOptions, SendOptions.SendReliable);
                    }
                }
            }
            else if (other.CompareTag("Decor"))
            {
                //if the button appears in a piece of furniture
                GameManager.Instance.isStuck = true;
            }
        }
    }
}
