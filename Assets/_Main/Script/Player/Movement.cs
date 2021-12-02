using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    
    // X축 이동(좌우)
    [SerializeField]private float moveXWidth = 1.5f;    // 1회 이동시 이동 거리
    private float moveTimeX = 0.1f;     // 1회 이동에 소요되는 시간
    //[SerializeField] float moveSmooth = 1f;
    private bool isXMove = false;       // true : 이동 중, false : 이동 가능
    public int rightmoveCnt = 0;
    public int leftmoveCnt = 0;
  

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


    //추락
    private float limitY = -1.0f;       // 플레이어가 사망하는 y 위치
    
    //넉백
    [SerializeField] float basicY =1f;
    [Range(10f,40f)] public float knockBackforce = 20f;
    bool isInvinclble; //무적상태
    [SerializeField] float notDeadTime = 3f;

    [SerializeField]  bool canMove=true;
    bool isknockBack;

    private Rigidbody rigid;
    private Animator anim;
    private MeshRenderer render;
    private Collider col;

    //plane값 저장
    [SerializeField] GameObject plane;
    [SerializeField] Vector3 rotPos;
  

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
        
        // (전진)
        if (canMove)
        { 
            //for Debug

                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            
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

    public Vector3 setExactPos(Vector3 pos)
    {
        return new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
    }
    public void MoveToX(int x)
    {

        Debug.Log(x);
        // 현재 x축 이동 중으로 이동 불가능
        if(isXMove == true || isSlide || !canMove) return;
     
        if(x>0 && rightmoveCnt<2)
        {
            rightmoveCnt++;
            leftmoveCnt--;
            MoveX(-x);
            Debug.Log("plus");
        }

        if (x<0 && leftmoveCnt<2)
        {
            leftmoveCnt++;
            rightmoveCnt--;
            MoveX(-x);
            Debug.Log("minus");
        }
    }
    public void MoveToY()
    {
        // 현재 점프 중으로 점프 불가능
        if (isJump == true || isSlide || !canMove) return;
        StartCoroutine(OnMoveToY());
        
    }

    public void MoveX(int direction)
    {
     
        Vector3 targetpos = transform.position + (direction) * transform.right * moveXWidth;
        StartCoroutine(Interpolate(transform, targetpos, moveTimeX));
    }
/*    public void MoveY()
    {
        Vector3 targetpos = transform.position + new Vector3(0,originY,0);
        StartCoroutine(Interpolate(transform, targetpos, moveTimeY));
    }*/

    public IEnumerator Interpolate(Transform obj, Vector3 destination, float overTime)
    {
        Vector3 source = new Vector3(obj.position.x, obj.position.y, obj.position.z);//deep copy
        float startTime = Time.time;
        while (Time.time < startTime + overTime && obj != null)
        {
            obj.position = Vector3.Lerp(source, destination, (Time.time - startTime) / overTime);
            yield return null;
        }
        obj.position = destination;
    }

    public IEnumerator InterpolateRotate(Transform obj, Quaternion destination, float overTime)
    {
        canMove = false;
        Quaternion source = new Quaternion(obj.rotation.x, obj.rotation.y, obj.rotation.z, obj.rotation.w); ;//deep copy
        float startTime = Time.time;
        while (Time.time < startTime + overTime && obj != null)
        {
            obj.rotation = Quaternion.Lerp(source, destination, (Time.time - startTime) / overTime);
            yield return null;
        }
        obj.rotation = destination;

        transform.position = setExactPos(transform.position);

        canMove = true;
       

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
        anim.SetBool("doSlide",true);
        //transform.Rotate(-90, 0, 0);
        yield return new WaitForSeconds(slideTime);
        //transform.Rotate(90, 0, 0);
        anim.SetBool("doSlide", false);
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
    public void knockDown()
    {
        Debug.Log("KnockDown");
        canMove = false;

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
        else if (collision.gameObject.tag == "summonObj")
        {
            if (!isInvinclble)
                knockDown();
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
            Debug.Log("Rotating");
            if (plane)
            {
                StartCoroutine(InterpolateRotate(transform, plane.transform.rotation, 0.5f));
            }
           
        }

    }




}
