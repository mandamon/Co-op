using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] mapPrefabs;        // �� ������ �迭
    [SerializeField]
    private int spawnMapCountAtStart = 3;   // ���� ���۽� ���� �����Ǵ� �� ����
    [SerializeField]
    private float zDistance = 100;          // �� ������ �Ÿ�(z)
    //[SerializeField] public int mapIndex = 0;               // �� �ε��� (��ġ�Ǵ� ���� z ��ġ ���꿡 �̿�)
    [SerializeField]
    private Transform playerTransform;      // �÷��̾� transform

    GameObject prev_clone;

    [SerializeField] float spawnTime = 1f;
    [SerializeField] float destroyTime = 3f;
    bool canSpawn=true;

    float flowTIme;

    private void Awake()
    {
        canSpawn = false;
        //spawnMapCountAtStart�� ����� ������ŭ �ּ� �� ����
        for (int i = 0; i < spawnMapCountAtStart; ++i)
        {
            // ù ��° ���� �׻� 0�� �� ���������� ����
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
        // ���� ��ġ�Ǵ� ��ġ ���� (z���� ���� �� �ε��� * zDistance)
        
        // ���� ������ �� ���ο� ���� ������ �� �ֵ��� this�� �÷��̾��� Transform ���� ����
        clone.GetComponent<Map>().Setup(this, playerTransform);
        //mapIndex++;
        Destroy(clone, destroyTime);
        prev_clone = clone;
    }


}
