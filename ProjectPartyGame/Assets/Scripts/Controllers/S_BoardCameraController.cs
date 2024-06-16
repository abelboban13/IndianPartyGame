using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_BoardCameraController : MonoBehaviour
{
    [SerializeField] private float _zOffSet = 5f;
    private S_BoardPlayer _trackedPlayer;

    private void Awake()
    {
        S_BoardManager.Instance.camera = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_trackedPlayer != null)
            transform.position = new Vector3(_trackedPlayer.transform.position.x, transform.position.y, _trackedPlayer.transform.position.z - _zOffSet);
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
