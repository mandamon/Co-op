using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    
    // X축 이동(좌우)
    [SerializeField]private float moveXWidth = 1.5f;    // 1회 이동시 이동 거리
    private float moveTimeX = 0.1f;     // 1회 이동에 소요되는 시간
    private bool isXMove = false;       // true : 이동 중, false : 이동 가능

    // Y축 이동(점프)
    [SerializeField]private float originY = 0.55f;      // 점프 및 착지하는 y축 값
    private float gravity = -9.81f;     // 중력
    [SerializeField] private float moveTimeY = 0.3f;     // 1회 이동에 소요되는 시간
    private bool isJump = false;        // true : 점프 중, false : 점프 가능

    // Z축 이동(전진)
    [SerializeField]
    private float moveSpeed = 20.0f;    // 이동 속도

    //슬라이드
    bool isSlide;
    [SerializeField] float slideTime = 1.0f;

    // 회전
    private float rotateSpeed = 300.0f; // 회전 속도

    private float limitY = -1.0f;       // 플레이어가 사망하는 y 위치

    [SerializeField] float basicY =1f;
    [Range(10f,40f)] public float knockBackforce = 20f;
    bool isInvinclble; //무적상태
    [SerializeField] float notDeadTime = 3f;

    bool canMove=true;
    bool isknockBack;

    private Rigidbody rigid;
    private Animator anim;
    private MeshRenderer render;
    private Collider col;

    [SerializeField] GameObject plane;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        render = GetComponent<MeshRenderer>();
        col = GetComponent<Collider>();
    }

    private void Start()
    {
        transform.position = new Vector3(0, basicY, 0);
    }

    private void Update()
    {
        // Z축 이동(전진)
        if (canMove)
        { 
            //for Debug
            if (isSlide)
            {
                transform.position += Vector3.forward * moveSpeed * Time.deltaTime;
            }
            else
            {
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }
        }
            

        // 오브젝트 회전 (X축 좌우)
        //transform.Rotate(Vector3.right * rotateSpeed * Time.deltaTime);

        // 낭떠러지로 떨이지면 플레이어 사망
        if(transform.position.y < limitY)
        {
            InGameManager.instance.GameOver();
            Destroy(gameObject);
        }

        if (isknockBack) //넉백중
        {
            if (rigid.velocity.y==0 && rigid.velocity.z==0) //땅에 닿으면
            {
                Debug.Log("Move now");
                canMove = true;
                isknockBack = false;
                
            }
        }
    }

 
    public void MoveToX(int x)
    {
        

        // 현재 x축 이동 중으로 이동 불가능
        if(isXMove == true || isSlide || !canMove) return;

        if(x > 0 && transform.position.x <= moveXWidth*2)
        {
            StartCoroutine(OnMoveToX(-x)); //우 이동
        }
        else if(x < 0 && transform.position.x >= -moveXWidth*2 )
        {
            StartCoroutine(OnMoveToX(-x)); //좌 이동
        }
    }
    public void MoveToY()
    {
        // 현재 점프 중으로 점프 불가능
        if (isJump == true || isSlide || !canMove) return;
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
        rigid.useGravity = false;

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
        rigid.useGravity = true;
    }


    public void Slide()
    {
        if (isSlide || !canMove || isJump)
            return;

        isSlide = true;
        StartCoroutine(OnSlide());
    }

    IEnumerator OnSlide()
    {
        Debug.Log("slide");
        //anim.SetBool("doSlide",true);
        transform.Rotate(-90, 0, 0);
        yield return new WaitForSeconds(slideTime);
        transform.Rotate(90, 0, 0);
        //anim.SetBool("doSlide", false);
        isSlide = false;
    }

    public void knockBack()
    {
        Debug.Log("KnockBack");
        canMove = false;

        Vector3 direction = transform.forward * -1;
        direction.y = 0.5f;

        rigid.AddForce(direction * knockBackforce, ForceMode.Impulse);

        StartCoroutine(onKnockBack());
        StartCoroutine(setKnockBack());

    }
    IEnumerator setKnockBack()
    {
        yield return new WaitForSeconds(0.2f);
        isknockBack = true;

    }
    IEnumerator onKnockBack()
    {
        Color color = new Color(render.material.color.r, render.material.color.g, render.material.color.b,0.5f);
        Color bak = render.material.color;
        render.material.color = color;

        isInvinclble = true;
        yield return new WaitForSeconds(notDeadTime);

        isInvinclble = false;
        render.material.color = bak;
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "plane")
        {
            plane = collision.gameObject;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "obstructor" )
        {
            if(!isInvinclble)
                knockBack();
        }else if (other.gameObject.tag == "Eater")
        {
            InGameManager.instance.GameOver();
            Destroy(gameObject);
        }else if (other.gameObject.tag == "rotator")
        {
            if (plane)
            {
                Map map = plane.gameObject.GetComponent<Map>();
                transform.rotation = plane.transform.rotation;
                //transform.Rotate(0, 90 * map.direction, 0);
               
            }

        }
    }


}
