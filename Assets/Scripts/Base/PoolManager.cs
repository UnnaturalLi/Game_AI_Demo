using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class PrefabEntry
{
    public string key;
    public GameObject prefab;
}
public class PoolManager : MonoSingletonBase<PoolManager>
{
    [SerializeField] private List<PrefabEntry> prefabList = new List<PrefabEntry>();
    public Dictionary<string, GameObject> Prefabs { get; private set; }
    private Dictionary<string, List<GameObject>> pool = new Dictionary<string, List<GameObject>>();
    void Awake()
    {
        base.Awake();
        Prefabs = new Dictionary<string, GameObject>();
        foreach (var entry in prefabList)
        {
            if (!Prefabs.ContainsKey(entry.key))
            {
                Prefabs.Add(entry.key, entry.prefab);
                pool.Add(entry.key, new List<GameObject>());
            }
        }
    }
    void Start()
    {
        
    }
    void Update()
    {
        
    }

    public GameObject GetGameObject(string key,params object[] initArgs)
    {
        if (pool.ContainsKey(key))
        {
            GameObject obj=null;
            for (int i = 0; i < pool[key].Count; i++)
            {
                if (pool[key][i].activeSelf == false)
                {
                    obj = pool[key][i];
                }
            }
            if (obj == null)
            {
                obj=Instantiate(Prefabs[key], Vector3.zero, Quaternion.identity);
                pool[key].Add(obj);
            }
            obj.SetActive(true);
            if (obj.GetComponent<PoolObjectBase>().Init(initArgs))
            {
                return obj;    
            }
            else
            {
                obj.SetActive(false);
            }
        }
        return null;
    }

    public void ReturnGameObject(string key, GameObject go)
    {
        go.SetActive(false);
    }
}
