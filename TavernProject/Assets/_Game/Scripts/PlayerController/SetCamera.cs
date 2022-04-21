using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using UnityEngine;

public class SetCamera : MonoBehaviourPunCallbacks
{
    void Start()
    {
        if(!photonView.IsMine)
            return;
        
        var vcam = Camera.main.GetComponent<CinemachineFreeLook>();
        vcam.LookAt = this.transform;
        vcam.Follow = this.transform;    
    }
    
}
