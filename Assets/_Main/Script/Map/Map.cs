using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField]
    private float destroyDistance = 15;
    private MapSpawner mapSpawner;
    private Transform playerTransform;

    [SerializeField] public float direction;
    [SerializeField] GameObject rotator;

    [SerializeField] GameObject obsPos;

    [SerializeField] public GameObject player;
    public void Setup(MapSpawner mapSpawner, Transform playerTransform)
    {
        this.mapSpawner = mapSpawner;
        this.playerTransform = playerTransform;
    }

    private void Start()
    {
        if (direction != 0)
        {
            GameObject temp=Instantiate(rotator, transform.position,transform.rotation);

            //Instantiate(rotators[1], transform.position, transform.rotation);
            if (direction == 1)
            {
                temp.transform.Rotate(0, 45, 0);
            }
            else
            {
                temp.transform.Rotate(0, -45, 0);
            }

        }
    }


    private void Update()
    {
  
     
    }

    
}
