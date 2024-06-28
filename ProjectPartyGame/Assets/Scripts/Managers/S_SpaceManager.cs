using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_SpaceManager : S_Singleton<S_SpaceManager>
{
    [SerializeField] private GameObject keyObject;
    private List<S_Space> _spaceList = new List<S_Space>();
    private List<S_Space> _mangoSpaces = new List<S_Space>();

    private int _currentRewardIndex = -1;
    // Start is called before the first frame update
    void Start()
    {
        foreach(S_Space space in GetComponentsInChildren<S_Space>())
        {
            _spaceList.Add(space);
            if(space.spaceType == SpaceType.Reward)
                _mangoSpaces.Add(space);
            space.transform.parent = null;
            DontDestroyOnLoad(space);
        }

        ManageMangoes();
    }

    public void ManageMangoes()
    {
        int newIndex = _currentRewardIndex;
        while(newIndex == _currentRewardIndex)
        {
            newIndex = Random.Range(0, _mangoSpaces.Count);
        }
        _currentRewardIndex = newIndex;
        _mangoSpaces[newIndex].GenerateReward(keyObject);
    }

    public void LoadSpaces()
    {
        foreach (S_Space space in _spaceList)
        {
            space.Load();
            if(space.isReward)
               space.GenerateReward(keyObject);
        }          
    }
    public void UnLoadSpaces()
    {
        foreach (S_Space space in _spaceList)
            space.UnLoad();
    }



}
