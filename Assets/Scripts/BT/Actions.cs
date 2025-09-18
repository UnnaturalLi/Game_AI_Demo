using UnityEngine;
using UnityEngine.UI;

public class BackToBaseActionNode : ActionNode
{
    protected override ENodeState OnExecute(AIAgent agent)
    {
        agent.GetTank().Move(agent.BasePosition);
        return ENodeState.Running;
    }
}

public class MoveToEnemyActionNode : ActionNode
{
    protected override ENodeState OnExecute(AIAgent agent)
    {
        agent.GetTank().Move((Vector3)BattleBlackboard.Instance.Information[EBlackboardInformationType.playerPosition][agent.GetEnemyID()]);
        return ENodeState.Running;
    }
}

public class RotateToEnemyActionNode : ActionNode
{
    protected override ENodeState OnExecute(AIAgent agent)
    {
        agent.GetTank().RotateBarrel((Vector3)BattleBlackboard.Instance.Information[EBlackboardInformationType.playerPosition][agent.GetEnemyID()]);
        return ENodeState.Running;
    }
}

public class ShootActionNode : ActionNode
{
    protected override ENodeState OnExecute(AIAgent agent)
    {
        agent.GetTank().Shoot();
        return ENodeState.Running;
    }
}