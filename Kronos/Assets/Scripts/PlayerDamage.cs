using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField]
    public float damageRate;


    public float CalculatedDamage()
        /*
         * ���ݷ� * ���ذ����� * ���ط�(%) * ũ��Ƽ�����ط� * ��Ÿ ����������
         * ���� '��Ÿ ������ ����' �ݿ� �ȵ�
         */
    {
        float damage = PlayerStatus.attack * (damageRate / 100f);

        int critical = Random.Range(0, 100);
        // ũ��Ƽ�� �߻�
        if(critical < PlayerStatus.criticalProb)
        {
            damage *= (PlayerStatus.criticalDamage / 100);
            Debug.Log("Critical Hit!");
        }

        return damage;
    }


}
