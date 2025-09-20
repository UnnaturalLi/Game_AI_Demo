using System.Collections.Generic;
using UnityEngine;

public class Utility
{
    private float _lastResult;
    public string Description;
    public float Calculate(AIAgent agent)
    {
        _lastResult = OnCalculation(agent);
        return _lastResult;
    }

    public float GetLastResult()
    {
        return _lastResult;
    }
    protected virtual float OnCalculation(AIAgent agent)
    {
        return 0f;
    }
}

public class CompositeUtility : Utility
{
    public List<Utility> utilities;

    public void AddUtility(Utility utility)
    {
        utilities.Add(utility);
    }
}

public class AdditiveCompositeUtility : CompositeUtility
{
    protected override float OnCalculation(AIAgent agent)
    {
        float result = 0f;
        foreach (var utility in utilities)
        {
            result += utility.Calculate(agent);
        }

        return result;
    }
}

public class MultipleCompositUtility : CompositeUtility
{
    protected override float OnCalculation(AIAgent agent)
    {
        float result = 1;
        foreach (var utility in utilities)
        {
            result*= utility.Calculate(agent);
        }

        return result;
    }
}

public class WeightedCompositeUtility : CompositeUtility
{
    public List<float> weights;
    public bool SetWeight(List<float> weights)
    {
        if (weights == null || weights.Count != utilities.Count)
        {
            return false;
        }
        this.weights = weights;
        return true;
    }


    protected override float OnCalculation(AIAgent agent)
    {
        float result = 0f;
        for (int i = 0; i < weights.Count; i++)
        {
            result+= weights[i]*utilities[i].Calculate(agent);
        }

        return result;
    }
}

public class InvertUtility : Utility
{
    public Utility utility;
    protected override float OnCalculation(AIAgent agent)
    {
        return 1 - Mathf.Clamp01(utility.Calculate(agent));
    }
}