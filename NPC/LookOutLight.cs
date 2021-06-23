using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HNS.NPC
{
    /// <summary>
    /// Effects of entering the gaze of the on look out NPC.
    /// </summary>
    public class LookOutLight : MonoBehaviour
    {
        /// <summary>
        /// Position where a player is teleported to if seen by the guard.
        /// </summary>
        private Transform _teleportPosition;

        private void Start()
        {
            _teleportPosition = GameObject.FindGameObjectWithTag("TeleportPosition").transform;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Head"))
            {
                if (_teleportPosition != null)
                {
                    ViveManager.Instance.player.transform.position = _teleportPosition.position;
                }
                else
                {
                    Debug.Log("TeleportPosition is null");
                }
            }
        }
    }
}
