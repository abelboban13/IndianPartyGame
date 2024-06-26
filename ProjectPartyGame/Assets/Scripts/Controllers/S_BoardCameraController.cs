using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_BoardCameraController : MonoBehaviour
{

    [Tooltip("the distance the camera stays away from the tracked player on the z axis")][SerializeField] private float _zOffSet = 5f; 
    private S_BoardPlayer _trackedPlayer;

    private void Awake()
    {
        S_BoardManager.Instance.boardCamera = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(S_BoardManager.Instance.startingSpace.transform.position.x, transform.position.y, S_BoardManager.Instance.startingSpace.transform.position.z + _zOffSet);
    }

    // Update is called once per frame
    void Update()
    {
        if(_trackedPlayer != null)
            transform.position = new Vector3(_trackedPlayer.transform.position.x, transform.position.y, _trackedPlayer.transform.position.z + _zOffSet);
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
