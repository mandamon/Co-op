using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstructorSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] obstructor;
    [SerializeField] Transform[] poses;

    [SerializeField ]float[] randomz;
    [SerializeField] float[] randomy;

    [SerializeField] int percent;

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
        int ranyidx = Random.Range(0, randomy.Length);

        obstructorObj = Instantiate(obstructor[ranidx], poses[ranPosidx]);
        //·£´ýÇÑ È®·ü·Î ¿òÁ÷ÀÎ´Ù
        if (ranPosidx == 1 || ranPosidx==2 || ranPosidx == 3)
        {
            int temp_per = Random.Range(0, 100);
            if (temp_per <= percent)
            {
                obstructorObj.GetComponent<obstructor>().move = true;
            }
            
        }
       
        obstructorObj.transform.localPosition= new Vector3(0, randomy[ranyidx], randomz[ranzidx]);
        //obstructorObj.transform.parent = poses[ranPosidx].transform.parent;

    }
}
