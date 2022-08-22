using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    /**********************************************************************/
    static Managers _instance;
    public static Managers Instance { get { init(); return _instance; }}
    
    /**********************************************************************/
    private InputManager _input = new InputManager();
    private ResourceManager _resource = new ResourceManager();
    private SceneManagerEx _scene = new SceneManagerEx();
    private MyCharacter _myCharacter = new MyCharacter();
    /**********************************************************************/
    public static InputManager Input { get {return Instance._input; } }
    public static ResourceManager Resource { get {return Instance._resource; } }
    public static SceneManagerEx Scene { get {return Instance._scene; } }
    public static GameObject MyCharacter { get {return Instance._myCharacter.Character; } set { Instance._myCharacter.Character = value; } }
    /**********************************************************************/
    public static void init()
    {
        if (_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
                
                DontDestroyOnLoad(go);
                _instance = go.GetComponent<Managers>();
            }
        }
    }

    private void Start()
    {
        init();
    }

    private void Update()
    {
        _input.OnUpdate();
    }
    
    public static void Clear()
    {
        Input.Clear();
    }
}
