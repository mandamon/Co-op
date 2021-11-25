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
        //spawnMapCountAtStart�� ����� ������ŭ �ּ� �� ����
        for (int i = 0; i < spawnMapCountAtStart; ++i)
        {
            // ù ��° ���� �׻� 2�� �� ���������� ����
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
        // ���� ��ġ�Ǵ� ��ġ ���� (z���� ���� �� �ε��� * zDistance)
        
        // ���� ������ �� ���ο� ���� ������ �� �ֵ��� this�� �÷��̾��� Transform ���� ����
        //clone.GetComponent<Map>().Setup(this, playerTransform);
        //mapIndex++;
        //Destroy(clone, destroyTime);
        prev_clone = clone;
        prev_direction = clone.GetComponentInChildren<Map>().direction;
    }


}
