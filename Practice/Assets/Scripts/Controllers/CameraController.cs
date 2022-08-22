using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Vector3 _delta = new Vector3(0.0f, 20.5f, -20.0f);
    [SerializeField] private GameObject _player;
    
    void Start()
    {
        _player = Managers.MyCharacter;
    }

    
    void LateUpdate()
    {
        transform.position = _player.transform.position + _delta;
        transform.LookAt(_player.transform);
    }
}
