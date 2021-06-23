using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

namespace HNS.Core
{
    /// <summary>
    /// Manages the game.
    /// </summary>
    public class GameManager : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        #region SINGLETON
        public static GameManager Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
        #endregion

        /// <summary>
        /// Did the cat button spawn in a piece of furniture?
        /// </summary>
        public bool isStuck = false;

        /// <summary>
        /// Has the game started?
        /// </summary>
        public bool hasStarted = false;
        /// <summary>
        /// Prefab for the cat button. OBSOLETE?
        /// </summary>
        //public GameObject catButtonPrefab;
        /// <summary>
        /// Maximum number of cat buttons that can spawn during a game.
        /// </summary>
        public int maxNbCatButtons = 4;
        /// <summary>
        /// Number of cat buttons that have already spawned.
        /// </summary>
        public int curNbCatButtons = 0;
        /// <summary>
        /// Extremities of the map.
        /// </summary>
        public Transform[] mapLimits;
        /// <summary>
        /// Instance of the cat button. OBSOLETE?
        /// </summary>
        //public GameObject instantiatedCatButton;
        /// <summary>
        /// The button that transforms a player into a cat. NEW SYSTEM 
        /// </summary>
        public GameObject sphereCat;

        public void OnEvent(EventData photonEvent)
        {
            byte eventCode = photonEvent.Code;
            if (eventCode == 2)
            {
                Debug.Log("Changing position");

                photonView.RPC("StartingGame", RpcTarget.AllBuffered);
            }
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        [PunRPC]
        public void StartingGame()
        {
            isStuck = false;
            hasStarted = true;

            curNbCatButtons++;

            StartCoroutine(MoveCatButton());
        }

        /// <summary>
        /// Makes the cat button move to a new spot.
        /// </summary>
        /// <returns></returns>
        IEnumerator MoveCatButton()
        {
            isStuck = false;
            sphereCat.SetActive(true);

            float xPos = Random.Range(mapLimits[0].transform.position.x, mapLimits[1].transform.position.x);
            float zPos = Random.Range(mapLimits[0].transform.position.z, mapLimits[2].transform.position.z);

            Vector3 movedPosition = new Vector3(xPos, this.gameObject.transform.position.y, zPos);

            sphereCat.transform.position = movedPosition;

            yield return new WaitForSeconds(1f);

            if (isStuck)
            {
                StartCoroutine(MoveCatButton());
            }
        }

        /// <summary>
        /// Instantiate the cat button. OBSOLETE?
        /// </summary>
        /*[PunRPC]
        public void InstantianteBallCat()
        {
            //random position for ball

            float xPos = Random.Range(mapLimits[0].transform.position.x, mapLimits[1].transform.position.x);
            float zPos = Random.Range(mapLimits[0].transform.position.z, mapLimits[2].transform.position.z);

            Vector3 instantiatedPosition = new Vector3(xPos, this.gameObject.transform.position.y, zPos);

            instantiatedCatButton = (GameObject)PhotonNetwork.Instantiate(catButtonPrefab.name, instantiatedPosition, Quaternion.identity, 0);

            //recursive instantiateballcat if ball cat is in collider
        }*/

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("start sphere touched");
            if (other.CompareTag("Hand") /*&& ismaster*/ && !hasStarted)
            {
                //if a player's hand touches the start button.
                photonView.RPC("StartingGame", RpcTarget.AllBuffered);
            }
        }
    }
}
