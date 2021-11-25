using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    
    // X�� �̵�(�¿�)
    [SerializeField]private float moveXWidth = 1.5f;    // 1ȸ �̵��� �̵� �Ÿ�
    private float moveTimeX = 0.1f;     // 1ȸ �̵��� �ҿ�Ǵ� �ð�
    //[SerializeField] float moveSmooth = 1f;
    private bool isXMove = false;       // true : �̵� ��, false : �̵� ����
    public int rightmoveCnt = 0;
    public int leftmoveCnt = 0;
  

    // Y�� �̵�(����)
    [SerializeField]private float originY = 0.55f;      // ���� �� �����ϴ� y�� ��
    private float gravity = -9.81f;     // �߷�
    [SerializeField] private float moveTimeY = 0.3f;     // 1ȸ �̵��� �ҿ�Ǵ� �ð�
    private bool isJump = false;        // true : ���� ��, false : ���� ����

    // Z�� �̵�(����)
    [SerializeField]
    private float moveSpeed = 20.0f;    // �̵� �ӵ�

    //�����̵�
    bool isSlide;
    [SerializeField] float slideTime = 1.0f;

    // ȸ��
    private float rotateSpeed = 300.0f; // ȸ�� �ӵ�

    private float limitY = -1.0f;       // �÷��̾ ����ϴ� y ��ġ

    [SerializeField] float basicY =1f;
    [Range(10f,40f)] public float knockBackforce = 20f;
    bool isInvinclble; //��������
    [SerializeField] float notDeadTime = 3f;

    [SerializeField]  bool canMove=true;
    bool isknockBack;

    private Rigidbody rigid;
    private Animator anim;
    private MeshRenderer render;
    private Collider col;

    [SerializeField] GameObject plane;
    bool isRotating;

    bool isinRotator;

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
        
        // Z�� �̵�(����)
        if (canMove)
        { 
            //for Debug

                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            
        }
            

        // ������Ʈ ȸ�� (X�� �¿�)
        //transform.Rotate(Vector3.right * rotateSpeed * Time.deltaTime);

        // ���������� �������� �÷��̾� ���
        if(transform.position.y < limitY)
        {
            InGameManager.instance.GameOver();
            Destroy(gameObject);
        }

        if (isknockBack) //�˹���
        {
            if (rigid.velocity.y==0 && rigid.velocity.z==0) //���� ������
            {
                Debug.Log("Move now");
                canMove = true;
                isknockBack = false;
                
            }
        }

        if (isRotating)
        {
            canMove = false;
            transform.rotation = Quaternion.Lerp(transform.rotation, plane.transform.rotation, Time.deltaTime * 10f);

            if (transform.rotation == plane.transform.rotation)
            {
                isRotating = false;
                canMove = true;
            }
        }



    }

 
    public void MoveToX(int x)
    {

        Debug.Log(x);
        // ���� x�� �̵� ������ �̵� �Ұ���
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
        // ���� ���� ������ ���� �Ұ���
        if (isJump == true || isSlide || !canMove) return;
        StartCoroutine(OnMoveToY());
    }

    public void MoveX(int direction)
    {
        Vector3 targetpos = transform.position + (direction) * transform.right * moveXWidth;
        StartCoroutine(Interpolate(transform, targetpos, moveTimeX));
    }

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
    /*    private IEnumerator OnMoveToX(int direction)
        {

            float current = 0;
            float percent = 0;
            float start = transform.position.x;
            float end = transform.position.x + direction * moveXWidth;
            isXMove = true;

            while (percent < 1)
            {
                current += Time.deltaTime;
                percent = current / moveTimeX;


                float x = Mathf.Lerp(start, end, percent);
                transform.position = new Vector3(x, transform.position.y, transform.position.z);

                yield return null;
            }


    *//*        Vector3 targetpos = transform.position + (direction) * transform.right * moveXWidth;
            while (transform.position != targetpos)
            {
                transform.position = Vector3.Lerp(transform.position, targetpos, moveSmooth * Time.deltaTime);
            }
            //transform.position = transform.position +(direction)*transform.right * moveXWidth;*//*

            isXMove = false;

        }*/

    private IEnumerator OnMoveToY()
    {
        float current = 0;
        float percent = 0;
        float v0 = -gravity; // y ������ �ʱ� �ӵ�

        isJump = true;
        rigid.useGravity = false;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / moveTimeY;

            // �ð� ����� ���� ������Ʈ�� y ��ġ�� �ٲ��ش�
            // ������ � : ������ġ + �ʱ�ӵ� * �ð� + �߷� * �ð�����
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
        }
        else if (other.gameObject.tag == "rotator")
        {
            if (plane && !isinRotator)
            {
                isRotating = true;
                isinRotator = true;
            }

        }
    }

    private void OnTriggerStay(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "rotator")
        {
            isinRotator = false;
        }
    }


}
