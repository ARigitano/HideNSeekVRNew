using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class StartGame : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public static StartGame Instance;

    public bool isStuck = false;

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

    public bool hasStarted = false;
    public GameObject sphereCatPrefab;
    public int maxNbBalls = 4;
    public int curNbBalls = 0;
    public Transform[] mapLimits;
    public GameObject instantiatedBallCat;

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

    [PunRPC]
    public void StartingGame()
    {
        isStuck = false;
        hasStarted = true;
        
        curNbBalls++;

        StartCoroutine(MoveBall());

        //photonView.RPC("InstantianteBallCat", RpcTarget.MasterClient);
    }

    IEnumerator MoveBall()
    {
        isStuck = false;
        sphereCat.SetActive(true);

        float xPos = Random.Range(mapLimits[0].transform.position.x, mapLimits[1].transform.position.x);
        float zPos = Random.Range(mapLimits[0].transform.position.z, mapLimits[2].transform.position.z);

        Vector3 movedPosition = new Vector3(xPos, this.gameObject.transform.position.y, zPos);

        sphereCat.transform.position = movedPosition;

        yield return new WaitForSeconds(1f);

        if(isStuck)
        {
            StartCoroutine(MoveBall());
        }
    }

    [PunRPC]
    public void InstantianteBallCat()
    {
        //random position for ball

        float xPos = Random.Range(mapLimits[0].transform.position.x, mapLimits[1].transform.position.x);
        float zPos = Random.Range(mapLimits[0].transform.position.z, mapLimits[2].transform.position.z);

        Vector3 instantiatedPosition = new Vector3(xPos, this.gameObject.transform.position.y, zPos);

        instantiatedBallCat = (GameObject)PhotonNetwork.Instantiate(sphereCatPrefab.name, instantiatedPosition, Quaternion.identity, 0);

        //recursive instantiateballcat if ball cat is in collider
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand") /*&& ismaster*/ && !hasStarted)
        {
            photonView.RPC("StartingGame", RpcTarget.AllBuffered);
        }
    }
}
