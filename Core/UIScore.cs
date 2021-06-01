using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace HNS.Core
{
    /// <summary>
    /// Displays the score and status for all the players.
    /// </summary>
    public class UIScore : MonoBehaviourPunCallbacks
    {
        #region SINGLETON

        private static UIScore _instance;
        public static UIScore Instance { get { return _instance; } }

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

        /// <summary>
        /// Prefab for the score line of a player.
        /// </summary>
        [SerializeField]
        private GameObject scorePrefab;

        /// <summary>
        /// Adds a score line for a new player.
        /// </summary>
        /// <param name="player">The new player</param>
        public void AddPlayer(PlayerStat player)
        {
            GameObject newScorePrefab = (GameObject)Instantiate(scorePrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);

            newScorePrefab.transform.parent = this.gameObject.transform;

            UIPlayerScore playerScore = newScorePrefab.GetComponent<UIPlayerScore>();
            playerScore.playerName.text = player.playerName;

            player.uiPlayerScore = playerScore;
        }

        /// <summary>
        /// Updates the status for one player.
        /// </summary>
        /// <param name="player">The player with updated status</param>
        /// <param name="isCat">Is this player the cat?</param>
        [PunRPC]
        public void UpdateStatus(PlayerStat player, bool isCat)
        {
            if (isCat)
                player.uiPlayerScore.playerStatus.text = "Cat";
            else
                player.uiPlayerScore.playerStatus.text = "Mouse";
        }

        /// <summary>
        /// Remove the UI line for one player.
        /// </summary>
        /// <param name="player">The player to remove from UI</param>
        [PunRPC]
        public void RemovePlayer(PlayerStat player)
        {
            Destroy(player.uiPlayerScore);
        }

        /// <summary>
        /// Updates the score for one player.
        /// </summary>
        /// <param name="player">The player to update score</param>
        [PunRPC]
        public void UpdateScore(PlayerStat player)
        {
            player.uiPlayerScore.playerScore.text = player.score.ToString();
        }

        /// <summary>
        /// Called when a player enters the room.
        /// </summary>
        /// <param name="newPlayer">The player that entered</param>
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Instantiate(scorePrefab).GetComponent<UIPlayerScore>().SetUp(newPlayer);
        }
    }
}
