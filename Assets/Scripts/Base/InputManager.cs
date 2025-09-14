using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoSingletonBase<InputManager>
{
    public Vector3 mouseWorldPosition;
    public UnityEvent Mouse0Clicked=new UnityEvent();
    public UnityEvent Mouse1Clicked=new UnityEvent();

    public void RegisterMouseClick(int button, UnityAction action)
    {
        if (button == 0)
        {
            Mouse0Clicked.AddListener(action);
        }else if (button == 1)
        {
            Mouse1Clicked.AddListener(action);
        }
    }
    void Update()
    {
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hitInfo);
        mouseWorldPosition = hitInfo.point;
        if (Input.GetMouseButtonDown(0))
        {
            Mouse0Clicked?.Invoke();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Mouse1Clicked?.Invoke();
        }
    }
}
