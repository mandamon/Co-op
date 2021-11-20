using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] mapPrefabs;       // �� ������ �迭
    [SerializeField]
    private int spawnMapCountAtStart = 3;  // ���� ���۽� ���� �����Ǵ� �� ����
    [SerializeField]
    private float zDistance = 100;           // �� ������ �Ÿ�(z)
    private int mapIndex = 0;              // �� �ε��� (��ġ�Ǵ� ���� z ��ġ ���꿡 �̿�)
    private void Awake()
    {
        //spawnMapCountAtStart�� ����� ������ŭ �ּ� �� ����
        for(int i = 0; i < spawnMapCountAtStart; ++i)
        {
            // ù ��° ���� �׻� 0�� �� ���������� ����
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
        // ���� ��ġ�Ǵ� ��ġ ���� (z���� ���� �� �ε��� * zDistance)
        clone.transform.position = new Vector3(0, 0, mapIndex * zDistance);
        mapIndex++;
    }
}
