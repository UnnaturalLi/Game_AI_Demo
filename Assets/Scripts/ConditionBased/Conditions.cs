using System;
using UnityEngine;
[Serializable]
public class TrueCondition : Condition
{
    public override bool GetCondition(AIAgent agent)
    {
        return true;
    }
}
[Serializable]
public class FalseCondition : Condition
{
    public override bool GetCondition(AIAgent agent)
    {
        return false;
    }
}
[Serializable]
public class AndCondition : Condition
{
    public Condition a;
    public Condition b;
    public override bool GetCondition(AIAgent agent)
    {
        return a.GetCondition( agent) && b.GetCondition( agent);
    }
}
[Serializable]
public class OrCondition : Condition
{
    public Condition a;
    public Condition b;
    public override bool GetCondition(AIAgent agent)
    {
        return a.GetCondition( agent) || b.GetCondition( agent);
    }
}
[Serializable]
public class XorCondition : Condition
{
    public Condition a;
    public Condition b;
    public override bool GetCondition(AIAgent agent)
    {
        return a.GetCondition( agent) ^ b.GetCondition( agent);
    }
}
[Serializable]
public class NotCondition : Condition
{
    public Condition condition;
    public override bool GetCondition(AIAgent agent)
    {
        return !condition.GetCondition( agent);
    }
}
[Serializable]
public class CanSeeEnemyCondition : Condition
{
    public override bool GetCondition(AIAgent agent)
    {
        return agent.CanSeeEnemy();
    }
}
[Serializable]
public class AimAtEnemyCondition : Condition
{
    public override bool GetCondition(AIAgent agent)
    {
        return agent.AimAtEnemy();
    }
}
[Serializable]
public class DistanceTooCloseCondition : Condition
{
    public float distance;
    public override bool GetCondition(AIAgent agent)
    {
        return Vector3.Distance((Vector3)BattleBlackboard.Instance.Information[EBlackboardInformationType.playerPosition][agent.GetEnemyID()],agent.transform.position)<distance;
    }
}
[Serializable]
public class HpTooLowCondition : Condition
{
    public float percentage;

    public override bool GetCondition(AIAgent agent)
    {
        var tank = agent.GetComponent<Tank>();
        Debug.Log(tank._CurrentHp < tank.MaxHp * percentage);
        return tank._CurrentHp < tank.MaxHp * percentage;
    }
}
