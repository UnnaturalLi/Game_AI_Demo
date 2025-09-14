using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public float RecoverPerSec;
    public int PlayerID;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")&&other.GetComponent<Tank>().PlayerID == PlayerID)
        {
            other.GetComponent<Tank>().HpChange(RecoverPerSec*Time.deltaTime);
        }
    }
}
