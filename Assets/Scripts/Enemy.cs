using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy 
{
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EnemyStateMachine stateMachine { get; private set; }

    protected virtual void Awake()
    {
        stateMachine = new EnemyStateMachine();
    }

    protected virtual void Update() {
        stateMachine.currentState.Update();
    }
}
