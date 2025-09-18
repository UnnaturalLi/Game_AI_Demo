public abstract class ActionNode : Node
{
    private int currentState = 0;
    protected override ENodeState OnUpdate(AIAgent agent)
    {
        ENodeState state;
        if (currentState == 0)
        {
            state= OnEnter(agent);
        }
        else
        {
            state= OnExecute(agent);
        }
        if (state == ENodeState.Finished)
        {
            OnReset(agent);
        }
        return state;
    }

    protected override void OnReset(AIAgent agent)
    {
        currentState = 0;
    }

    protected virtual ENodeState OnEnter(AIAgent agent)
    {
        currentState++;
        return ENodeState.Running;
    }

    protected abstract ENodeState OnExecute(AIAgent agent);
    public override string GetDescription()
    {
        return Description;
    }
}