using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] mapPrefabs;        // 맵 프리팹 배열
    [SerializeField]
    private int spawnMapCountAtStart = 3;   // 게임 시작시 최초 생성되는 맵 개수
    [SerializeField]
    private float zDistance = 100;          // 맵 사이의 거리(z)
    //[SerializeField] public int mapIndex = 0;               // 맵 인덱스 (배치되는 맵의 z 위치 연산에 이용)
    [SerializeField]
    private Transform playerTransform;      // 플레이어 transform

    GameObject prev_clone;

    [SerializeField] float spawnTime = 1f;
    [SerializeField] float destroyTime = 3f;
    bool canSpawn=true;

    float flowTIme;

    private void Awake()
    {
        canSpawn = false;
        //spawnMapCountAtStart에 저장된 개수만큼 최소 맵 생성
        for (int i = 0; i < spawnMapCountAtStart; ++i)
        {
            // 첫 번째 맵은 항상 0번 맵 프리팹으로 설정
            if (i == 0)
            {
                SpawnMap(false);

            }
            else
            {
                SpawnMap();
            }
            
        }
        canSpawn = true;

    }

    private void Update()
    {
        if (canSpawn && !InGameManager.instance.isgameOver)
            StartCoroutine(onSpawnMap());

        flowTIme += Time.deltaTime;
        if (flowTIme >= destroyTime)
        {
            InGameManager.instance.eaterStart();
        }
    }
    IEnumerator onSpawnMap()
    {
        canSpawn = false;
        SpawnMap();
        yield return new WaitForSeconds(spawnTime);
        canSpawn = true;
    }

    public void SpawnMap(bool isRandom = true)
    {
        GameObject clone = null;

        if(isRandom == false)
        {
            clone = Instantiate(mapPrefabs[0]);
        }
        else
        {
            int index = Random.Range(0, mapPrefabs.Length);
            clone = Instantiate(mapPrefabs[index]);// index]);
        }

        if (prev_clone)
        {
            clone.transform.position = new Vector3(0, 0, prev_clone.transform.position.z + zDistance);
        }
        else
        {
            clone.transform.position = new Vector3(0, 0, 0);
        }
        // 맵이 배치되는 위치 설정 (z축은 현재 맵 인데스 * zDistance)
        
        // 맵이 삭제될 때 새로운 맵을 생성할 수 있도록 this와 플레이어의 Transform 정보 전달
        clone.GetComponent<Map>().Setup(this, playerTransform);
        //mapIndex++;
        Destroy(clone, destroyTime);
        prev_clone = clone;
    }


}
