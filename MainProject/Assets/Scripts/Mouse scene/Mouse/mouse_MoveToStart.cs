using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class mouse_MoveToStart : MonoBehaviour
{
    [SerializeField] private Mouse mouse;

    
    [SerializeField] private float circleRadius;
    
    public void EnterState(State state)
    {
        this.mouse.startedWalking?.Invoke();
        transform.rotation.y.LerpTo(90 * Mathf.Deg2Rad, 0.5f,
            value => { transform.rotation = quaternion.Euler(0, value, 0); }, pkg => { },
            GlobalLerpProcessor.easeInOutCurve);

        transform.position.LerpTo(this.mouse.OriginPoint + (new Vector3(1, 0, 0) * this.circleRadius), 1.0f,
            val => { transform.position = val; }, pkg => { state.Transition(); }, GlobalLerpProcessor.easeInOutCurve);
    }
}
