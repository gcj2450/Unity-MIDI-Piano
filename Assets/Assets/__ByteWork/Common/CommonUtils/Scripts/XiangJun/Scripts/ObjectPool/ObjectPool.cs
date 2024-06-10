using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

///<summary>
///<para>Copyright (C) 2010 北京暴风魔镜科技有限公司版权所有</para>
/// <para>文 件 名： ObjectPool.cs</para>
/// <para>文件功能： ObjectPool类</para>
/// <para>开发部门： 暴风魔镜</para>
/// <para>创 建 人： 刘享军</para>
/// <para>电子邮件： </para>
/// <para>创建日期：2015-10-20</para>
/// <para>修 改 人：</para>
/// <para>修改日期：</para>
/// <para>备    注：</para>
/// </summary>
public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    /// <summary>
    /// The entries
    /// </summary>
    public List<ObjectPoolEntry> Entries;

    /// <summary>
    /// The container object
    /// </summary>
    protected GameObject ContainerObject;

    [HideInInspector]
    public List<Pool> Pools;
    private bool _initialized;

    private List<DelayedPoolObject> _delayedPoolObjects;

    /// <summary>
    /// Awakes this instance.
    /// </summary>
    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start()
    {
        ContainerObject = new GameObject("ObjectPool");
        _delayedPoolObjects = new List<DelayedPoolObject>();
        //Loop through the object prefabs and make a new list for each one.
        //We do this because the pool can only support prefabs set to it in the editor,
        //so we can assume the lists of pooled objects are in the same order as object prefabs in the array
        Pools = new List<Pool>();
        for (int i = 0; i < Entries.Count; i++)
        {
            var entry = Entries[i];
            AddNewPool();
            InitializeEntry(entry);
        }

        _initialized = true;
    }

    public void AddEntry(ObjectPoolEntry entry)
    {
        Entries.Add(entry);
        if (_initialized)
        {
            AddNewPool();
            InitializeEntry(entry);
        }
    }

    private void AddNewPool()
    {
        Pools.Add(new Pool { Objects = new List<GameObject>() });
    }

    private void InitializeEntry(ObjectPoolEntry entry)
    {
        for (int j = 0; j < entry.PreInstantiateCount; j++)
        {
            CreateNewPoolObject(entry);
        }
    }

    public bool ExistEntry(string prefabName)
    {
        return Entries.Exists(e => e.Prefab.name.IsEqualIgnoreCase(prefabName));
    }

    /// <summary>
    /// Creates the new pool object.
    /// </summary>
    /// <param name="entry">The entry.</param>
    private void CreateNewPoolObject(ObjectPoolEntry entry)
    {
        var newObj = Instantiate(entry.Prefab);
        newObj.name = entry.Prefab.name;
        PoolObject(newObj, true);
    }

    void Update()
    {
        for (int i = _delayedPoolObjects.Count - 1; i >= 0; i--)
        {
            var d = _delayedPoolObjects[i];
            if (d.PoolItem)
            {
                if (d.Duration > 0.0f)
                {
                    d.Duration -= Time.deltaTime;

                    if (d.Duration <= 0.0f)
                    {
                        PoolObject(d.PoolItem, false);
                        _delayedPoolObjects.Remove(d);
                    }
                }
            }
            else
            {
                _delayedPoolObjects.Remove(d);
            }
        }
    }

    public GameObject Get(string objectName)
    {
        return Get(objectName, false);
    }

    public GameObject Get(string objectName, bool onlyPooled)
    {
        for (int i = 0; i < Entries.Count; i++)
        {
            var entry = Entries[i];
            if (!entry.Prefab.name.IsEqualIgnoreCase(objectName))
            {
                continue;
            }

            var pool = Pools[i];

            if (pool.Objects.Count > 0)
            {
                var poolObject = pool.Objects[0];
                pool.Objects.Remove(poolObject);

                poolObject.transform.parent = null;
                poolObject.SetActive(true);
                var poolItem = poolObject.GetComponent<ObjectPoolItem>();
                if (poolItem)
                {
                    poolItem.Initialize();
                }

                poolObject.transform.rotation = Quaternion.identity;
                poolObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                return poolObject;
            }

            if (entry.Buffer > 0)
            {
                entry.Buffer--;
                CreateNewPoolObject(entry);
                return Get(objectName, onlyPooled);
            }

            if (!onlyPooled)
            {
                return Instantiate(entry.Prefab);
            }
        }

        return null;
    }

    public bool PoolObject(GameObject obj)
    {
        return PoolObject(obj, false);
    }

    public bool PoolObject(GameObject obj, float t)
    {
        if (t <= 0.0f)
        {
            return PoolObject(obj, false);
        }

        if (!IsPoolObject(obj))
        {
            return false;
        }

        _delayedPoolObjects.Add(new DelayedPoolObject(obj, t));
        return true;
    }

    public bool IsPoolObject(GameObject obj)
    {
        return Entries.Any(e => e.Prefab.name.IsEqualIgnoreCase(obj.name));
    }

    private bool PoolObject(GameObject obj, bool forInitialize)
    {
        for (int i = 0; i < Entries.Count; i++)
        {
            if (!Entries[i].Prefab.name.IsEqualIgnoreCase(obj.name))
            {
                continue;
            }

            obj.transform.parent = ContainerObject.transform;
            obj.transform.localPosition = Vector2.zero;
            obj.transform.localRotation = Quaternion.identity;

            if (!forInitialize)
            {
                var poolItem = obj.GetComponent<ObjectPoolItem>();
                if (poolItem)
                {
                    poolItem.BackToPool();
                }
            }

            obj.SetActive(false);
            Pools[i].Objects.Add(obj);
            return true;
        }

        return false;
    }

    public IEnumerator BackToPool(GameObject o, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        BackToPool(o);
    }

    public void BackToPool(GameObject o)
    {
        o.transform.parent = null;
        if (!PoolObject(o))
        {
            Destroy(o);
        }
    }

    [Serializable]
    public class ObjectPoolEntry
    {
        public GameObject Prefab;
        public int PreInstantiateCount;
        public int Buffer;
    }

    private class DelayedPoolObject
    {
        public GameObject PoolItem { get; private set; }
        public float Duration { get; set; }

        public DelayedPoolObject(GameObject poolItem, float duration)
        {
            this.PoolItem = poolItem;
            this.Duration = duration;
        }
    }

    public class Pool
    {
        public List<GameObject> Objects;
    }
}