using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSandGlass : MonoBehaviour
{

    // abilities[i]["header"] : CSV ���� ���� "header" ���� �ִ� i��° ���� ���� �����´�.
    protected List<Dictionary<string, object>> abilities;
    protected List<Dictionary<string, object>> abilities_text;

    enum dataHeader
        /*
         * abilities.csv�� �����
         */
    {
        tier,
        type,
        cost,
        maxLevel,
        level,
    }

    enum dataHeader_text
        /*
         * abilities_text.csv�� �����
         */
    {
        name,
        desc,
    }

    protected int[] SumLevelOfType;

    protected enum typeOfAbility
    {
        attack,
        health,
        skill,
        critical,
        shield,
        utility,
    }

    private void Awake()
    {
        abilities = CSVReader.Read("abilities");
        abilities_text = CSVReader.Read("abilities_text");

        SumLevelOfType = new int[6];
    }


    public void DataSave()
        /*
         * ���� List�� ����� �����͵��� .csv ���Ͽ� ����
         * columns�� �� ���� ������� add�ϰ� writer.WriteRow()�� ����� ���� ����
         */
    {
        var writer = new CsvFileWriter(Application.dataPath + "/Resources/abilities.csv");

        List<string> columns = new List<string>();

        // ù ���� data_header
        foreach (dataHeader h in System.Enum.GetValues(typeof(dataHeader)))
        {
            string temp = h.ToString();
            columns.Add(temp);
        }

        writer.WriteRow(columns);
        columns.Clear();

        // ���� ����� ���� ������ ����
        for (int i = 0; i < abilities.Count; i++)
        {
            columns.Clear();

            foreach (dataHeader h in System.Enum.GetValues(typeof(dataHeader)))
            {
                string header = h.ToString();
                string temp = abilities[i][header].ToString();
                columns.Add(temp);
            }
            writer.WriteRow(columns);
            columns.Clear();
        }

        writer.Dispose();
        Debug.Log("data saved");
    }

    protected void RenewSumLevelOfType()
        /*
         * �� �迭 �ɷµ��� ��ų ���� ���ؼ� ����
         */
    {
        for(int i=0; i<SumLevelOfType.Length; i++)
        {
            SumLevelOfType[i] = 0;
        }

        for (int i=0; i<abilities.Count; i++)
        {
            switch(abilities[i]["type"].ToString()) 
            {
                case "attack":
                    SumLevelOfType[0] += int.Parse(abilities[i]["level"].ToString());
                    break;

                case "health":
                    SumLevelOfType[1] += int.Parse(abilities[i]["level"].ToString());
                    break;

                case "skill":
                    SumLevelOfType[2] += int.Parse(abilities[i]["level"].ToString());
                    break;

                case "critical":
                    SumLevelOfType[3] += int.Parse(abilities[i]["level"].ToString());
                    break;

                case "shield":
                    SumLevelOfType[4] += int.Parse(abilities[i]["level"].ToString());
                    break;

                case "utility":
                    SumLevelOfType[5] += int.Parse(abilities[i]["level"].ToString());
                    break;

                default:
                    break;
            }
        }
    }

}

