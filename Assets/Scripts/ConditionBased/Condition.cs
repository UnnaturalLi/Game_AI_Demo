using System;

[Serializable]
public abstract class Condition
{
    public abstract bool GetCondition(AIAgent agent);   
}