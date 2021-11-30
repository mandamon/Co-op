using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRotator : MonoBehaviour
{
    // Start is called before the first frame update
    public float x = 0;
    public float y = 0;
    public float z = 0;



    // Update is called once per frame
    void Update()
    {
        Quaternion localRottation = Quaternion.Euler(x, y, z);
        transform.rotation = transform.rotation * localRottation;

    }
}
