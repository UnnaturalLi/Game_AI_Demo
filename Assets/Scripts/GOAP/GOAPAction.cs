public class GOAPAction
{
    public WorldState preState;
    public WorldState effectState;
    public bool Update(AIAgent agent)
    {
        return OnUpdate(agent);
    }

    public void Enter(AIAgent agent)
    {
        OnEnter(agent);
    }

    public virtual void OnEnter(AIAgent agent)
    {
        
    }
    public virtual bool OnUpdate(AIAgent agent)
    {
        return false;
    }
    public GOAPAction Clone()
    {
        var clone = new GOAPAction();
        clone.preState = preState.Clone();
        clone.effectState = effectState.Clone();
        return clone;
    }

    public virtual float GetCost(AIAgent agent)
    {
        return 0f;
    }
}