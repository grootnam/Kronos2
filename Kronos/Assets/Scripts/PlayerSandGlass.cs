using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ability
{
    public enum AbilityType
    {
        none,
        attack,
        health,
        skillUpgrade,
        sharp,
        shield,
        utility,
    }

    public AbilityType type;
    public string name;
    public int tier;
    public int cost;
    public int maxLevel;
    public string desc;
    public string note;

    public int level;
}

public class PlayerSandGlass : MonoBehaviour
{

    [SerializeField]
    public List<Ability> abilities = new List<Ability>();

    public void Init()
    {
        for(int i=0; i<33; i++)
        {
            abilities[i].level = 0;
        }
    }

    public void DataLoad()
    {
        // �����Ͱ� ���ٸ�
        Init();

        // �ִٸ�...
    }


}

