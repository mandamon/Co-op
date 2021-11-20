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
    private void Update()
    {
        if(playerTransform.position.z - transform.position.z >= destroyDistance)
        {
            // 货肺款 甘阑 积己
            mapSpawner.SpawnMap();
            // 泅犁 备开篮 昏力
            Destroy(gameObject);
        }
    }
}
