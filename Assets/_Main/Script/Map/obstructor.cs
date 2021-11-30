using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstructor : MonoBehaviour
{

    [SerializeField] public bool move;

    [SerializeField] float minMove;


    public float moveSpeed = 5f;

    Vector3 InitPos;

    int sign = -1;

    private void Start()
    {
        InitPos = transform.position;
    }

    private void Update()
    {
        if (move)
        {
            transform.position += transform.right * moveSpeed * Time.deltaTime * sign;

            if (Vector3.Distance(InitPos, transform.position)>=minMove)
            {
                sign *= -1;
            }

        }
    }
}
