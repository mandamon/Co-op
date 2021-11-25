using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstructorSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] obstructor;
    [SerializeField] Transform[] poses;

    [SerializeField ]float[] randomz;

    private GameObject obstructorObj;

    private void Start()
    {
        spawnObstructor();
    }

    private void spawnObstructor()
    {
        int ranidx = Random.Range(0, obstructor.Length);
        int ranPosidx = Random.Range(0, poses.Length);
        int ranzidx=Random.Range(0, randomz.Length);



        obstructorObj=Instantiate(obstructor[ranidx],poses[ranPosidx]);
        obstructorObj.transform.localPosition= new Vector3(0, 0, randomz[ranzidx]);
        //obstructorObj.transform.parent = poses[ranPosidx].transform.parent;

    }
}
