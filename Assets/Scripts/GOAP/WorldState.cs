using System.Collections.Generic;

public class WorldState
{
    private Dictionary<string, bool> _State;

    public WorldState()
    {
        _State = new Dictionary<string, bool>();
    }

    public void SetState(string stateName, bool state)
    {
        _State[stateName]=state;
    }

    public bool GetState(string stateName)
    {
        if (_State.ContainsKey(stateName))
        {
            return _State[stateName];
        }
        return false;
    }

    public void ClearState()
    {
        _State.Clear();
    }

    public WorldState Clone()
    {
        WorldState clone = new WorldState() { _State = new Dictionary<string, bool>() };
        foreach (KeyValuePair<string, bool> pair in _State)
        {
            clone._State.Add(pair.Key, pair.Value);
        }

        return clone;
    }

    public WorldState Merge(WorldState other)
    {
        foreach (var VARIABLE in other._State)
        {
            if (_State.ContainsKey(VARIABLE.Key))
            {
                _State[VARIABLE.Key] = VARIABLE.Value || _State[VARIABLE.Key];
            }
            else
            {
                _State.Add(VARIABLE.Key, VARIABLE.Value);
            }
        }
        return this;
    }

    public bool IsSatisfied(WorldState other)
    {
        bool satisfied = true;
        foreach (var VARIABLE in other._State)
        {
            if (_State.ContainsKey(VARIABLE.Key))
            {
                if (_State[VARIABLE.Key] != VARIABLE.Value)
                {
                    satisfied = false;
                    break;
                }
            }
            else
            {
                satisfied = false;
                break;
            }
        }

        return satisfied;
    }
}