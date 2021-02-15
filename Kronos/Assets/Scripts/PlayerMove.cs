using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    float speedRate = 1.0f;

    private Rigidbody rigidbody;
    float maxSpeed = 5f;

    enum PlayerState
    {
        Die,
        Idle,
        Walk,
        Jump,
        Fall,
        Roll,
        Attack,
        Defend
    }

    [SerializeField]
    PlayerState _playerState;

    // ��Ÿ�� ����
    float rollCoolTime = 1.5f;
    float remainRollCoolTime = 0f;
    float remainQSkillCool = 0f;
    float remainESkillCool = 0f;


    //���� ����
    [System.NonSerialized]
    public bool SwordSlashAttack;
    Animator animator;

    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        animator = gameObject.GetComponent<Animator>();

        _playerState = PlayerState.Idle;

        // InputManager�� �Լ� ���
    }

    private void FixedUpdate()
    {
    }

    void Update()
    {
        KeyboardMove();
        Jump();
        Attack();
        Defend();
        Roll();
        Interaction();
        QSkill();
        ESkill();


        MouseRotation();
        PlayerMoveBase();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            // ���� �ִϸ��̼�
            if (animator.GetInteger("Input") == 10 || animator.GetInteger("Input") == 11)
            {
                animator.SetInteger("Input", 12);
            }
        }
    }




    void PlayerMoveBase()
    {
        // �Է� ������ �ӵ� ����
        rigidbody.velocity = new Vector3(rigidbody.velocity.x * 0.95f,
            rigidbody.velocity.y,
            rigidbody.velocity.z * 0.95f);
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
        if (_playerState != PlayerState.Idle
            && _playerState != PlayerState.Walk
            && _playerState != PlayerState.Jump
            && _playerState != PlayerState.Fall)
            return;


        // ���� �̵��� ����...
        float forceForward = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            forceForward = 50000f;
            if(animator.GetInteger("Input")<=5)
                animator.SetInteger("Input", 1);
        }
        else if (Input.GetKey(KeyCode.S)) 
        { 
            forceForward = -50000f;
            if (animator.GetInteger("Input") <= 5)
                animator.SetInteger("Input", 4);
        }


        // �¿� �̵��� ���� ����
        float forceRight = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            forceRight = -50000f;
            if (animator.GetInteger("Input") <= 5)
                animator.SetInteger("Input", 2);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            forceRight = 50000f;
            if (animator.GetInteger("Input") <= 5)
                animator.SetInteger("Input", 3);
        }

        // �밢�� ��츦 ����Ͽ�, ��� �������� ������ �ӵ��� ������ �Ѵ�.
        if(forceForward != 0 && forceRight != 0)
        {
            forceForward = forceForward / Mathf.Sqrt(2);
            forceRight = forceRight / Mathf.Sqrt(2);
        }

        else if (forceForward == 0 && forceRight == 0 && animator.GetInteger("Input") <= 4)
        {
            _playerState = PlayerState.Idle;
            animator.SetInteger("Input", 0);
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
        // �߶�
        if (rigidbody.velocity.y < -0.5f)
        {
            animator.SetInteger("Input", 11);
            _playerState = PlayerState.Fall;
        }

        if (_playerState != PlayerState.Idle
            && _playerState != PlayerState.Walk
            && _playerState != PlayerState.Jump 
            && _playerState != PlayerState.Fall)
            return;


        if (Input.GetKeyDown(KeyCode.Space) 
            && (_playerState == PlayerState.Idle || _playerState == PlayerState.Walk))
        {
            rigidbody.AddForce(new Vector3(0, 1, 0) * 5000f);
            animator.SetInteger("Input", 10);
            _playerState = PlayerState.Jump;
            Debug.Log("jump");
        }

        if(animator.GetInteger("Input") == 12
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f
            && animator.GetCurrentAnimatorStateInfo(0).IsName("JumpEnd")
            )
        {
            animator.SetInteger("Input", 0);
            _playerState = PlayerState.Idle;
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
            rigidbody.AddForce(playerForward * 22500);
        }

        else if (Input.GetKey(KeyCode.S)) { }

        else
        {
            rigidbody.AddForce(playerForward * 7500);
        }
    }

    void Attack()
        /*
         * ��Ŭ������ �⺻����
         * Input 20~22 : ���� �⺻���� 1Ÿ~3Ÿ
         */
    {

        if (_playerState != PlayerState.Idle
            && _playerState != PlayerState.Walk
            && _playerState != PlayerState.Attack)
            return;


        if (animator.GetInteger("Input") >=20 && animator.GetInteger("Input") <= 22 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            rigidbody.velocity *= 0.97f;
        }


        if(Input.GetMouseButtonDown(0))
        {
            rigidbody.velocity = new Vector3(0, 0, 0);
            Vector3 playerForward = gameObject.transform.forward;
            playerForward.y = 0;

            if (animator.GetInteger("Input") <= 5)
            {
                animator.SetInteger("Input", 20);
                _playerState = PlayerState.Attack;
                MoveWithAttack(playerForward);
            }

            else if (animator.GetInteger("Input") == 20
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f
            && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack01"))
            {
                animator.SetInteger("Input", 21);
                _playerState = PlayerState.Attack;
                MoveWithAttack(playerForward);
            }

            else if (animator.GetInteger("Input") == 21
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f
            && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack02"))
            {
                animator.SetInteger("Input", 22);
                _playerState = PlayerState.Attack;
                MoveWithAttack(playerForward);
            }




        }

        else if (animator.GetInteger("Input") == 20 
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f
            && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack01"))
        {
            animator.SetInteger("Input", 0);
            _playerState = PlayerState.Idle;
        }

        else if (animator.GetInteger("Input") == 21
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f
            && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack02"))
        {
            animator.SetInteger("Input", 0);
            _playerState = PlayerState.Idle;
        }

        else if (animator.GetInteger("Input") == 22
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f
            && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
        {
            animator.SetInteger("Input", 0);
            _playerState = PlayerState.Idle;
        }
    }

    void Defend()
        /*
         * ��Ŭ������ ���
         * 30 : ���
         */
    {

        if (_playerState != PlayerState.Idle
            && _playerState != PlayerState.Walk
            && _playerState != PlayerState.Defend)
            return;

        // ��Ŭ�� ���� = ��� ����
        if (Input.GetMouseButtonDown(1) && animator.GetInteger("Input") <=5 )
        {
            animator.SetInteger("Input", 30);
            _playerState = PlayerState.Defend;
        }

        if(animator.GetInteger("Input") == 30 && !Input.GetMouseButton(1))
        {
            animator.SetInteger("Input", 0);
            _playerState = PlayerState.Idle;
            animator.Play("Idle_Battle");
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
        remainRollCoolTime -= Time.deltaTime;

        if (_playerState != PlayerState.Idle
            && _playerState != PlayerState.Walk
            && _playerState != PlayerState.Roll)
            return;


        if (Input.GetKeyDown(KeyCode.LeftShift) && remainRollCoolTime <= 0f)
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
                _playerState = PlayerState.Roll;

                animator.SetInteger("Input", 40 + rollDir);
                rigidbody.AddForce(playerFront * frontForce);
                rigidbody.AddForce(playerRight * rightForce);

                remainRollCoolTime = rollCoolTime;
            }

        }

        else if(animator.GetInteger("Input") >= 40 && animator.GetInteger("Input") <= 70
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.85f)
        {
            _playerState = PlayerState.Idle;
            animator.SetInteger("Input", 0);
        }

    }

    void Interaction()
        /*
         * FŰ�� ��ȣ�ۿ�
         */
    {
        if(Input.GetKeyDown(KeyCode.F))
        {



        }
    }

    void QSkill()
    /*
     * QŰ�� ��ų1 ���
     */
    {
        if (Input.GetKeyDown(KeyCode.Q) && remainQSkillCool <= 0)
        {



        }
    }

    void ESkill()
    /*
     * EŰ�� ��ų2 ���
     */
    {
        if (Input.GetKeyDown(KeyCode.E) && remainESkillCool <= 0)
        {



        }
    }


}
