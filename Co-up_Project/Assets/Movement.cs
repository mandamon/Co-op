using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // X축 이동(좌우)
    private float moveXWidth = 1.5f;    // 1회 이동시 이동 거리
    private float moveTimeX = 0.1f;     // 1회 이동에 소요되는 시간
    private bool isXMove = false;       // true : 이동 중, false : 이동 가능

    // Y축 이동(점프)
    private float originY = 0.55f;      // 점프 및 착지하는 y축 값
    private float gravity = -9.81f;     // 중력
    private float moveTimeY = 0.3f;     // 1회 이동에 소요되는 시간
    private bool isJump = false;        // true : 점프 중, false : 점프 가능

    // Z축 이동(전진)
    [SerializeField]
    private float moveSpeed = 20.0f;    // 이동 속도

    // 회전
    private float rotateSpeed = 300.0f; // 회전 속도

    private float limitY = -1.0f;       // 플레이어가 사망하는 y 위치

    private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        // Z축 이동(전진)
        transform.position += Vector3.forward * moveSpeed * Time.deltaTime;

        // 오브젝트 회전 (X축 좌우)
        transform.Rotate(Vector3.right * rotateSpeed * Time.deltaTime);

        // 낭떠러지로 떨이지면 플레이어 사망
        if(transform.position.y < limitY)
        {
            Debug.Log("게임 오버");
        }
    }
    public void MoveToX(int x)
    {
        // 현재 x축 이동 중으로 이동 불가능
        if(isXMove == true) return;
        if(x > 0 && transform.position.x < moveXWidth)
        {
            StartCoroutine(OnMoveToX(x));
        }
        else if(x < 0 && transform.position.x > -moveXWidth)
        {
            StartCoroutine(OnMoveToX(x));
        }
    }
    public void MoveToY()
    {
        // 현재 점프 중으로 점프 불가능
        if (isJump == true) return;
        StartCoroutine(OnMoveToY());
    }
    private IEnumerator OnMoveToX(int direction)
    {
        float current = 0;
        float percent = 0;
        float start = transform.position.x;
        float end = transform.position.x + direction * moveXWidth;

        isXMove = true;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / moveTimeX;

            float x = Mathf.Lerp(start, end, percent);
            transform.position = new Vector3(x, transform.position.y, transform.position.z);

            yield return null;
        }

        isXMove = false;
    }
    private IEnumerator OnMoveToY()
    {
        float current = 0;
        float percent = 0;
        float v0 = -gravity; // y 방향의 초기 속도

        isJump = true;
        rigidbody.useGravity = false;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / moveTimeY;

            // 시간 경과에 따라 오브젝트의 y 위치를 바꿔준다
            // 포물선 운동 : 시작위치 + 초기속도 * 시간 + 중력 * 시간제곱
            float y = originY + (v0 * percent) + (gravity * percent * percent);
            transform.position = new Vector3(transform.position.x, y, transform.position.z);

            yield return null;
        }

        isJump = false;
        rigidbody.useGravity = true;
    }
}
