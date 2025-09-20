using System;
using UnityEngine;
using System.Collections.Generic;
public class UtilityBasedAIAgent : AIAgent
{
    public UtilitySelector selector;
    private bool _running=false;
    public void LoadUtilitySelector()
    {
        selector = new UtilitySelector();

        // ---- Utility 0: 进攻行为 ----
        var attackComposite = new WeightedCompositeUtility();
        attackComposite.Description = "Attack Enemy";

        attackComposite.utilities = new List<Utility>
        {
            new CloseToEnemyUtility { maxDistance = 8f },   // 越近越好
            new UPUtility()                                // 血量越多越倾向进攻
        };

        attackComposite.SetWeight(new List<float>
        {
            0.7f,  // 更看重接近敌人的情况
            0.3f   // 血量次要
        });

        // ---- Utility 1: 防守行为 ----
        var defendComposite = new WeightedCompositeUtility();
        defendComposite.Description = "Defend Base";

        defendComposite.utilities = new List<Utility>
        {
            new EnemyAtBaseUtility(),                      // 敌人是否在我家基地
            new FarawayFromEnemyUtility { maxDistance = 10f }, // 离敌人远
            new InvertUtility{utility =new UPUtility()},
            new CloseToEnemyUtility(){maxDistance = 3}
        };

        defendComposite.SetWeight(new List<float>
        {
            0.6f,  // 基地优先级更高
            0.2f,  // 远离敌人稍微考虑
            1.0f,  // 新增：血量权重（高）
            2f
        });

        // 加入选择器
        selector.AddUtility(attackComposite, defendComposite);
    }

    protected override void Start()
    {
        LoadUtilitySelector();
        _running=true;
    }

    private void Update()
    {
        if (!_running)
        {
            return;
        }

        int decision = selector.Select(this);
        switch (decision)
        {
            case 0:
                _tank.Move((Vector3)BattleBlackboard.Instance.Information[EBlackboardInformationType.playerPosition][GetEnemyID()]);
                break;
            case 1:
                _tank.Move(BasePosition);
                break;
        }

        if (AimAtEnemy())
        {
            _tank.Shoot();
        }
        else
        {
            _tank.RotateBarrel((Vector3)BattleBlackboard.Instance.Information[EBlackboardInformationType.playerPosition][GetEnemyID()]);
        }
    }

    public override string GetDescription()
    {
        if (selector == null)
        {
            return "";
        }
        string result = "";
        foreach (var VARIABLE in selector.utilities)
        {
            result = string.Concat(result, $"{VARIABLE.Description} {VARIABLE.GetLastResult().ToString("F2")}");
        }
        return result;
    }
}