using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    [Header("Eater")]
    [SerializeField] GameObject Eater;
    [SerializeField] float moveSpeed;
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
    public void GameOver()

    {
        Debug.Log("게임 오버");
        isgameOver = true;
    }
    public void eaterStart()
    {
        Eater.SetActive(true);
        eaterMoveStart = true;
    }
    private void Update()
    {
        if (eaterMoveStart)
        {
            Eater.transform.position += Vector3.forward * moveSpeed * Time.deltaTime;
        }
    }
}
