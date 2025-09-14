using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Tank : MonoBehaviour
{
    private NavMeshAgent _Agent;
    private Transform _Barrel; 
    public Transform BulletReleasePoint;
    private float _Timer;
    public float MaxRotateAngularSpeed;
    public float CoolTime;
    public float MaxHp; 
    public float _CurrentHp;
    private Image _HpBar;
    public int PlayerID;
    public void HpChange(float hp)
    {
        _CurrentHp += hp;
        _CurrentHp = Mathf.Clamp(_CurrentHp, _CurrentHp, MaxHp);
        if (_HpBar)
        {
            _HpBar.fillAmount = _CurrentHp / MaxHp;
        }
    }
    public bool CanShoot
    {
        get
        {
            return _Timer >= CoolTime;
        }
    }

    public void ReSpawn(Vector3 pos)
    {
        _Agent.SetDestination(pos);
        transform.position = pos;
        _CurrentHp = MaxHp;
        if (_HpBar)
        {
            _HpBar.fillAmount = _CurrentHp / MaxHp;
        }
    }

    public void Move(Vector3 destination)
    {
        destination.x = Mathf.Clamp(destination.x,-5, 5);
        destination.z = Mathf.Clamp(destination.z, -5, 5);
        _Agent.SetDestination(destination);
    }

    public void RotateBarrel(Vector3 target)
    {
        Vector3 dir = target - _Barrel.position;
        dir.y = 0;
        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
            _Barrel.rotation = Quaternion.RotateTowards(
                _Barrel.rotation,
                targetRot,
                MaxRotateAngularSpeed * Time.deltaTime * Mathf.Rad2Deg
            );
        }
    }

    public void Shoot()
    {
        if (CanShoot)
        {
            if (PoolManager.Instance.GetGameObject("Bullet", BulletReleasePoint, _Barrel.forward,PlayerID))
            {
                _Timer = 0;
            }
        }
    }

    private void Awake()
    {
        _CurrentHp = MaxHp;
    }

    private void Start()
    {
        _Agent=GetComponent<NavMeshAgent>();
        _HpBar = transform.Find("Canvas").Find("HPBar").GetComponent<Image>();
        _Barrel=transform.Find("Barrel");
        BulletReleasePoint = _Barrel.Find("ReleasePoint");
        _Timer = 0;
    }
    private void Update()
    {
       _Timer+=Time.deltaTime;
       
    }
}
