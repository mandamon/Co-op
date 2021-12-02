using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstructorSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] obstructor;
    [SerializeField] Transform[] poses;

    [SerializeField]float[] randomz;
    [SerializeField] float[] randomy;

    [SerializeField] int obstructor_percent;

    private GameObject obstructorObj;

    [SerializeField] GameObject candyroll;
    [SerializeField] int candyroll_percent;

    private void Start()
    {
        spawnObstructor();
        spawnCandyRoll();
    }

    private void spawnObstructor()
    {
        int ranidx = Random.Range(0, obstructor.Length);
        int ranPosidx = Random.Range(0, poses.Length);
        int ranzidx=Random.Range(0, randomz.Length);
        int ranyidx=-1;

        if (ranidx == 0) //지상형 장애물
        {
            ranyidx = Random.Range(0, randomy.Length - 1);

            obstructorObj = Instantiate(obstructor[ranidx], poses[ranPosidx]);
            //랜덤한 확률로 움직인다
            if (ranPosidx == 1 || ranPosidx == 2 || ranPosidx == 3)
            {
                int temp_per = Random.Range(0, 100);
                if (temp_per <= obstructor_percent)
                {
                    obstructorObj.GetComponent<obstructor>().move = true;
                }

            }
        }
        else //공중형 장애물
        {

        }


        obstructorObj.transform.localPosition = new Vector3(0, randomy[ranyidx], randomz[ranzidx]);
        //obstructorObj.transform.parent = poses[ranPosidx].transform.parent;

    }
    private void spawnCandyRoll()
    {
        int temp_per = Random.Range(0, 100);
        if(temp_per <= candyroll_percent)
        {
            candyrollPos = poses[2].position + new Vector3(0, 0.75f, 0);
            Instantiate(candyroll, candyrollPos);
        }
    }
}
