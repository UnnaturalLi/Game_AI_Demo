using System;
using UnityEngine;
[Serializable]
public class MoveToEnemyState : State
{
    public override void OnEnter(AIAgent agent)
    {
    }

    public override State OnUpdate(AIAgent agent, StateMachine stateMachine)
    {
        agent.GetTank().Move((Vector3)BattleBlackboard.Instance.Information[EBlackboardInformationType.playerPosition][agent.GetEnemyID()]);
        if (agent.AimAtEnemy())
        {
            agent.GetTank().Shoot();
        }
        else
        {
            agent.GetTank().RotateBarrel(
                (Vector3)BattleBlackboard.Instance.Information[EBlackboardInformationType.playerPosition][agent.GetEnemyID()]);
        }
        return base.OnUpdate(agent, stateMachine);
    }
}
[Serializable]
public class RetreatToBaseState : State
{
    public override void OnEnter(AIAgent agent)
    {
        Vector3 basePos = agent.BasePosition;
        basePos.y = agent.transform.position.y;
        agent.GetTank().Move(basePos);
    }

    public override State OnUpdate(AIAgent agent, StateMachine stateMachine)
    {
        
        if (agent.AimAtEnemy())
        {
            agent.GetTank().Shoot();
        }
        else
        {
            agent.GetTank().RotateBarrel(
                (Vector3)BattleBlackboard.Instance.Information[EBlackboardInformationType.playerPosition][agent.GetEnemyID()]);
        }
        return base.OnUpdate(agent, stateMachine);
    }
}
[Serializable]
public class RetreatFromEnemyState : State
{
    public float retreatDistance;
    public override void OnEnter(AIAgent agent)
    {
        
    }

    public override State OnUpdate(AIAgent agent, StateMachine stateMachine)
    {
        Vector3 myPos = agent.transform.position;
        Vector3 enemyPos = (Vector3)BattleBlackboard.Instance.Information[EBlackboardInformationType.playerPosition][agent.GetEnemyID()];
        Vector3 dir = myPos - enemyPos;
        dir.y = 0;
        agent.GetTank().Move(myPos + dir.normalized * retreatDistance); 
        if (agent.AimAtEnemy())
        {
            agent.GetTank().Shoot();
        }
        else
        {
            agent.GetTank().RotateBarrel(
                (Vector3)BattleBlackboard.Instance.Information[EBlackboardInformationType.playerPosition][agent.GetEnemyID()]);
        }
        return base.OnUpdate(agent, stateMachine);
    }
}