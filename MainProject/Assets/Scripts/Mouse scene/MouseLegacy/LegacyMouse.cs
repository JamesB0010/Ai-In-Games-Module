using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

public class LegacyMouse : FSMBehaviour, PathfollowingAgent
{
    [SerializeField] private AnimationCurve jumpUpCurve;

    [SerializeField] private AnimationCurve jumpDownCurve;

    [SerializeField] private State moveToStart, idleState, movingToTargetState, foundTargetState, goHome, goToSleepState, sleep;

    [SerializeField] private UnityEvent startedWalking;

    [SerializeField] private UnityEvent stoppedWalking;

    [SerializeField] private UnityEvent GoToSleep;

    [SerializeField] private UnityEvent WakeUp;
    
    public UnityEvent CelebrateEvent;

    private Vector3 originPoint;

    [SerializeField] private float circleRadius;

    private float rotationAngle = 0;

    [SerializeField] private float rotationSpeed;

    public Transform home;

    private List<Node> path;
    [SerializeField]
    private float jumpHeight;

    [SerializeField] private float nodeTraversalTime;

    public Transform Target { set; private get; }

    public Transform houseSleepSpot;

    private Vector3 targetPosition;

    private Vector3LerpPackage movementLerpPackage;
    
    [Tooltip("If the target is within thr possessionRange then the mouse will idle. if the target is outside the possession range then the mouse will chase")]
    [SerializeField] private float possessionRange;

    [SerializeField] private float yOffset;
    public float YOffset => this.yOffset;
    [SerializeField] private float rotationSpeedMultiplier = 2;
    private bool enteringHouse;

    private void Start()
    {
        this.originPoint = transform.position;
    }

    public override void EnterState(State state)
    {
        if (state.IsInstanceOf(this.moveToStart))
        {
            this.startedWalking?.Invoke();
            transform.rotation.y.LerpTo(90 * Mathf.Deg2Rad, 0.5f,
                value => { transform.rotation = quaternion.Euler(0, value, 0); }, pkg => { },
                GlobalLerpProcessor.easeInOutCurve);

                transform.position.LerpTo(this.originPoint + (new Vector3(1, 0, 0) * this.circleRadius), 1.0f,
                    val => { transform.position = val; }, pkg =>
                    {
                        state.Transition();
                    }, GlobalLerpProcessor.easeInOutCurve);
        }

        if (state.IsInstanceOf(this.idleState))
        {
            this.startedWalking?.Invoke();
            transform.rotation = Quaternion.identity;
        }

        if (state.IsInstanceOf(this.movingToTargetState))
            this.MoveToTarget(this.Target);

        if (state.IsInstanceOf(this.foundTargetState))
            this.Celebrate();

        if (state.IsInstanceOf(this.goHome))
        {
            this.startedWalking?.Invoke();
            this.MoveToTarget(this.home);
        }
        
        if (state.IsInstanceOf(this.goToSleepState))
        {
            this.GoToSleep?.Invoke();
            this.stoppedWalking?.Invoke();
        }
    }


