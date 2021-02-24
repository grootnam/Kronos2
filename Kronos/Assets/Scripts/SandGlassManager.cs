using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SandGlassManager : PlayerSandGlass
{
    [SerializeField]
    GameObject AbilityList;

    [SerializeField]
    GameObject ScrollViewContent;

    

    void Start()
    {
        InitList();
    }

    void InitList()
        /*
         * ui���� ����Ʈ���� ��ġ
         */
    {
        RenewSumLevelOfType();

        for(int i=0; i<abilities.Count; i++)
        {
            GameObject SandGlass_list = GameObject.Instantiate(AbilityList);
            SandGlass_list.transform.parent = ScrollViewContent.transform;
            SandGlass_list.name = "SandGlass_list" + i.ToString();


            Text[] dataText = SandGlass_list.GetComponentsInChildren<Text>();
            //������� type, tier, name, desc, level

            string type = abilities[i]["type"].ToString();
            string type_kor;

            // �ѱ۷� ����
            switch(type)
            {
                case "none":
                    type_kor = "-";
                    break;

                case "attack":
                    type_kor = "����";
                    break;

                case "health":
                    type_kor = "ü��";
                    break;

                case "skill":
                    type_kor = "��ų��ȭ";
                    break;

                case "critical":
                    type_kor = "ġ��";
                    break;

                case "shield":
                    type_kor = "���";
                    break;

                case "utility":
                    type_kor = "��ƿ��Ƽ";
                    break;

                default:
                    type_kor = type;
                    break;
            }

            dataText[0].text = type_kor;
            dataText[1].text = abilities[i]["tier"].ToString();
            dataText[2].text = abilities_text[i]["name"].ToString();
            dataText[3].text = abilities_text[i]["desc"].ToString();
            dataText[4].text = abilities[i]["level"].ToString();

            // ��ư Ȱ��ȭ ���� üũ
            Transform button = SandGlass_list.transform.Find("Button");
            button.GetComponent<Button>().interactable = false;

            int tier = int.Parse(dataText[1].text);

            switch(tier)
            {
                case 0:
                    button.GetComponent<Button>().interactable = true;
                    break;

                case 1:
                    // 1Ƽ�� : '�ð� ����' �ɷ� 10���� �̻�
                    if(int.Parse(abilities[0]["level"].ToString()) >= 10)
                    {
                        button.GetComponent<Button>().interactable = true;
                    }
                    break;

                case 2:
                    // 2Ƽ�� : ���ϰ迭 �ɷ� ���� �� 10 �̻�
                    int index = 10;
                    foreach(typeOfAbility t in System.Enum.GetValues(typeof(typeOfAbility)))
                    {
                        if(t.ToString() == abilities[i]["type"].ToString())
                        {
                            index = (int)t;
                            break;
                        }
                    }

                    if (SumLevelOfType[index] >= 10)
                    {
                        button.GetComponent<Button>().interactable = true;
                    }
                    break;

                case 3:
                    break;

                default:
                    break;
            }

            // �� ��ư�� onClick Listener �߰�
            //button.GetComponent<Button>().onClick.AddListener(delegate { AbilityLevelUp(i); });
            button.GetComponent<Button>().onClick.AddListener(() => { AbilityLevelUp(i); });

            Debug.Log($"index = {i}");

        }
    }

    void AbilityLevelUp(int index)
    {
        index -= 39;
        Debug.Log($"index = {index}");

        int cost = int.Parse(abilities[index]["cost"].ToString());

        //if (cost > PlayerStatus.remainPoint)
        //    return;
        

        PlayerStatus.remainPoint -= cost;
        int level = int.Parse(abilities[index]["level"].ToString());
        level++;
        abilities[index]["level"] = level;
        DataSave();

        //PlayerStatus �� ���� ���ּ���

        // ui ����
        GameObject.Find("SandGlass_list" + index.ToString()).transform.Find("data_level").GetComponent<Text>().text = level.ToString();

        // ��ư Ȱ��ȭ ���� ��üũ

    }


}
