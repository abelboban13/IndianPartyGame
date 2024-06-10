using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_BoardCameraController : MonoBehaviour
{

    private S_BoardPlayer _trackedPLayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FollowPLayer(S_BoardPlayer player)
    {
        //center camera on player
        _trackedPLayer = player;

        transform.position = player.cameraSocket.position;
        transform.SetParent(player.cameraSocket);
    }
}
