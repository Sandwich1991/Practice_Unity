using UnityEngine;
public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int idx = name.LastIndexOf('/');
            if (idx >= 0) // idx가 존재한다면
                name = name.Substring(idx + 1); // name은 문자 '/'이후
        }
        return Resources.Load<T>(path);
    }
    
    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Faild to Load prefab : {path}");
            return null;
        }

        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;

        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;
        
        Object.Destroy(go);
    }
}