using System;
using System.Collections.Generic;
using UnityEngine;
public class GOAPBasedAIAgent : AIAgent
{
    private GOAPPlanner _Planner;
    private GOAPActionMachine _ActionMachine;
    private WorldState _WorldState;
    private WorldState _TargetWorldState;
    protected override void Awake()
    {
        _TargetWorldState = new WorldState();
        _TargetWorldState.SetState("EnemyDead",true);
        _WorldState = new WorldState();
        _WorldState.SetState("FullHP",true);
        _WorldState.SetState("Healthy",true);
        _WorldState.SetState("Recovering",true);
        _WorldState.SetState("EnemyDead",false);
        _Planner = new GOAPPlanner();
        _ActionMachine= new GOAPActionMachine();
        var actions = new List<GOAPAction>();
        actions.Add(new AttackGOAPActon());
        actions.Add(new BackHomeGOAPAction());
        actions.Add(new StayAtBaseGOAPAction());
        _Planner.Init(actions);
        base.Awake();
    }

    private void Update()
    {
        UpdateWorldState();
        
        var running=_ActionMachine.Update(this);
        if (!running)
        {
            NewPlan();
        }
    }

    public void NewPlan()
    {
        var plan = _Planner.Plan(_WorldState, _TargetWorldState, this);
        if (plan != null && plan.Count > 0)
        {
            _ActionMachine.SetActions(new Queue<GOAPAction>(plan));
        }
    }
    public void UpdateWorldState()
    {
        if (Vector3.Distance(transform.position, BasePosition) < 0.3f)
        {
            _WorldState.SetState("Recovering",true);
        }
        else
        {
            _WorldState.SetState("Recovering",false);
        }

        if (GetTank().CurrentHp / GetTank().MaxHp < 0.5f)
        {
            _WorldState.SetState("Healthy",false);
            if (GetTank().CurrentHp / GetTank().MaxHp == 1)
            {
                _WorldState.SetState("FullHP",true);
            }
            else
            {
                _WorldState.SetState("FullHP",false);
            }
        }
        else
        {
            _WorldState.SetState("Healthy",true);
            _WorldState.SetState("FullHP",false);
        }
    }
    public override string GetDescription()
    {
        return "";
    }

    public override void OnRespawn()
    {
     NewPlan();
    }
}