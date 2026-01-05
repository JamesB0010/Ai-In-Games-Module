using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class mouse_FoundTarget : MonoBehaviour
{
    [SerializeField] private Mouse mouse;

    [SerializeField] private UnityEvent CelebrateEvent;
    
    [SerializeField]
    private float jumpHeight;
    
    [SerializeField] private AnimationCurve jumpUpCurve;
    
    [SerializeField] private AnimationCurve jumpDownCurve;
    
    
    public void EnterState(State state)
    {
        this.Celebrate();
    }
    
    private void Celebrate()
        {
            this.mouse.stoppedWalking?.Invoke();
            this.CelebrateEvent?.Invoke();
            Vector3 startPos = transform.position;
            
            transform.position.y.LerpTo(transform.position.y + this.jumpHeight, 0.3f, value =>
            {
                transform.position = new Vector3(transform.position.x, value, transform.position.z);
            }, 
                pkg =>
                {
                    transform.position.y.LerpTo(startPos.y, 0.3f, value =>
                    {
                        transform.position = new Vector3(transform.position.x, value, transform.position.z);
                    }, pkg =>
                    {
                        this.mouse.OriginPoint = transform.position;
                        GetComponent<FSMMonoComponent>().FiniteStateMachine.ActiveState.Transition();
                    }, this.jumpDownCurve);
                }, this.jumpUpCurve);
        }
}
