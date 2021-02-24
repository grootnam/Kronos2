using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
public class ChatScript : MonoBehaviour
{
    //���� �ؽ�Ʈ(�޸���)
    public TextAsset Write_Text;

    //���� ���ӿ� ���� �ؽ�ƮUI
    public Text Chat_Text;

    //���ϴ� npc�̸�
    private Text CharacterName;

    //��ȭ ��ũ��Ʈ �Ҽ��ִ��� ������
    bool flag=true;

    bool skip = false;
    string[] lines;
    int count=0;
    // Start is called before the first frame update
    void Start()
    {
        lines = Write_Text.text.Split('\n');
    }

    // Update is called once per frame
    void Update()
    {
        Chat();
    }
   
    void Chat()
    {
        if(flag ==true && Input.GetKeyDown(KeyCode.F) && lines.Length > count && skip == true)
        {
            Chat_Text.text = "";
            Chat_Text.text = lines[count];
            skip = false;
        }
        else if (flag == true && Input.GetKeyDown(KeyCode.F) && lines.Length >count&&skip==false)
        {
            StartCoroutine(Typing_chat(gameObject.name, lines[count]));
            Debug.Log(lines[count]);
        }
    }
    IEnumerator Typing_chat(string narrator, string chat)
    {
        //CharacterName.text = narrator;
        Chat_Text.text = "";
        for (int i =0;i<chat.Length;i++)
        {
            skip = true;
            Chat_Text.text += chat[i];
            yield return new WaitForSeconds(0.1f);
            if (skip == false)
            {
                break;
            }
        }
        skip = false;
        count++;
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            flag = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            flag = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            flag = false;
        }
    }
}
