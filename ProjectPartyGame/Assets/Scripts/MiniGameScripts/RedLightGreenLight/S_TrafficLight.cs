using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_TrafficLight : MonoBehaviour
{

    [SerializeField] private Material _redMaterial;
    [SerializeField] private Material _greenMaterial;
    [SerializeField] private  S_MiniGame _miniGame;

    [HideInInspector] public List<S_RedLightPlayer> _players;
    private bool _isActive = false;

    private void Awake()
    {
        S_GameManager.Instance.currentMiniGame = this.gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
