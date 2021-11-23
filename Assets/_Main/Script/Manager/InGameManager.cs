using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    [Header("GameConfig")]
    [SerializeField] GameObject outCam;
    [SerializeField] GameObject inCam;

    [Header("Eater")]
    [SerializeField] GameObject Eater;
    [SerializeField] float moveSpeed;
    [SerializeField] float eaterStartTime = 5f;
  


    bool eaterMoveStart;

    public bool isgameOver;

    public static InGameManager instance;

    // 코인
    public int coinCount;
    
    private void Awake()
    {
        instance = this;
        coinCount = 0;
    }

    private void Start()
    {
        StartCoroutine(OnStartEater());
    }

    IEnumerator OnStartEater()
    {
        yield return new WaitForSeconds(eaterStartTime);
        Eater.SetActive(true);
    }
    public void GameOver()

    {
        Debug.Log("게임 오버");
        isgameOver = true;

       /* outCam.transform.position = inCam.transform.position;

        inCam.SetActive(false);
        outCam.SetActive(true);*/
    }

}
