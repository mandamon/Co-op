using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonObj : MonoBehaviour
{
    [SerializeField]
    private float summonObjSpeed;
    [SerializeField]
    private float objDisappear = -5f; //SummonObj ������� y ��ġ
    [SerializeField]
    private float rotatespeed = 0.5f;

    GameObject plane;
    bool isRotating;
    bool isinRotator;
    void Update()
    {
        transform.position += transform.forward * summonObjSpeed * Time.deltaTime;
        transform.Rotate(0, 0, rotatespeed, Space.Self);
        if(transform.position.y < objDisappear)
        {
            Destroy(gameObject);
        }
    }
    public IEnumerator InterpolateRotate(Transform obj, Quaternion destination, float overTime)
    {
        Quaternion source = new Quaternion(obj.rotation.x, obj.rotation.y, obj.rotation.z, obj.rotation.w);
        float startTime = Time.time;
        while (Time.time < startTime + overTime && obj != null)
        {
            obj.rotation = Quaternion.Lerp(source, destination, (Time.time - startTime) / overTime);
            yield return null;
        }
        obj.rotation = destination;
        isinRotator = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "plane")
        {
            plane = collision.gameObject;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "rotator")
        {
            if(plane && !isinRotator)
            {
                StartCoroutine(InterpolateRotate(transform, plane.transform.rotation, 0.5f));
                Debug.Log(plane.GetComponent<Map>().direction);
                isRotating = true;
                isinRotator = true;
            }
        }
    }
}
