using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

namespace HNS.Core
{
    /// <summary>
    /// Displays the score and status of a player.
    /// </summary>
    public class UIPlayerScore : MonoBehaviourPunCallbacks
    {
        /// <summary>
        /// Text for the player's name.
        /// </summary>
        public TextMeshProUGUI playerName;
        /// <summary>
        /// Text for the player's score.
        /// </summary>
        public TextMeshProUGUI playerScore;
        /// <summary>
        /// Text for the player status (mouse, cat or caught).
        /// </summary>
        public TextMeshProUGUI playerStatus;

        Player player;

        /// <summary>
        /// Initializes the score UI.
        /// </summary>
        /// <param name="_player">The player for this UI</param>
        public void SetUp(Player _player)
        {
            player = _player;
            playerName.text = _player.NickName;
        }

        /// <summary>
        /// Called when a player leaves the room.
        /// </summary>
        /// <param name="otherPlayer">The player that left the room</param>
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (player == otherPlayer)
            {
                Destroy(gameObject);
            }
        }

        public override void OnLeftRoom()
        {
            Destroy(gameObject);
        }
    }
}
