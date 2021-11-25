using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonObj : MonoBehaviour
{
    [SerializeField]
    private float summonObjSpeed;
    [SerializeField]
    private float objDisappear = -5f; //SummonObj ������� y ��ġ
    void Update()
    {
        transform.position += transform.forward * summonObjSpeed * Time.deltaTime;
        if(transform.position.y < objDisappear)
        {
            Destroy(gameObject);
        }
    }
}
