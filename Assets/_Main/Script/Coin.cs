using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    private GameObject coinEffectPrefab;
    private float rotateSpeed;
    private void Awake()
    {
        rotateSpeed = Random.Range(0, 360);
    }
    private void Update()
    {
        // ���� ������Ʈ ȸ��
        transform.Rotate(Vector3.right * rotateSpeed * Time.deltaTime);
    }
<<<<<<< HEAD
=======
    
>>>>>>> 08003a4eed44169f8dcf7cb8d2512d224058ce70
    private void OnTriggerEnter(Collider other)
    {
        // ���� ȹ�� ȿ�� ����
        //GameObject clone = Instantiate(coinEffectPrefab);
        //clone.transform.position = transform.position;
<<<<<<< HEAD
        // ���� ������Ʈ ����
        Destroy(gameObject);
=======
        // ���� ���
        if (other.gameObject.tag == "Player")
        {
            InGameManager.instance.coinCount++;
            // ���� ������Ʈ ����
            Destroy(gameObject);
        }
      
>>>>>>> 08003a4eed44169f8dcf7cb8d2512d224058ce70
    }
}
