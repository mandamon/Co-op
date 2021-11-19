using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // X�� �̵�(�¿�)
    private float moveXWidth = 1.5f;    // 1ȸ �̵��� �̵� �Ÿ�
    private float moveTimeX = 0.1f;     // 1ȸ �̵��� �ҿ�Ǵ� �ð�
    private bool isXMove = false;       // true : �̵� ��, false : �̵� ����

    // Y�� �̵�(����)
    private float originY = 0.55f;      // ���� �� �����ϴ� y�� ��
    private float gravity = -9.81f;     // �߷�
    private float moveTimeY = 0.3f;     // 1ȸ �̵��� �ҿ�Ǵ� �ð�
    private bool isJump = false;        // true : ���� ��, false : ���� ����

    // Z�� �̵�(����)
    [SerializeField]
    private float moveSpeed = 20.0f;    // �̵� �ӵ�

    // ȸ��
    private float rotateSpeed = 300.0f; // ȸ�� �ӵ�

    private float limitY = -1.0f;       // �÷��̾ ����ϴ� y ��ġ

    private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        // Z�� �̵�(����)
        transform.position += Vector3.forward * moveSpeed * Time.deltaTime;

        // ������Ʈ ȸ�� (X�� �¿�)
        transform.Rotate(Vector3.right * rotateSpeed * Time.deltaTime);

        // ���������� �������� �÷��̾� ���
        if(transform.position.y < limitY)
        {
            Debug.Log("���� ����");
        }
    }
    public void MoveToX(int x)
    {
        // ���� x�� �̵� ������ �̵� �Ұ���
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
        // ���� ���� ������ ���� �Ұ���
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
        float v0 = -gravity; // y ������ �ʱ� �ӵ�

        isJump = true;
        rigidbody.useGravity = false;

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
        rigidbody.useGravity = true;
    }
}
