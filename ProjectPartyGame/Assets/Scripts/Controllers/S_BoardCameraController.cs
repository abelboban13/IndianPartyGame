using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_BoardCameraController : MonoBehaviour
{

    [Tooltip("the distance the camera stays away from the tracked player on the z axis")][SerializeField] private float _zOffSet = 5f;
    [SerializeField] private float _cameraSpeed;
    private GameObject _trackedPlayer;
    private bool _isPanning = false;
    private Quaternion _cameraRotation;
    private float _beseHeight;
    private void Awake()
    {
        if(S_BoardManager.Instance.boardCamera == null)
        {
            DontDestroyOnLoad(this);
            S_BoardManager.Instance.boardCamera = this;
        }
        else
            Destroy(gameObject);
        _cameraRotation = transform.rotation;
        _beseHeight = transform.position.y;
    }
    // Start is called before the first frame update
    public void StartTracking(GameObject trackedObject)
    {
        transform.position = new Vector3(trackedObject.transform.position.x, _beseHeight, trackedObject.transform.position.z + _zOffSet);
        _trackedPlayer = trackedObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(_trackedPlayer != null && !_isPanning)
            transform.position = new Vector3(_trackedPlayer.transform.position.x, transform.position.y, _trackedPlayer.transform.position.z + _zOffSet);

    }
    private void FixedUpdate()
    {
        if (_trackedPlayer != null && _isPanning)
        {
            if(transform.position.x > -65 && transform.position.x < 45 && transform.position.z < 16 && transform.position.z > -115)
            {
                Vector3 vec = _trackedPlayer.GetComponent<S_InputController>().MoveInput;
                transform.Translate(new Vector3(vec.x, vec.y, 0) * _cameraSpeed * Time.deltaTime);
            }
            else
                transform.position = new Vector3(_trackedPlayer.transform.position.x, transform.position.y , _trackedPlayer.transform.position.z);

        }
    }

    public void FollowPLayer(S_BoardPlayer player)
    {
        //center camera on player
        _trackedPlayer = player.gameObject;
    }

    public void Pan()
    {
        _isPanning = true;
        transform.position = new Vector3(_trackedPlayer.transform.position.x,transform.position.y + _zOffSet, _trackedPlayer.transform.position.z);
        transform.LookAt(_trackedPlayer.transform);
        transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, 180, transform.rotation.eulerAngles.z);
    }

    public void StopPan()
    {
        _isPanning = false;
        transform.position = new Vector3(_trackedPlayer.transform.position.x, _beseHeight, _trackedPlayer.transform.position.z + _zOffSet);
        transform.rotation = _cameraRotation;
    }

    public void DisconnectFromPlayer()
    {
        _trackedPlayer = null;
    }

    public void SetActive()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
