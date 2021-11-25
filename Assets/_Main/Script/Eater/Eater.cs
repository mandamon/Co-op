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
    bool isinRotator;
    bool canMove=true;

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
            transform.position = Vector3.Lerp(transform.position, atePlanepos, smooth * Time.deltaTime);
        }

        if (isRotating)
        {
            canMove = false;
            transform.rotation = Quaternion.Lerp(transform.rotation, atePlanerot, Time.deltaTime * 20f);
            if (transform.rotation == atePlanerot)
            {
                isRotating = false;
                canMove = true;
            }
            
        }

        posses.transform.position = transform.position;
        posses.transform.rotation = transform.rotation;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "plane")
        {
            
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "rotator")
           
        {
            if (!isRotating)
            {
                isRotating = true;
                Destroy(other.gameObject);
            }

        }


    }
}
