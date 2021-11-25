using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour
{
    [SerializeField]
    private GameObject obstacle;
    [SerializeField]
    private Transform summonPos;
    private bool summonTurn;
    [SerializeField]
    private float summonTime = 3f;

    private void Update()
    {
        if(!InGameManager.instance.isgameOver)
            StartCoroutine(onSummon()); 
    }

    IEnumerator onSummon()
    {
        summonTurn = true;
        if(summonTurn)
            summon();
        yield return new WaitForSeconds(summonTime);
    }
    private void summon()
    {
        // º“»Ø
        GameObject summonObstacle = Instantiate(obstacle, summonPos.position, summonPos.rotation);
        Rigidbody summonRigid = summonObstacle.GetComponent<Rigidbody>();
        summonRigid.velocity = summonPos.forward * 10;
        summonTurn = false;
    }
}