    private void Celebrate()
    {
        this.stoppedWalking?.Invoke();
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
                    this.originPoint = transform.position;
                    GetComponent<FSMMonoComponent>().FiniteStateMachine.ActiveState.Transition();
                }, this.jumpDownCurve);
            }, this.jumpUpCurve);
    }
    public override void Behave(State state)
    {
        Debug.Log($"Currently in state {state.StateName}");
        if(state.IsInstanceOf(this.idleState))
            this.RotateAroundOrigin();

        if (state.IsInstanceOf(this.movingToTargetState))
        {
            if(this.Target.position != this.targetPosition)
                this.MoveToTarget(this.Target);
        }

        if (state.IsInstanceOf(this.goHome))
        {
            if (Vector3.Distance(transform.position, this.home.position) <= 10 && !this.enteringHouse)
            {
                EnterMouseHouse();
            }
        }
    }

    private void EnterMouseHouse()
    {
        this.enteringHouse = true;
        GlobalLerpProcessor.RemovePackage(this.movementLerpPackage);
        this.movementLerpPackage = transform.position.LerpTo(this.houseSleepSpot.position, 4f,
            value =>
            {
                transform.position = value;
                transform.rotation = Quaternion.Lerp(transform.rotation,
                    Quaternion.LookRotation(houseSleepSpot.position - transform.position, Vector3.up), Time.deltaTime * 1f);
            },
            pkg =>
            {
                transform.rotation.LerpTo(this.houseSleepSpot.rotation, 2f, value => transform.rotation = value,
                    pkg =>
                    {
                        this.enteringHouse = false;
                        GetComponent<FSMMonoComponent>().FiniteStateMachine.ActiveState.Transition();
                    }, GlobalLerpProcessor.easeInOutCurve);
            });
    }

    public override bool EvaluateTransition(State current, State to)
    {
        if (current.IsInstanceOf(this.idleState))
            return Vector3.Distance(transform.position, this.Target.position) > this.possessionRange;
        
        return false;
    }

    private void MoveToTarget(Transform moveToTransform) 
    {
        this.startedWalking?.Invoke();
        
    if (this.movementLerpPackage != null)
    {
        GlobalLerpProcessor.RemovePackage(this.movementLerpPackage);
    }

    this.targetPosition = moveToTransform.position;
    this.path = Pathfinding.FindPath(transform.position, moveToTransform.position);

    int i = 0;

    if (path == null)
    {
        Debug.Log("path is null");
        return;
    }

   if (path?.Count == 0)
    {
        if(GetComponent<FSMMonoComponent>().FiniteStateMachine.ActiveState == this.goHome)
            this.EnterMouseHouse();
        else
            GetComponent<FSMMonoComponent>().FiniteStateMachine.ActiveState.Transition();
        
        return;
    }


    Vector3 firstNodeWorldPos = path[0].worldPosition + Vector3.up * this.yOffset;

    // Initialize rotation interpolation package
    Quaternion initialRotation = transform.rotation;
    Quaternion targetRotation = Quaternion.LookRotation(new Vector3(firstNodeWorldPos.x, firstNodeWorldPos.y, firstNodeWorldPos.z) - transform.position);

    this.movementLerpPackage = transform.position.LerpTo(
        this.path[0].worldPosition + Vector3.up * this.yOffset,
        this.nodeTraversalTime,
        value =>
        {
            transform.position = new Vector3(value.x, value.y, value.z);

            // Lerp rotation
            transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, this.movementLerpPackage.currentPercentage * rotationSpeedMultiplier);
        },
        pkg =>
        {
            i++;
            if (i == this.path.Count)
            {
                GetComponent<FSMMonoComponent>().FiniteStateMachine.ActiveState.Transition();
                return;
            }

            // Update movement lerp package
            pkg.start = transform.position;
            pkg.target = this.path[i].worldPosition + Vector3.up * this.yOffset;
            pkg.ResetTiming();

            // Update rotation lerp package
            initialRotation = transform.rotation;
            Vector3 nextNodeWorldPos = path[i].worldPosition + Vector3.up * this.yOffset;
            targetRotation = Quaternion.LookRotation(new Vector3(nextNodeWorldPos.x, nextNodeWorldPos.y, nextNodeWorldPos.z) - transform.position);

            GlobalLerpProcessor.AddLerpPackage(pkg);
        }
    );
}

    public override void ExitState(State state)
    {
        Debug.Log($"Exited state {state.StateName}");
        if(state.IsInstanceOf(this.sleep))
            this.WakeUp?.Invoke();
    }


    private void RotateAroundOrigin()
    {
        transform.RotateAround(originPoint, Vector3.up, rotationSpeed * Time.deltaTime);
    }

    public List<Node> GetPath()
    {
        return this.path;
    }
}
