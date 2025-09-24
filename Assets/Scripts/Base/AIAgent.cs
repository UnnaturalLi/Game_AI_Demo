using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public abstract class AIAgent : TankAgent
{
    public Vector3 BasePosition;
    protected Tank _tank;

    public Tank GetTank()
    {
        if (_tank == null)
        {
            _tank = GetComponent<Tank>();
        }
        return _tank;
    }
    public int GetEnemyID()
    {
        int id = -1;
        foreach (var VARIABLE in BattleBlackboard.Instance.Information[EBlackboardInformationType.playerPosition])
        {
            if (VARIABLE.Key != GetTank().PlayerID)
            {
                id= VARIABLE.Key;
                break;
            }
        }
        return id;
    }

    public void Attack()
    {
        if (AimAtEnemy())
        {
            _tank.Shoot();
        }
        else
        {
            _tank.RotateBarrel((Vector3)BattleBlackboard.Instance.Information[EBlackboardInformationType.playerPosition][GetEnemyID()]);
        }
    }

    public bool CanSeeEnemy()
    {
        var dic = BattleBlackboard.Instance.Information[EBlackboardInformationType.playerPosition];
        int enemyID=-1;
        foreach (var pair in dic)
        {
            if (pair.Key == GetTank().PlayerID)
            {
                continue;
            }
            enemyID=pair.Key;
        }

        if (enemyID != -1)
        {
            RaycastHit hitInfo;
            var dir = ((Vector3)dic[enemyID] - transform.position);
            dir.y = 0;
            Physics.Raycast(transform.position, dir.normalized, out hitInfo, 100f);
            if (hitInfo.collider.tag=="Player"&& hitInfo.collider.GetComponent<Tank>().PlayerID==enemyID)
            {
                return true;
            }
        }
        return false;
    }

    public bool AimAtEnemy()
    {
        var dic = BattleBlackboard.Instance.Information[EBlackboardInformationType.playerPosition];
        int enemyID = -1;
        foreach (var pair in dic)
        {
            if (pair.Key == GetTank().PlayerID)
            {
                continue;
            }
            enemyID = pair.Key;
        }

        if (enemyID != -1)
        {
            RaycastHit hitInfo;
            var origin = _tank.BulletReleasePoint.position;
            var direction = _tank.BulletReleasePoint.forward;
            if (Physics.Raycast(origin, direction, out hitInfo, 100f))
            {
                if (hitInfo.collider.tag == "Player" && hitInfo.collider.GetComponent<Tank>().PlayerID == enemyID)
                {
                    return true;
                }
            }
        }
        return false;
    }
    
    protected virtual void Start()
    {
        
    }

    protected virtual void Awake()
    {
        _tank = GetComponent<Tank>();
    }
}