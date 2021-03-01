using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : UIManager
{
    /*
     * �κ��丮 ui�� �����ϴ� ��ũ��Ʈ
     * - �� ���ӿ��� ȹ���� ������ �� ��ų ������ ����
     * - 
     */


    [SerializeField]
    GameObject itemList;

    [SerializeField]
    GameObject skillList;

    bool tapToItemList;

    void Start()
    {
        tapToItemList = true;
    }

    void Update()
    {
        
    }

    void OnClickTap(string tap)
    {
        switch (tap)
        {
            case "itemList":
                OpenUI(itemList);
                CloseUI(skillList);
                tapToItemList = true;
                break;

            case "skillList":
                OpenUI(skillList);
                CloseUI(itemList);
                tapToItemList = false;
                break;

            default:
                Debug.Log("wrong tap name");
                break;
        }
    }
    void Close()
    {
        Time.timeScale = 1f;
        CloseUI(gameObject);
    }

}
