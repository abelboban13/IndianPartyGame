using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_BoardCameraController : MonoBehaviour
{

    private S_BoardPlayer _trackedPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_trackedPlayer != null)
            transform.position = new Vector3(_trackedPlayer.transform.position.x, transform.position.y, _trackedPlayer.transform.position.z);
    }

    public void FollowPLayer(S_BoardPlayer player)
    {
        //center camera on player
        _trackedPlayer = player;


    }

    public void DisconnectFromPlayer()
    {
        _trackedPlayer = null;
    }
}
