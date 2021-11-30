using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour
{
    [SerializeField]
    private GameObject obstacle;
    [SerializeField]
    private Transform[] summonPos;
    private bool summonTurn = true;
    [SerializeField]
    private float summonTime = 3f;

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
        int ranidx = Random.Range(0, summonPos.Length);
        GameObject summonObstacle = Instantiate(obstacle, summonPos[ranidx].position, summonPos[ranidx].rotation);

        summonTurn = false;
    }
}
