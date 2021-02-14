using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject Player;

    [SerializeField]
    float radius = 6f;

    float angleY = 90f;

    void Start()
    {
    }

    private void LateUpdate()
    {
        // ���� ����
        angleY -= 15f*Input.GetAxis("Mouse Y");

        // ���ϰ��� ����
        if (angleY > 165f)
            angleY = 165f;
        else if (angleY < 10f)
            angleY = 10f;

        // ĳ���Ͱ� �ٶ󺸴� ������ �ݴ��� ī�޶� ��ġ
        Vector3 playerForward = Player.transform.forward;
        playerForward.y = 0;
        playerForward = playerForward.normalized; // (x, 0, z), x^2 +z^2 = 1


        // ĳ���Ϳ� ī�޶� ���̿� ��ü�� ��ġ�ϸ� ������ ����
        RaycastHit hit;
        if(Physics.Raycast(Player.transform.position, transform.position, out hit, radius, LayerMask.GetMask("Wall")))
        {
            float dist = (hit.point - Player.transform.position).magnitude * 0.8f;

            float planeRange = Mathf.Cos(angleY / 180f) * dist;
            transform.position = new Vector3(
                Player.transform.position.x - playerForward.x * planeRange,
                Player.transform.position.y + Mathf.Sin(angleY / 180f) * dist,
                Player.transform.position.z - playerForward.z * planeRange
                );
        }

        else
        {
            float planeRange = Mathf.Cos(angleY / 180f) * radius;
            transform.position = new Vector3(
                Player.transform.position.x - playerForward.x * planeRange,
                Player.transform.position.y + Mathf.Sin(angleY / 180f) * radius,
                Player.transform.position.z - playerForward.z * planeRange
                );
        }

        // �÷��̾��� �� ��ġ���� y������ +1��ŭ�� ��ġ�� �Ĵٺ���.
        Vector3 LookTaregt = Player.transform.position;
        LookTaregt.y += 1f;
        transform.LookAt(LookTaregt);

    }

    void Update()
    {
        
    }
}
