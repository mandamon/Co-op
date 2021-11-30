using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eater : MonoBehaviour
{

    public float spawnTIme=10f;
    public float rayMaxDistance = 3f;
    public float smooth = 10f;
    RaycastHit hit;

    [SerializeField] GameObject posses;

    public GameObject plane;
    public Vector3 atePlanepos;
    public Quaternion atePlanerot;

    bool isRotating;

    bool canMove=true;
    public float moveSpeed=5f;

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * rayMaxDistance, Color.black, 0.3f);
        if(Physics.Raycast(transform.position,transform.forward,out hit, rayMaxDistance)){
            if (hit.collider.gameObject.tag == "plane")
            {
                plane = hit.collider.gameObject;

                atePlanepos = plane.transform.position;
                atePlanerot = plane.transform.rotation;
            }
        }

        if (canMove)
        {
            /*transform.position = Vector3.Lerp(transform.position, atePlanepos, smooth * Time.deltaTime);*/
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }

        posses.transform.position = transform.position;
        posses.transform.rotation = transform.rotation;

    }

    public IEnumerator InterpolateRotate(Transform obj, Quaternion destination, float overTime)
    {
        canMove = false;

        Quaternion source = new Quaternion(obj.rotation.x, obj.rotation.y, obj.rotation.z, obj.rotation.w); ;//deep copy
        float startTime = Time.time;
        while (Time.time < startTime + overTime && obj != null)
        {
            obj.rotation = Quaternion.Lerp(source, destination, (Time.time - startTime) / overTime);
            yield return null;
        }
        obj.rotation = destination;

        canMove = true;

     
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "plane")
        {
            Destroy(other.gameObject.transform.parent.gameObject);
        }
        else if (other.gameObject.tag == "rotator")
           
        {
                StartCoroutine(InterpolateRotate(transform, atePlanerot, 0.5f));
                //Debug.Log(plane.GetComponent<Map>().direction);
             
            
            /*  if (!isRotating)
              {
                  isRotating = true;
                  Destroy(other.gameObject);
              }*/

        }


    }
}
