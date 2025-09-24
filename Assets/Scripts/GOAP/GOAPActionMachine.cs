using System.Collections.Generic;

public class GOAPActionMachine
{
    public Queue<GOAPAction> actions;
    private GOAPAction _currentAction;
    public void SetActions(Queue<GOAPAction> actions)
    {
        _currentAction = null;
        this.actions = actions;
        
    }

    public bool Update(AIAgent agent)
    {
        if (_currentAction == null&&actions!=null&&actions.Count > 0)
        {
            _currentAction = actions?.Dequeue();
            _currentAction?.OnEnter(agent);
        }

        if (_currentAction != null)
        {
            bool running=_currentAction.Update(agent);
            if (!running)
            {
                _currentAction=null;
                actions.TryDequeue(out _currentAction);
                if (_currentAction != null)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    public void Clear()
    {
        _currentAction=null;
        actions=null;
    }
}