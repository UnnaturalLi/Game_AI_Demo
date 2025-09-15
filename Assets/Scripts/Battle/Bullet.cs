using UnityEngine;

public class Bullet : PoolObjectBase
{
    public float speed;
    private int shooter;
    public override bool Init(params object[] args)
    {
        var trans = args[0] as Transform;
        transform.position = trans.position;
        transform.rotation = trans.rotation;
        GetComponent<Rigidbody>().velocity=(Vector3)args[1]*speed;
        transform.GetChild(0).GetComponent<TrailRenderer>().Clear();
        shooter = (int)args[2];
        return true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player"&& other.GetComponent<Tank>().PlayerID!=shooter)
        {
         other.GetComponent<Tank>().HpChange(-1);
         PoolManager.Instance.ReturnGameObject("Bullet", gameObject);
        }else if (other.tag == "terrain")
        {
            PoolManager.Instance.ReturnGameObject("Bullet", gameObject);
        }
        
    }
}
