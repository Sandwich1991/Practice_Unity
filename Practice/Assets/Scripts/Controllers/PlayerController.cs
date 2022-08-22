using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Define.EntityState _state = Define.EntityState.Idle;
    Define.EntityState State { get { return _state; } set { _state = value; } }
    private Animator _animator;
    private Vector3 _desPos;
    private Camera _camera;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _fadeTime = 0.07f;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _camera = Camera.main;

        Managers.Input.MouseAction += OnMouse;
    }

    private void Update()
    {
        switch (State)
        {
            case Define.EntityState.Idle:
                UpdateIdle();
                break;
            case Define.EntityState.Move:
                UpdateMove();
                break;
        }
    }

    void OnMouse(Define.MouseEvent evt)
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Ground")))
        {
            _desPos = hit.point;
            State = Define.EntityState.Move;
        }
        
        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("NPC")) && evt == Define.MouseEvent.Click)
        {
            _desPos = hit.collider.transform.position + Vector3.back * 3f;
            State = Define.EntityState.Move;
            NPCController npc = hit.collider.GetComponent<NPCController>();
            npc.OnClicked();
        }
    }
    
    private void UpdateIdle()
    {
        _animator.CrossFade("Idle", _fadeTime);
    }

    private void UpdateMove()
    {
        Vector3 dir = _desPos - transform.position;

        if (dir.magnitude < 0.3f)
            State = Define.EntityState.Idle;
        else
        {
            _state = Define.EntityState.Move;
            float moveDist = Math.Clamp(_speed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            _animator.Play("Run");
        }
    }
}