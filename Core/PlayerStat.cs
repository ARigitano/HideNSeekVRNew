using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

namespace HNS.Core
{
    /// <summary>
    /// Holds the stats of a player.
    /// </summary>
    public class PlayerStat : MonoBehaviourPunCallbacks
    {
        /// <summary>
        /// Instance of the player's head.
        /// </summary>
        public GameObject head;
        /// <summary>
        /// Instance of the player's left hand.
        /// </summary>
        public GameObject leftHand;
        /// <summary>
        /// Instance of the player's right hand.
        /// </summary>
        public GameObject rightHand;

        /// <summary>
        /// Id of the player.
        /// </summary>
        public int id;
        /// <summary>
        /// Name of the player.
        /// </summary>
        public string playerName;
        /// <summary>
        /// Score of the player.
        /// </summary>
        public int score;

        /// <summary>
        /// Is the player the cat?
        /// </summary>
        public bool isCat = false;
        /// <summary>
        /// Has the player been caught by the cat?
        /// </summary>
        public bool isCaught = false;

        /// <summary>
        /// Element displaying the player's score on the score UI.
        /// </summary>
        public UIPlayerScore uiPlayerScore;

        public const byte GET_CAUGHT_EVENT = 4;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Hand"))
            {
                //if the hand of a player touches the player
                List<PlayerStat> players = PlayersManager.Instance.playerStats;

                foreach (PlayerStat player in players)
                {
                    if (player.rightHand == other.gameObject || player.leftHand == other.gameObject)
                    {
                        if (player.isCat && player != this)
                        {
                            int viewHead = player.head.GetComponent<PhotonView>().ViewID;

                            object[] data = new object[] { viewHead };
                            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
                            PhotonNetwork.RaiseEvent(GET_CAUGHT_EVENT, data, raiseEventOptions, SendOptions.SendReliable);
                        }
                        break;
                    }
                }
            }
        }
    }
}
