using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour
{
    [SerializeField]
    private GameObject obstacle;
    [SerializeField]
    private Transform summonPos;
    private bool summonTurn = true;
    [SerializeField]
    private float summonTime = 3f;
    [SerializeField]
    private float summonSpeed;

    private void Update()
    {
        if(summonTurn && !InGameManager.instance.isgameOver)
            StartCoroutine(onSummon());
    }

    IEnumerator onSummon()
    {
        summon();
        yield return new WaitForSeconds(summonTime);
        summonTurn = true;
    }
    private void summon()
    {
        // º“»Ø
        GameObject summonObstacle = Instantiate(obstacle, summonPos.position, summonPos.rotation);
        Rigidbody summonRigid = summonObstacle.GetComponent<Rigidbody>();
        summonRigid.velocity = summonPos.forward * summonSpeed;
        summonTurn = false;
    }
}
