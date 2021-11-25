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

    int prev_turn = -1;

    [SerializeField] float spawnTime = 1f;
    bool canSpawn=true;

    float flowTIme;
    float prev_direction;

    bool spawnTurn;

    [SerializeField] float turnSpawnTime = 5f;

    private void Awake()
    {
        canSpawn = false;
        //spawnMapCountAtStart에 저장된 개수만큼 최소 맵 생성
        for (int i = 0; i < spawnMapCountAtStart; ++i)
        {
            // 첫 번째 맵은 항상 2번 맵 프리팹으로 설정
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

    private void Start()
    {
        StartCoroutine(onSpawnTurn());
    }

    IEnumerator onSpawnTurn()
    {
        while (true)
        {
            yield return new WaitForSeconds(turnSpawnTime);
            spawnTurn = true;
        }
        
    }

    private void Update()
    {
        if (canSpawn && !InGameManager.instance.isgameOver)
            StartCoroutine(onSpawnMap());
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
            clone = Instantiate(mapPrefabs[2]);
        }
        else
        {

            if (spawnTurn)
            {
                int index = Random.Range(0,2);
                if (prev_turn == 0)
                {
                    index = 1;
                }else if (prev_turn == 1)
                {
                    index = 0;
                }
                prev_turn = index;
                clone = Instantiate(mapPrefabs[index]);
                
                spawnTurn = false;
            }
            else
            {
                int index = Random.Range(2, mapPrefabs.Length);
                clone = Instantiate(mapPrefabs[index]);
            }
            
        }

        if (prev_clone)
        {
            
            
           if (prev_direction == 1)
            {
                prev_clone.transform.rotation *= Quaternion.AngleAxis(90f, Vector3.up);

                //clone.transform.Rotate(0, 90, 0);
            }
            else if(prev_direction == -1)
            {
                prev_clone.transform.rotation *= Quaternion.AngleAxis(-90f, Vector3.up);
            }
            clone.transform.rotation = prev_clone.transform.rotation;
            clone.transform.position = prev_clone.transform.position + prev_clone.transform.forward * zDistance;
        }
        else
        {
            clone.transform.position = new Vector3(0, 0, 0);
        }
        // 맵이 배치되는 위치 설정 (z축은 현재 맵 인데스 * zDistance)
        
        // 맵이 삭제될 때 새로운 맵을 생성할 수 있도록 this와 플레이어의 Transform 정보 전달
        //clone.GetComponent<Map>().Setup(this, playerTransform);
        //mapIndex++;
        //Destroy(clone, destroyTime);
        prev_clone = clone;
        prev_direction = clone.GetComponentInChildren<Map>().direction;
    }


}
