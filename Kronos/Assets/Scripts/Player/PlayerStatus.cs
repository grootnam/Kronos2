using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public static class PlayerStatus
{
    static public float timeMax;
    static public float timeRemain;

    static public int HPMax;
    static public int HP;
    static public float StaminaMax;
    static public float Stamina;

    static public int attack;
    static public int shield;
    static public int attackSpeed;
    static public int moveSpeed;
    static public int criticalProb;
    static public int criticalDamage;
    static public float avoidanceRate;
    static public float coolTimeDecreaseRate;

    // �𷡽ð� �ý���
    static public int level;
    static public int remainPoint;
    static public float exp;
    static public float expMax;


    static public void init()
    {
        timeMax = 30;
        timeRemain = 30;

        HPMax = 100;
        HP = 100;
        StaminaMax = 100;
        Stamina = 100;
        attack = 10;
        shield = 0;
        attackSpeed = 100;
        moveSpeed = 100;
        criticalProb = 0;
        criticalDamage = 150;
        avoidanceRate = 0;
        coolTimeDecreaseRate = 0;

        exp = 0;
        expMax = 100;
    }

    static public void Load()
        /*
         * ����� �����͸� �ҷ��´�.
         * - �����ϴ� �����ʹ� ���� ���� ������ ����
         */
    {

    }

    static public void GainExp(float amount)
        /*
         * ���͸� ���� �� ����
         */
    {
        exp += amount;

        // ������ ����
        while (exp >= expMax)
        {
            exp -= expMax;
            level++;
            remainPoint++;
        }
    }

    static public void DeleteExp()
        /*
         * ����� ����ġ ����
         */
    {
        exp = 0;
    }


}
