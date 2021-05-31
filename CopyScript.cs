using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyScript : Photon.Pun.MonoBehaviourPun
{
    public int index = 1;

    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine)
        {
            switch(index)
            {
                case 1: //head
                    transform.position = ViveManager.Instance.head.transform.position;
                    transform.rotation = ViveManager.Instance.head.transform.rotation;
                    break;
                case 2: //left hand
                    transform.position = ViveManager.Instance.leftHand.transform.position;
                    transform.rotation = ViveManager.Instance.leftHand.transform.rotation;
                    break;
                case 3: //right hand
                    transform.position = ViveManager.Instance.rightHand.transform.position;
                    transform.rotation = ViveManager.Instance.rightHand.transform.rotation;
                    break;
                default:
                    break;
            }
        }
    }
}
