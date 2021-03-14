using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ��ȣ�ۿ��� ������ ������Ʈ�� .cs ������ ������ ��,
 * �� Ŭ������ ����ϸ�
 * 'OnInteract()'�Լ� �κи� �ۼ��ϸ� �ȴ�.
 */

public abstract class PlayerInteract : MonoBehaviour
{
    protected GameObject interactPopup;

    bool canInteract;


    private void Start()
    {
        interactPopup = GameObject.Find("Canvas").transform.Find("Ingame").Find("Ingame_Interact").gameObject;
        interactPopup.SetActive(false);
        canInteract = false;
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && canInteract)
        {
            OnInteract();
        }
    }


    private void OnTriggerStay(Collider other)
    {
        // �÷��̾� ���� �� UI Ȱ��ȭ
        if(other.transform.tag == "Player")
        {
            InteractPopup();
            interactPopup.SetActive(true);
            canInteract = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {

        // �÷��̾� exit = ��Ȱ��ȭ
        if (other.transform.tag == "Player")
        {
            interactPopup.SetActive(false);
            canInteract = false;
        }
    }


    // ����(�ڽ�) Ŭ�������� ����

    // ��ȣ�ۿ� ���� ����
    public abstract void InteractPopup();

    // ��ȣ�ۿ� Ű �Է� �� ����
    public abstract void OnInteract();

}
