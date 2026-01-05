using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIAgent))]
public class BehaviourTreeRunner : MonoBehaviour
{
    [SerializeField] private BehaviourTree tree;

    public BehaviourTree Tree => this.tree;


    private void Start()
    {
        this.tree = this.tree.Clone();
        tree.Bind(GetComponent<AIAgent>());
    }

    private void Update()
    {
        this.tree.Update();
    }
}
