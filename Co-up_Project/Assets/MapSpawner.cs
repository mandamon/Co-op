using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] mapPrefabs;       // 맵 프리팹 배열
    [SerializeField]
    private int spawnMapCountAtStart = 3;  // 게임 시작시 최초 생성되는 맵 개수
    [SerializeField]
    private float zDistance = 100;           // 맵 사이의 거리(z)
    private int mapIndex = 0;              // 맵 인덱스 (배치되는 맵의 z 위치 연산에 이용)
    private void Awake()
    {
        //spawnMapCountAtStart에 저장된 개수만큼 최소 맵 생성
        for(int i = 0; i < spawnMapCountAtStart; ++i)
        {
            // 첫 번째 맵은 항상 0번 맵 프리팹으로 설정
            if(i == 0)
            {
                SpawnMap(false);
            }
            else
            {
                SpawnMap();
            }
        }
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
            //int index = Random.Range(0, mapPrefabs.Length);
            clone = Instantiate(mapPrefabs[0]);// index]);
        }
        // 맵이 배치되는 위치 설정 (z축은 현재 맵 인데스 * zDistance)
        clone.transform.position = new Vector3(0, 0, mapIndex * zDistance);
        mapIndex++;
    }
}
