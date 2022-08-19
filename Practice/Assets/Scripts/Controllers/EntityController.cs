using UnityEngine;

public abstract class EntityController : MonoBehaviour
{
    protected Define.EntityState _state = Define.EntityState.Idle;

    protected string _idle = "Idle";
    protected string _run = "Run";
    
    
    public virtual Define.EntityState State
    {
        get
        { return _state; }
        set
        {
            _state = value;
            switch (State)
            {
                case Define.EntityState.Idle:
                    // _animator.CrossFade(_idle, 0.05f);
                    break;
            
                case Define.EntityState.Move:
                    // _animator.Play(_run);
                    break;
            }
        }
    }

    protected abstract void init();
}