using UnityEngine;

public abstract class TankAgent : MonoBehaviour
{
    public abstract string GetDescription();
    public virtual void StartAgent()
    {
    }
    public void ReSpawn(){OnRespawn();}

    public virtual void OnRespawn()
    {
        
    }
}