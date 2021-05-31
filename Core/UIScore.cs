using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace HNS.Core
{
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

        [SerializeField]
        private GameObject scorePrefab;

        public void AddPlayer(PlayerStat player)
        {
            //GameObject newScorePrefab = (GameObject)PhotonNetwork.Instantiate(scorePrefab.name, this.gameObject.transform.position, this.gameObject.transform.rotation, 0);

            GameObject newScorePrefab = (GameObject)Instantiate(scorePrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);

            newScorePrefab.transform.parent = this.gameObject.transform;

            UIPlayerScore playerScore = newScorePrefab.GetComponent<UIPlayerScore>();
            playerScore.playerName.text = player.playerName;

            player.uiPlayerScore = playerScore;
        }

        [PunRPC]
        public void UpdateStatus(PlayerStat player, bool isCat)
        {
            if (isCat)
                player.uiPlayerScore.playerStatus.text = "Cat";
            else
                player.uiPlayerScore.playerStatus.text = "Mouse";
        }

        [PunRPC]
        public void RemovePlayer(PlayerStat player)
        {
            Destroy(player.uiPlayerScore);
        }

        [PunRPC]
        public void UpdateScore(PlayerStat player)
        {
            player.uiPlayerScore.playerScore.text = player.score.ToString();
        }



        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log("okoko");

            Instantiate(scorePrefab).GetComponent<UIPlayerScore>().SetUp(newPlayer);
        }
    }
}
