using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    float speedRate = 1.0f;

    private Rigidbody rigidbody;
    float maxSpeed = 5f;
    bool canJump = true;
    bool canMove = true;
    bool canAttack = true;

    Animator animator;

    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        animator = gameObject.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
    }

    void Update()
    {
        MouseRotation();
        if(canMove)
            KeyboardMove();
        Jump();
        Attack();
        
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "ground")
        {
            canJump = true;
            canAttack = true;

            // ���� �ִϸ��̼�
            if (animator.GetInteger("act") == 11)
            {
                animator.SetInteger("act", 12);
            }
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "ground")
        {
            if(canMove)
                canJump = true;

            // ���� �ִϸ��̼�
            if (animator.GetInteger("act") == 11 || animator.GetInteger("act") == 10)
            {
                animator.SetInteger("act", 12);
            }
            
            
            if (animator.GetInteger("act") == 12 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
            {
                animator.SetInteger("act", 0);
            }
            

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            if(canJump)
            {
                animator.SetInteger("act", 11);
            }

            canJump = false;
            canAttack = false;
        }
    }

    void MouseRotation()
        /*
         * ���콺 ���� ���������� ĳ���� ȸ��
         */
    {
        float MouseX = Input.GetAxis("Mouse X");

        transform.Rotate(Vector3.up * MouseX);
    }

    void KeyboardMove()
    /*
     * WASD Ű�� ĳ���� (������ǥ) ������
     */
    {
        // ���� �̵��� ����...
        float forceForward = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            forceForward = 100000f;
            if(animator.GetInteger("act")<=5)
                animator.SetInteger("act", 1);
        }
        else if (Input.GetKey(KeyCode.S)) 
        { 
            forceForward = -100000f;
            if (animator.GetInteger("act") <= 5)
                animator.SetInteger("act", 4);
        }


        // �¿� �̵��� ���� ����
        float forceRight = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            forceRight = -100000f;
            if (animator.GetInteger("act") <= 5)
                animator.SetInteger("act", 2);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            forceRight = 100000f;
            if (animator.GetInteger("act") <= 5)
                animator.SetInteger("act", 3);
        }

        // �밢�� ��츦 ����Ͽ�, ��� �������� ������ �ӵ��� ������ �Ѵ�.
        if(forceForward != 0 && forceRight != 0)
        {
            forceForward = forceForward / Mathf.Sqrt(2);
            forceRight = forceRight / Mathf.Sqrt(2);
        }

        else if (forceForward == 0 && forceRight == 0 && animator.GetInteger("act") <= 4)
        {
            animator.SetInteger("act", 0);
        }

        float backMovingCorrectionValue = 1.0f;

        // �ڷ� ���� �� �̵��ӵ� 25% ����
        if(forceForward < 0)
        {
            backMovingCorrectionValue = 0.75f;
        }


        // �ִ�ӵ� ���� & ������ ����
        // : �ִ�ӵ��� ���� ���� ��쿡�� addForce ����
        Vector3 playerForward = gameObject.transform.forward;
        playerForward.y = 0;
        playerForward = playerForward.normalized;

        Vector3 playerRight = gameObject.transform.right;
        playerRight.y = 0;
        playerRight = playerRight.normalized;

        float horizontalSpeed = new Vector2(rigidbody.velocity.x, rigidbody.velocity.z).magnitude;
        if(horizontalSpeed < maxSpeed * backMovingCorrectionValue)
        {
            rigidbody.AddForce(playerForward * speedRate * forceForward * backMovingCorrectionValue * Time.deltaTime);
            rigidbody.AddForce(playerRight * speedRate * forceRight * Time.deltaTime);
        }


        // �Է� ������ �ӵ� ����
        rigidbody.velocity = new Vector3(rigidbody.velocity.x * 0.97f,
            rigidbody.velocity.y,
            rigidbody.velocity.z * 0.97f);
        
    }

    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rigidbody.AddForce(new Vector3(0, 1, 0) * 5000f);
            animator.SetInteger("act", 10);
            Debug.Log("jump");
        }

        // �߶�
        if (rigidbody.velocity.y < -0.005f && (animator.GetInteger("act") == 10 || animator.GetInteger("act") <= 5))
        {
            animator.SetInteger("act", 11);
        }
    }

    void MoveWithAttack(Vector3 playerForward)
    {
        // ���� �� �������� ����
        if (Input.GetKey(KeyCode.W))
        {
            rigidbody.AddForce(playerForward * 7500);
        }

        else if (Input.GetKey(KeyCode.S)) { }

        else
        {
            rigidbody.AddForce(playerForward * 1500);
        }
    }

    void Attack()
    {
        if(animator.GetInteger("act") >=20 && animator.GetInteger("act") <= 22 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            rigidbody.velocity *= 0.97f;
            canJump = false;
            canMove = false;
        }


        if(Input.GetMouseButtonDown(0) && canAttack)
        {
            rigidbody.velocity = new Vector3(0, 0, 0);
            Vector3 playerForward = gameObject.transform.forward;
            playerForward.y = 0;

            if (animator.GetInteger("act") <= 5)
            {
                animator.SetInteger("act", 20);
                MoveWithAttack(playerForward);
            }

            else if (animator.GetInteger("act") == 20
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f
            && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack01"))
            {
                animator.SetInteger("act", 21);
                MoveWithAttack(playerForward);
            }

            else if (animator.GetInteger("act") == 21
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f
            && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack02"))
            {
                animator.SetInteger("act", 22);
                MoveWithAttack(playerForward);
            }




        }

        else if (animator.GetInteger("act") == 20 
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f
            && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack01"))
        {
            animator.SetInteger("act", 0);
            canJump = true;
            canMove = true;
        }

        else if (animator.GetInteger("act") == 21
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f
            && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack02"))
        {
            animator.SetInteger("act", 0);
            canJump = true;
            canMove = true;
        }

        else if (animator.GetInteger("act") == 22
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f
            && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
        {
            animator.SetInteger("act", 0);
            canJump = true;
            canMove = true;
        }
    }
}
