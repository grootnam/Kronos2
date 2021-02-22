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
        StartCoroutine(Chat());
    }
   
    IEnumerator Chat()
    {
        Chat_Text.text = "";
        if (flag == true && Input.GetKeyDown(KeyCode.F) && lines[count]!=null)
        {
            yield return StartCoroutine(Typing_chat(gameObject.name, lines[count]));
            Debug.Log(lines[count]);
            count++;
        }
    }
    IEnumerator Typing_chat(string narrator, string chat)
    {
        //CharacterName.text = narrator;

        for(int i =0;i<chat.Length;i++)
        {
            Chat_Text.text += chat[i];
            yield return new WaitForSeconds(0.1f);
        }
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
