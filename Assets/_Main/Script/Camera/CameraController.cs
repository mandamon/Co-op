using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform target;   // 카메라가 추적하는 대상

    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    private void Awake()
    {
       
    }
    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPos = target.position;
            targetPos.y = 0;
            Vector3 desiredPos =targetPos+ (target.forward * offset.z)+new Vector3(0,offset.y,0);
            Vector3 smoothPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
            transform.position = smoothPos;


            transform.rotation = target.rotation;
            transform.Rotate(18, 180, 0);
        }
    }
}
