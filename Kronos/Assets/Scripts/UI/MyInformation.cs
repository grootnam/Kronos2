using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyInformation : UIManager
{
    /*
     * �� ���� ui�� �����ϴ� ��ũ��Ʈ
     * - ������ ������ point�� �ǽð� ����
     * - �÷��̾� ���� ������ ǥ��
     * - �𷡽ð� �Ǵ� ���� ���� �̵�
     * - �𷡽ð� : 
     * - ���� :
     * - [�ݱ�] ������ UI ����
     */

    bool tapToSandglass;

    [SerializeField]
    GameObject MyInformation_Taps_SandGlass;

    [SerializeField]
    GameObject MyInformation_Taps_HolyThings;

    Text[] numbers; //�������ͽ�. ������� �ð�,���ݷ�,����,���ݼӵ�,�̵��ӵ�,ġȮ,ġ��,ȸ��,��
    Text remain; // ���� ����Ʈ
    Button[] taps; //�� ��ư
    Button closer;

    private void Start()
    {
        tapToSandglass = true;

        remain = GameObject.Find("MyInformation_remain_text").GetComponent<Text>();
        numbers = GameObject.Find("MyInformation_ability_number").transform.GetComponentsInChildren<Text>();
        taps = GameObject.Find("MyInformation_Taps_tapButtons").transform.GetComponentsInChildren<Button>();
        closer = GameObject.Find("MyInformation_close").GetComponent<Button>();

        taps[0].onClick.AddListener(delegate { OnClickTap("SandGlass"); });
        taps[1].onClick.AddListener(delegate { OnClickTap("HolyThings"); });
        closer.onClick.AddListener(Close);

        RefreshInformation();
    }

    private void OnEnable()
    {
        // Ȱ��ȭ���� �� �ڵ����� �� �� �ϳ��� ����
        if(tapToSandglass)
        {
            OpenUI(MyInformation_Taps_SandGlass);
            CloseUI(MyInformation_Taps_HolyThings);
        }

        else
        {
            OpenUI(MyInformation_Taps_HolyThings);
            CloseUI(MyInformation_Taps_SandGlass);
        }

        Time.timeScale = 0;
    }

    public void RefreshInformation()
    {
        // (���� ����Ʈ)/(����)
        remain.text = PlayerStatus.remainPoint.ToString() + " / " + PlayerStatus.level.ToString();

        numbers[0].text = PlayerStatus.timeMax.ToString();
        numbers[1].text = PlayerStatus.attack.ToString();
        numbers[2].text = PlayerStatus.shield.ToString();
        numbers[3].text = PlayerStatus.attackSpeed.ToString();
        numbers[4].text = PlayerStatus.moveSpeed.ToString();
        numbers[5].text = PlayerStatus.criticalProb.ToString();
        numbers[6].text = PlayerStatus.criticalDamage.ToString();
        numbers[7].text = PlayerStatus.avoidanceRate.ToString();
        numbers[8].text = PlayerStatus.coolTimeDecreaseRate.ToString();
    }

    void OnClickTap(string tap)
    {
        switch(tap)
        {
            case "SandGlass":
                OpenUI(MyInformation_Taps_SandGlass);
                CloseUI(MyInformation_Taps_HolyThings);
                tapToSandglass = true;
                break;

            case "HolyThings":
                OpenUI(MyInformation_Taps_HolyThings);
                CloseUI(MyInformation_Taps_SandGlass);
                tapToSandglass = false;
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
