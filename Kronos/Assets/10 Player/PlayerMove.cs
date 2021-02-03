using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    private Rigidbody rigidbody;
    float maxSpeed = 5f;
    float speedRate = 1.0f;

    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Debug.Log(gameObject.transform.forward);
    }

    void Update()
    {
        MouseRotation();
        KeyboardMove();
    }

    void MouseRotation()
    {
        float MouseX = Input.GetAxis("Mouse X");

        transform.Rotate(Vector3.up * MouseX);
    }

    void KeyboardMove()
    {
        // ���� �̵��� ���� ����
        float forceForward = 0f;

        if (Input.GetKey(KeyCode.W))
            forceForward = 100000f;
        else if (Input.GetKey(KeyCode.S))
            forceForward = -100000f;

        // �¿� �̵��� ���� ����
        float forceRight = 0f;

        if (Input.GetKey(KeyCode.A))
            forceRight = -100000f;
        else if (Input.GetKey(KeyCode.D))
            forceRight = 100000f;

        // �밢�� ��츦 ����Ͽ�, ��� �������� ������ �ӵ��� ������ �Ѵ�.
        if(forceForward != 0 && forceRight != 0)
        {
            forceForward = forceForward / Mathf.Sqrt(2);
            forceRight = forceRight / Mathf.Sqrt(2);
        }

        // �ڷ� ���� �� �̵��ӵ� 25% ����
        if(forceForward < 0)
        {
            forceForward *= 0.75f;
        }


        // ������ ����
        Vector3 playerForward = gameObject.transform.forward;
        playerForward.y = 0;
        playerForward = playerForward.normalized;

        Vector3 playerRight = gameObject.transform.right;
        playerRight.y = 0;
        playerRight = playerRight.normalized;

        rigidbody.AddForce(playerForward * speedRate * forceForward * Time.deltaTime);
        rigidbody.AddForce(playerRight * speedRate * forceRight * Time.deltaTime);

        /*
        // �Է� ������ �ӵ� ���� (�Է� �ִٸ� �þ�״ϱ� ���� �ȵɵ�)
        rigidbody.velocity = new Vector3(rigidbody.velocity.x * 0.98f,
            rigidbody.velocity.y,
            rigidbody.velocity.z * 0.98f);
        */

        // �ִ�ӵ� ����
        if (rigidbody.velocity.x > maxSpeed * speedRate)
            rigidbody.velocity = new Vector3(maxSpeed * speedRate, rigidbody.velocity.y, rigidbody.velocity.z);

        else if (rigidbody.velocity.x < -maxSpeed * speedRate)
            rigidbody.velocity = new Vector3(-maxSpeed * speedRate, rigidbody.velocity.y, rigidbody.velocity.z);


        if (rigidbody.velocity.z > maxSpeed * speedRate)
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, maxSpeed * speedRate);
        else if(rigidbody.velocity.z < -maxSpeed * speedRate)
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, -maxSpeed * speedRate);

    }
}
