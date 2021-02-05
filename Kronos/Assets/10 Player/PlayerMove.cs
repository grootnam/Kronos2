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
        PlayerMoveBase();
        Jump();
        Attack();
        Defend();
        Roll();
        
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

    void PlayerMoveBase()
    {
        // �Է� ������ �ӵ� ����
        rigidbody.velocity = new Vector3(rigidbody.velocity.x * 0.97f,
            rigidbody.velocity.y,
            rigidbody.velocity.z * 0.97f);

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
         * 1 = ����
         * 2 = ��, 3 = ��� �ȱ�
         * 4 = ����
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

    }

    void Jump()
        /*
         * space Ű�� ����
         * 10 = ���� ���� -> ���� ��
         * 11 = ���� �ٿ�
         * 12 = ����
         */
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
        /*
         * ���� �� ��/�� ����Ű�� �Է�, ���� �� �����̴� ���� ����
         */
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
        /*
         * ��Ŭ������ �⺻����
         * act 20~22 : ���� �⺻���� 1Ÿ~3Ÿ
         */
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

    void Defend()
        /*
         * ��Ŭ������ ���
         * 30 : ���
         */
    {

        // ��Ŭ�� ���� = ��� ����
        if(Input.GetMouseButtonDown(1) && animator.GetInteger("act") <=5 )
        {
            animator.SetInteger("act", 30);
            canMove = false;
            canJump = false;
            canAttack = false;
        }

        if(animator.GetInteger("act") == 30 && !Input.GetMouseButton(1))
        {
            animator.SetInteger("act", 0);
            animator.Play("Idle_Battle");
            canMove = true;
            canJump = true;
            canAttack = true;
        }


    }

    void Roll()
        /*
         * Shift�� ������
         * 40 �̻� 70 �̸�
         * (+1) : ������, (+4) ��������, (+9) ����������, (+16) �ڷ�
         * �밢���� �� ������ ��
         */
    {
        if(canMove && canJump && Input.GetKeyDown(KeyCode.LeftShift))
        {
            Vector3 playerFront = gameObject.transform.forward;
            Vector3 playerRight = gameObject.transform.right;
            playerFront.y = 0;
            playerRight.y = 0;
            playerFront = playerFront.normalized;
            playerRight = playerRight.normalized;

            rigidbody.velocity = new Vector3(0, 0, 0);
            float frontForce = 0;
            float rightForce = 0;

            int rollDir = 0;

            // �Է¿� ���� ���� ���
            // �¿�
            if(Input.GetKey(KeyCode.A))
            {
                rollDir += 4;
                rightForce = -30000;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                rollDir += 9;

                rightForce = 30000;
            }
            // ����
            if (Input.GetKey(KeyCode.S))
            {
                rollDir += 16;
                frontForce = -30000;
            }
            else if (Input.GetKey(KeyCode.W))
            {
                rollDir += 1;
                frontForce = 30000;
            }

            if(frontForce != 0 && rightForce != 0)
            {
                frontForce /= Mathf.Sqrt(2);
                rightForce /= Mathf.Sqrt(2);
            }

            // �ִϸ��̼� �� ���� ����
            if (!(frontForce == 0 && rightForce == 0))
            {
                canMove = false;
                canAttack = false;
                canJump = false;

                animator.SetInteger("act", 40 + rollDir);
                rigidbody.AddForce(playerFront * frontForce);
                rigidbody.AddForce(playerRight * rightForce);
            }

        }

        else if(animator.GetInteger("act") >= 40 && animator.GetInteger("act") <= 70
            && animator.GetCurrentAnimatorStateInfo(0).IsName("Roll_end"))
        {
            canMove = true;
            canAttack = true;
            canJump = true;

            animator.SetInteger("act", 0);
        }

    }

}
