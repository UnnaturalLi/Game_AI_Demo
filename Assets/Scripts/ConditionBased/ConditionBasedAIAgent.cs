using System;
using UnityEngine;


public class ConditionBasedAIAgent :AIAgent
{
    private string _Description="";
    private AimAtEnemyCondition _aimCondition;
    private CanSeeEnemyCondition _canSeeCondition;
    private DistanceTooCloseCondition _distanceTooCloseCondition;
    private HpTooLowCondition _hpTooLowCondition;
    protected override void Start()
    {
        base.Start();
        _aimCondition = new AimAtEnemyCondition();
        _canSeeCondition = new CanSeeEnemyCondition();
        _distanceTooCloseCondition=new DistanceTooCloseCondition();
        _hpTooLowCondition=new HpTooLowCondition();
    }

    public void Stop()
    {
        _tank.Move(transform.position);
    }
    private void Update()
    {
        if (_hpTooLowCondition.GetCondition(this))
        {
            BasePosition.y=transform.position.y;
            _tank.Move(BasePosition);
            _Description = "Moving back to base";
        }else if (_distanceTooCloseCondition.GetCondition(this))
        {
            var dir = (transform.position -
                       (Vector3)BattleBlackboard.Instance.Information[EBlackboardInformationType.playerPosition][GetEnemyID()]);
            dir.y = 0;
            _tank.Move(dir.normalized);
            _Description = "Moving back";
        }
        else
        {
            _tank.Move(
                (Vector3)BattleBlackboard.Instance.Information[EBlackboardInformationType.playerPosition][GetEnemyID()]);
            _Description = "Moving towards Enemy";
        }
        
        if (_aimCondition.GetCondition(this))
        { 
            _tank.Shoot();
            Invoke("Stop",0.1f);
            _Description += "\nShooting";
        }
        else
        {
            _tank.RotateBarrel(
                (Vector3)BattleBlackboard.Instance.Information[EBlackboardInformationType.playerPosition][GetEnemyID()]);
            _Description += "\nRotating Towards Enemy";
        }
    }


    public override string GetDescription()
    {
        return _Description;
    }
}