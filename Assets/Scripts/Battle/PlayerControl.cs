using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Tank))]
public class PlayerControl : TankAgent
{
    private Tank _Tank;



    public void Shoot()
    {
        _Tank.Shoot();
    }
    public void Rotate()
    {
        var position = InputManager.Instance.mouseWorldPosition;
        position.y= transform.position.y;
        _Tank.RotateBarrel(position);
    }
    public void Move()
    {
        var position = InputManager.Instance.mouseWorldPosition;
        position.y= transform.position.y;
        _Tank.Move(position);
    }
    private void Start()
    {
        _Tank = GetComponent<Tank>();
        InputManager.Instance.RegisterMouseClick(0,Shoot);
        InputManager.Instance.RegisterMouseClick(1,Move);
    }

    private void Update()
    {
        Rotate();
    }

    public override string GetDescription()
    {
        return "Controled Manually";
    }
}
