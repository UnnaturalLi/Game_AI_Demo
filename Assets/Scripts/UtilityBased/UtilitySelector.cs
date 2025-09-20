using System.Collections.Generic;
using UnityEngine;

public class UtilitySelector
{
    public List<Utility> utilities = new List<Utility>();
    public void AddUtility(params Utility[] utility)
    {
        utilities.AddRange(utility);
    }
    public int Select(AIAgent agent)
    {
        int selected = -1;
        float maxResult = 0;
        for (int i = 0; i < utilities.Count; i++)
        {
            
            if (utilities[i].Calculate(agent) > maxResult)
            {
                selected = i;
                maxResult = utilities[i].GetLastResult();
            }
        }
        return selected;
    }
}