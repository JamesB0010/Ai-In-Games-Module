using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

public class Mouse : MonoBehaviour, PathfollowingAgent
{
    [SerializeField]
    private State goHome;

    [SerializeField] public UnityEvent startedWalking;

    [SerializeField] public UnityEvent stoppedWalking;

    public Vector3 OriginPoint { get; set; }

    private float rotationAngle = 0;

    private List<Node> path;

    [SerializeField] private float nodeTraversalTime;

    public Transform Target { set; get; }

    public Transform houseSleepSpot;

    private Vector3 targetPosition;

    public Vector3 TargetPosition => this.targetPosition;

    private Vector3LerpPackage movementLerpPackage;

    public Vector3LerpPackage MovementLerpPackage
    {
        get => this.movementLerpPackage;
        set => this.movementLerpPackage = value;
    }

    

    [SerializeField] private float yOffset;
    public float YOffset => this.yOffset;
    [SerializeField] private float rotationSpeedMultiplier = 2;
    private void Start()
    {
        this.OriginPoint = transform.position;
        Pathfinding.pathfindingStratChanged += () => { this.MoveToTarget(this.Target); };
    }

    public void EnterMouseHouse()
    {
        GlobalLerpProcessor.RemovePackage(this.movementLerpPackage);
        this.movementLerpPackage = transform.position.LerpTo(this.houseSleepSpot.position, 4f,
            value =>
            {
                transform.position = value;
                transform.rotation = Quaternion.Lerp(transform.rotation,
                    Quaternion.LookRotation(houseSleepSpot.position - transform.position, Vector3.up),
                    Time.deltaTime * 1f);
            },
            pkg =>
            {
                transform.rotation.LerpTo(this.houseSleepSpot.rotation, 2f, value => transform.rotation = value,
                    pkg =>
                    {
                        GetComponent<FSMMonoComponent>().FiniteStateMachine.ActiveState.Transition();
                    }, GlobalLerpProcessor.easeInOutCurve);
            });
    }

    public void MoveToTarget(Transform moveToTransform)
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
            if (GetComponent<FSMMonoComponent>().FiniteStateMachine.ActiveState == this.goHome)
                this.EnterMouseHouse();
            else
                GetComponent<FSMMonoComponent>().FiniteStateMachine.ActiveState.Transition();

            return;
        }


        Vector3 firstNodeWorldPos = path[0].worldPosition + Vector3.up * this.yOffset;

        // Initialize rotation interpolation package
        Quaternion initialRotation = transform.rotation;
        Quaternion targetRotation =
            Quaternion.LookRotation(new Vector3(firstNodeWorldPos.x, firstNodeWorldPos.y, firstNodeWorldPos.z) -
                                    transform.position);

        this.movementLerpPackage = transform.position.LerpTo(
            this.path[0].worldPosition + Vector3.up * this.yOffset,
            this.nodeTraversalTime,
            value =>
            {
                transform.position = new Vector3(value.x, value.y, value.z);

                // Lerp rotation
                transform.rotation = Quaternion.Lerp(initialRotation, targetRotation,
                    this.movementLerpPackage.currentPercentage * rotationSpeedMultiplier);
            },
            pkg =>
            {
                i++;
                if (i == this.path.Count)
                {

                    if (GetComponent<FSMMonoComponent>().FiniteStateMachine.ActiveState.IsInstanceOf(this.goHome))
                    {
                        this.EnterMouseHouse();
                        return;
                    }
                    
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
                targetRotation =
                    Quaternion.LookRotation(new Vector3(nextNodeWorldPos.x, nextNodeWorldPos.y, nextNodeWorldPos.z) -
                                            transform.position);

                GlobalLerpProcessor.AddLerpPackage(pkg);
            }
        );
    }

    public List<Node> GetPath()
    {
        return this.path;
    }
}