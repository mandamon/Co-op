using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField]
    private float destroyDistance = 15;
    private MapSpawner mapSpawner;
    private Transform playerTransform;

  


    public void Setup(MapSpawner mapSpawner, Transform playerTransform)
    {
        this.mapSpawner = mapSpawner;
        this.playerTransform = playerTransform;
    }

    private void Start()
    {
        
    }


    private void Update()
    {
        //transform.position += Vector3.forward *5.0f * Time.deltaTime;

        /*if (playerTransform.position.z - transform.position.z >= destroyDistance) //�־����� ����
        {
            // ���ο� ���� ����
            mapSpawner.SpawnMap();
            // ���� ������ ����
            
            Destroy(gameObject);
        }*/
    }

    
}
