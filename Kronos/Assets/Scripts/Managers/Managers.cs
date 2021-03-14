using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance { get { Init(); return s_instance; } }


    InputManager _input = new InputManager();
    public static InputManager Input { get { return Instance._input; } }

    [SerializeField]
    List<GameObject> dontDestroyObj;

    void Start()
    {
        Init();

        // ���콺 Ŀ�� ���� 
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Serialize�� ������ ������Ʈ��
        for (int i=0; i<dontDestroyObj.Count; i++)
        {
            DontDestroyOnLoad(dontDestroyObj[i]);
        }
    }

    void Update()
    {
        _input.OnUpdate();
    }

    static void Init()
    {
        // Managers�� Empty Object�κ��� �������µ�, ���� �ش� ������Ʈ�� ��� �ҷ��� �� ���ٸ� ���� �����Ѵ�.
        if(s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");

            if(go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();
        }
    }
}
