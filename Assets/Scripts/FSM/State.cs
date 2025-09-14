using System;
using System.Collections.Generic;

[Serializable]
public class Transition
{
    public Condition condition;
    public string targetState;  
}
[Serializable]
public abstract class State
{
    public string name;
    public List<Transition> transitions;
    public string StateDescription;
    public virtual void OnEnter(AIAgent agent){}
    public virtual void OnExit(AIAgent agent){}

    public virtual State OnUpdate(AIAgent agent,StateMachine stateMachine)
    {
        foreach (var transition in transitions)
        {
            if (transition.condition != null && transition.condition.GetCondition(agent))
            {
                
                return stateMachine.GetState(transition.targetState);
            }
        }
        return this;
    }
}
