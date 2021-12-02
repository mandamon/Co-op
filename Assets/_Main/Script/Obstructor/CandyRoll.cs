using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyRoll : MonoBehaviour
{
    [SerializeField]
    private float ObjSpeed;
    [SerializeField]
    private float objDisappear = -5f; //SummonObj 사라지는 y 위치

    bool canMove = true;

    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (canMove)
        {
            transform.position += transform.forward * ObjSpeed * Time.deltaTime;
        }
        if (transform.position.y < objDisappear)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}