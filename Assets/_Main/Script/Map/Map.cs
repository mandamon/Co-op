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
        //transform.position += Vector3.forward *5.0f * Time.deltaTime;

        /*if (playerTransform.position.z - transform.position.z >= destroyDistance) //멀어지면 삭제
        {
            // 새로운 맵을 생성
            mapSpawner.SpawnMap();
            // 현재 구역은 삭제
            
            Destroy(gameObject);
        }*/


     
    }

    
}
