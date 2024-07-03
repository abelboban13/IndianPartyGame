using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReadyButton : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private void Awake()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
        Debug.Log("????");
    }
    private void FixedUpdate()
    {
        
    }
    public void Disable()
    {
        transform.parent.gameObject.SetActive(false);
        if(_camera != null)
            _camera.gameObject.SetActive(false);
    }
}
