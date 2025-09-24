using System;
using UnityEngine;

public class BackHomeGOAPAction : GOAPAction
{
    public BackHomeGOAPAction()
    {
        effectState = new WorldState();
        effectState.SetState("Recovering",true);
        preState = new WorldState();
    } 
    public override float GetCost(AIAgent agent)
    {
        float hpPercentage = agent.GetTank().CurrentHp / agent.GetTank().MaxHp;

        return Mathf.Clamp01(hpPercentage)+Vector3.Distance(agent.transform.position,agent.BasePosition)/5f;
    }

    public override bool OnUpdate(AIAgent agent)
    {
        agent.Attack();
        agent.GetTank().Move(agent.BasePosition);
        if (Vector3.Distance(agent.transform.position, agent.BasePosition) < 0.3f)
        {
            return false;
        }

        return true;
    }
}

public class AttackGOAPActon : GOAPAction
{
    private int lastScore;
    public AttackGOAPActon()
    {
        effectState = new WorldState();
        effectState.SetState("EnemyDead", true);
        preState = new WorldState();
        preState.SetState("Healthy", true);
    }

    public override void OnEnter(AIAgent agent)
    {
        lastScore = GetCurrentScore(agent);
    }

    public int GetCurrentScore(AIAgent agent)
    {
        return (int)BattleBlackboard.Instance.Information[EBlackboardInformationType.playerPoints][agent.GetTank().PlayerID];
    }
    public override bool OnUpdate(AIAgent agent)
    {
        if (lastScore + 1 == GetCurrentScore(agent))
        {
            return false;
        }
        agent.Attack();
        if (!agent.AimAtEnemy())
        {
            agent.GetTank()
                .Move((Vector3)BattleBlackboard.Instance.Information[EBlackboardInformationType.playerPosition][
                    agent.GetEnemyID()]);
        }
        else
        {
            agent.GetTank().Move(agent.transform.position);
        }

        return true;
    }

    public override float GetCost(AIAgent agent)
    {
        float hpPercentage = agent.GetTank().CurrentHp / agent.GetTank().MaxHp;
        return 1 - Mathf.Clamp01(hpPercentage) + Vector3.Distance(agent.transform.position,
            (Vector3)BattleBlackboard.Instance.Information[EBlackboardInformationType.playerPosition][agent.GetEnemyID()])/5f;
    }
}

public class StayAtBaseGOAPAction : GOAPAction
{
    public StayAtBaseGOAPAction()
    {
        effectState = new WorldState();
        effectState.SetState("FullHP",true);
        effectState.SetState("Healthy",true);
        preState = new WorldState();
        preState.SetState("Recovering", true);
    } 
    public override float GetCost(AIAgent agent)
    {
        return 0f;
    }
    public override bool OnUpdate(AIAgent agent)
    {
        agent.Attack();
        if (agent.GetTank().CurrentHp / agent.GetTank().MaxHp >= 1)
        {
            return false;
        }
        return true;
    }
}