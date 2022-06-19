using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;
    public Transform playerSpawnPoint;

    public GameObject player;

    public static bool use_network = false;

    private void Start()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",this);
        }
        else
        {
            if (use_network)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                PhotonNetwork.Instantiate(this.playerPrefab.name, playerSpawnPoint.position, playerSpawnPoint.rotation, 0);
            }
            else
            {
                player = Instantiate(playerPrefab, playerSpawnPoint.position,playerSpawnPoint.rotation);
            }
        }
    }
}
