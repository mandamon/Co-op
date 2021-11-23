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
        // 코인 오브젝트 회전
        transform.Rotate(Vector3.right * rotateSpeed * Time.deltaTime);
    }
<<<<<<< HEAD
=======
    
>>>>>>> 08003a4eed44169f8dcf7cb8d2512d224058ce70
    private void OnTriggerEnter(Collider other)
    {
        // 코인 획득 효과 생성
        //GameObject clone = Instantiate(coinEffectPrefab);
        //clone.transform.position = transform.position;
<<<<<<< HEAD
        // 코인 오브젝트 삭제
        Destroy(gameObject);
=======
        // 코인 계산
        if (other.gameObject.tag == "Player")
        {
            InGameManager.instance.coinCount++;
            // 코인 오브젝트 삭제
            Destroy(gameObject);
        }
      
>>>>>>> 08003a4eed44169f8dcf7cb8d2512d224058ce70
    }
}
