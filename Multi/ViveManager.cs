using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveManager : MonoBehaviour
{
    public GameObject head;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject player;

    public static ViveManager Instance;

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
}
