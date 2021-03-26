using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager_fire12 : PlayerInteract
{
    [SerializeField]
    GameObject PuzzleUI;

    bool isPuzzleMode;


    List<List<bool>> MatchTable;

    List<bool> BlockUpOrDown;


    [SerializeField]
    List<GameObject> CandleList;

    [SerializeField]
    List<GameObject> MovingBlockList;


    private void Start()
    {
        MatchTable = new List<List<bool>>();
        isPuzzleMode = false;
        BlockUpOrDown = new List<bool>();

        TableReset();
        BlockReset();
        PuzzleReset();
    }

    public override void InteractPopup()
    {
        interactPopup.transform.Find("content_text").GetComponent<Text>().text = "�ڼ�������";
    }

    public override void OnInteract()
    {
        PuzzleUI.SetActive(true);
        isPuzzleMode = true;
    }

    private void Update()
    {
        PuzzleModeInput();
    }


    void TableReset()
        /*
         * �� ���ʸ��� ������ �������� �Ҵ�,
         * �ش� ���� Ű�� �Է��ϸ� �Ҵ�� ������ �����δ�.
         */
    {
        for (int i = 0; i < CandleList.Count; i++)
        {
            List<bool> column = new List<bool>();

            for(int j=0; j<MovingBlockList.Count; j++)
            {
                float temp = Random.Range(0f, 1f);

                if (temp > 0.5f) 
                    column.Add(true);
                
                else
                    column.Add(false);
            }

            MatchTable.Add(column);

            Debug.Log($"Match Table = {column[0]}, {column[1]}, {column[2]}, {column[3]}, {column[4]}");
        }
    }

    void BlockReset()
        /*
         * ������ ��ġ�� ���� ��ġ�� �ʱ�ȭ,
         * BlockUpOrDown�� �ʱ�ȭ
         */
    {
        for(int i=0; i<MovingBlockList.Count; i++)
        {
            Vector3 StartPos = new Vector3();

            StartPos.x = MovingBlockList[i].transform.position.x;
            StartPos.y = 4;
            StartPos.z = MovingBlockList[i].transform.position.z;

            MovingBlockList[i].transform.position = StartPos;

            BlockUpOrDown.Add(true);
        }
    }


    void PuzzleReset()
        /*
         * MatchTable�� �Ҵ�� ������ ���� ���� ���¿��� �������� ������ ����
         */
    {
        int PuzzleDifficulty = 7;

        for (int i = 0; i < PuzzleDifficulty; i++)
        {
            int RandomCandleIndex = Random.Range(0, CandleList.Count);

            PuzzleInput(RandomCandleIndex);

            Debug.Log($"Puzzle Generated by this candle: {RandomCandleIndex}");
        }
    }


    // �����忡�� ����Ű �Է¿� ���� ���ʸ� �Ѱ� ����.
    void PuzzleModeInput()
    {
        int inputNumber = -1;
        if(isPuzzleMode && Input.GetKeyDown(KeyCode.Alpha1))
        {
            inputNumber = 0;
        }

        else if (isPuzzleMode && Input.GetKeyDown(KeyCode.Alpha2))
        {
            inputNumber = 1;
        }

        else if (isPuzzleMode && Input.GetKeyDown(KeyCode.Alpha3))
        {
            inputNumber = 2;
        }

        else if (isPuzzleMode && Input.GetKeyDown(KeyCode.Alpha4))
        {
            inputNumber = 3;
        }

        else if (isPuzzleMode && Input.GetKeyDown(KeyCode.Alpha5))
        {
            inputNumber = 4;
        }

        if(inputNumber>=0)
        {
            PuzzleInput(inputNumber);
        }
        
    }

    void PuzzleInput(int inputNumber)
        /*
         * - �Է¹��� ���� Ű�� ���� ������ ������
         * - ���� �Է¿� ���� ��ƼŬ ���� ���
         */
    {

        // particle play
        CandleList[inputNumber].transform.Find("Flare").GetComponent<ParticleSystem>().Play();
        
        // move block's position
        for (int i=0; i<MovingBlockList.Count; i++)
        {
            // ���ʿ� i�� ������ �Ҵ�Ǿ� �ִٸ�
            if (MatchTable[inputNumber][i] == true)
            {
                // up
                if(BlockUpOrDown[i])
                {
                    BlockUpOrDown[i] = false;
                    MovingBlockList[i].transform.position = new Vector3(
                        MovingBlockList[i].transform.position.x,
                        4,
                        MovingBlockList[i].transform.position.z);
                    //StartCoroutine("MoveUpBlock(i)");
                }

                // down
                else
                {
                    BlockUpOrDown[i] = true;
                    MovingBlockList[i].transform.position = new Vector3(
                        MovingBlockList[i].transform.position.x,
                        2,
                        MovingBlockList[i].transform.position.z);
                    //StartCoroutine("MoveDownBlock(i)");
                }

            }
        }
    }

    //IEnumerable MoveUpBlock(int blockNumber)
    //{
    //    Transform blockT = MovingBlockList[blockNumber].transform;
    //    while (blockT.position.y < 4)
    //    {
    //        blockT.position = new Vector3(
    //            blockT.position.x,
    //            blockT.position.y + Time.deltaTime,
    //            blockT.position.z);

    //        yield return new WaitForSeconds(0.1f);
    //    }
    //}

    //IEnumerable MoveDownBlock(int blockNumber)
    //{
    //    Transform blockT = MovingBlockList[blockNumber].transform;
    //    while (blockT.position.y > 2)
    //    {
    //        blockT.position = new Vector3(
    //            blockT.position.x,
    //            blockT.position.y + Time.deltaTime,
    //            blockT.position.z);

    //        yield return new WaitForSeconds(0.1f);
    //    }
    //}

}