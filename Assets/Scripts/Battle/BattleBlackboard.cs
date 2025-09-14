using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EBlackboardInformationType
{
    playerPosition,
    playerHp,
    playerPoints,
}
public class BattleBlackboard : MonoSingletonBase<BattleBlackboard>
{
    public Dictionary<EBlackboardInformationType,Dictionary<int,object>> Information = new Dictionary<EBlackboardInformationType, Dictionary<int,object>>();

    protected override void Awake()
    {
        base.Awake();
        Information.Add(EBlackboardInformationType.playerHp, new Dictionary<int,object>());
        Information.Add(EBlackboardInformationType.playerPosition, new Dictionary<int,object>());
        Information.Add(EBlackboardInformationType.playerPoints, new Dictionary<int,object>());
    }
}
