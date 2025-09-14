using System;
using UnityEngine;

public class TrueCondition : Condition
{
    public override bool GetCondition()
    {
        return true;
    }
}

public class FalseCondition : Condition
{
    public override bool GetCondition()
    {
        return false;
    }
}

public class AndCondition : Condition
{
    private Condition a;
    private Condition b;
    public AndCondition(Condition a, Condition b)
    {
        this.a = a;
        this.b = b;
    }
    public override bool GetCondition()
    {
        return a.GetCondition() && b.GetCondition();
    }
}

public class OrCondition : Condition
{
    private Condition a;
    private Condition b;
    public OrCondition(Condition a, Condition b)
    {
        this.a = a;
        this.b = b;
    }
    public override bool GetCondition()
    {
        return a.GetCondition() || b.GetCondition();
    }
}

public class XorCondition : Condition
{
    private Condition a;
    private Condition b;
    public XorCondition(Condition a, Condition b)
    {
        this.a = a;
        this.b = b;
    }
    public override bool GetCondition()
    {
        return a.GetCondition() ^ b.GetCondition();
    }
}

public class NotCondition : Condition
{
    private Condition condition;
    public NotCondition(Condition condition)
    {
        this.condition = condition;
    }
    public override bool GetCondition()
    {
        return !condition.GetCondition();
    }
}

public class CanSeeEnemyCondition : Condition
{
    AIAgent agent;
    public CanSeeEnemyCondition(AIAgent agent)
    {
        this.agent = agent;
    }
    public override bool GetCondition()
    {
        return agent.CanSeeEnemy();
    }
}

public class AimAtEnemyCondition : Condition
{
    AIAgent agent;

    public AimAtEnemyCondition(AIAgent agent)
    {
        this.agent = agent;
    }
    public override bool GetCondition()
    {
        return agent.AimAtEnemy();
    }
}
[Serializable]
public class DistanceTooCloseCondition : Condition
{
    AIAgent agent;
    public float distance;
    public DistanceTooCloseCondition(AIAgent agent)
    {
        this.agent = agent;
    }
    public override bool GetCondition()
    {
        return Vector3.Distance((Vector3)BattleBlackboard.Instance.Information[EBlackboardInformationType.playerPosition][agent.GetEnemyID()],agent.transform.position)<distance;
    }
}
[Serializable]
public class HpTooLowCondition : Condition
{
    AIAgent agent;
    public float percentage;
    public HpTooLowCondition(AIAgent agent)
    {
        this.agent = agent;
    }

    public override bool GetCondition()
    {
        return agent.GetComponent<Tank>()._CurrentHp < agent.GetComponent<Tank>().MaxHp*percentage ;
    }
}
