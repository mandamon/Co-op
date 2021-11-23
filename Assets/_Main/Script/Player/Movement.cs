using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    
    // X�� �̵�(�¿�)
    [SerializeField]private float moveXWidth = 1.5f;    // 1ȸ �̵��� �̵� �Ÿ�
    private float moveTimeX = 0.1f;     // 1ȸ �̵��� �ҿ�Ǵ� �ð�
    private bool isXMove = false;       // true : �̵� ��, false : �̵� ����

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
        // Z�� �̵�(����)
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
    }

 
    public void MoveToX(int x)
    {
        

        // ���� x�� �̵� ������ �̵� �Ұ���
        if(isXMove == true || isSlide || !canMove) return;

        if(x > 0 && transform.position.x <= moveXWidth*2)
        {
            StartCoroutine(OnMoveToX(-x)); //�� �̵�
        }
        else if(x < 0 && transform.position.x >= -moveXWidth*2 )
        {
            StartCoroutine(OnMoveToX(-x)); //�� �̵�
        }
    }
    public void MoveToY()
    {
        // ���� ���� ������ ���� �Ұ���
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
