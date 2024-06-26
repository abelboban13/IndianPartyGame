using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Projectile : MonoBehaviour
{
    public int range = 3;
    [SerializeField] private int coinsLostOnImpact;
    [HideInInspector] public S_BoardPlayer player;
    private S_Space targetSpace;
    private S_Space currentSpace;
    [SerializeField] protected float _speed = 6;

    // Start is called before the first frame update
    void Start()
    {
        currentSpace = player.currentSpace;
        StartCoroutine(Move());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Go()
    {
        var step = _speed * Time.deltaTime; // calculate distance to move

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetSpace.transform.position.x, transform.position.y, targetSpace.transform.position.z), step);
        transform.LookAt(new Vector3(targetSpace.transform.position.x, transform.position.y, targetSpace.transform.position.z));
    }

    IEnumerator Move()
    {
        targetSpace = currentSpace.GiveNextSpace();
        for (int i = 0; i < range; i++)
        {
            targetSpace = currentSpace.GiveNextSpace();
            
            //handles the actual movement of the missile. rounded so that its not trying to get to an infinitly prisice position
            while ((Mathf.Round(transform.position.x) != Mathf.Round(targetSpace.transform.position.x)) || (Mathf.Round(transform.position.z) != Mathf.Round(targetSpace.transform.position.z)))
            {
                yield return new WaitForFixedUpdate();
                Go();
                yield return new WaitForSeconds(.1f);
                //Debug.Log(targetSpace);
            }
            currentSpace = targetSpace;
            if(currentSpace._playersOnSpace.Count > 0)
            {
                foreach(S_BoardPlayer damaged in currentSpace._playersOnSpace)
                {
                    damaged.ChangeCoins(-coinsLostOnImpact);
                    Destroy(gameObject, 1);
                    yield return null;
                }
            }
        }
        currentSpace = targetSpace;
        Destroy(gameObject,1);
        yield return null;
    }
}
