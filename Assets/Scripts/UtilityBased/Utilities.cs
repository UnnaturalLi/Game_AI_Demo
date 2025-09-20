using UnityEngine;
public class EnemyAtBaseUtility : Utility
{
    protected override float OnCalculation(AIAgent agent)
    {
        if (Vector3.Distance(
                (Vector3)BattleBlackboard.Instance.Information[EBlackboardInformationType.playerPosition][agent.GetEnemyID()],
                -agent.BasePosition) < 1f)
        {
            return 1f;
        }

        return 0f;
    }
}

public class UPUtility : Utility
{
    protected override float OnCalculation(AIAgent agent)
    {
        return agent.GetTank()._CurrentHp / agent.GetTank().MaxHp;
    }
}
public class FarawayFromEnemyUtility : Utility
{
    public float maxDistance;
    protected override float OnCalculation(AIAgent agent)
    {
        return Mathf.Clamp(Vector3.Distance((Vector3)BattleBlackboard.Instance.Information[EBlackboardInformationType.playerPosition][agent.GetEnemyID()],agent.transform.position)/maxDistance,0,1);
    }
}

public class CloseToEnemyUtility : Utility
{
    public float maxDistance;
    protected override float OnCalculation(AIAgent agent)
    {
        return 1-Mathf.Clamp(Vector3.Distance((Vector3)BattleBlackboard.Instance.Information[EBlackboardInformationType.playerPosition][agent.GetEnemyID()],agent.transform.position)/maxDistance,0,1);
    }
}