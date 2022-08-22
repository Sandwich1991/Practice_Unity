using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public static Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;

    public bool _isGameRunning = true;

    protected virtual void init()
    {
        Screen.SetResolution(1080, 1929, true);
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
        if (obj == null)
            Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";
    }
    
    public abstract void Clear();
    
    private void Awake()
    {
        init();
    }

}
