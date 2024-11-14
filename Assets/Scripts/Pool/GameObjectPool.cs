using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    public class GameObjectPool : MonoBehaviour
    {
        private static Dictionary<string, GameObjectPool> _pools;

        public static Dictionary<string, GameObjectPool> Pools
        {
            get
            {
                if (_pools == null)
                {
                    _pools = new Dictionary<string, GameObjectPool>();
                }

                return _pools;
            }
        }
        
        [SerializeField] private PoolableObject prefab;
        [SerializeField] private int maxCount;
        [SerializeField] private int initCount;

        private State _poolState = State.NotInitialized;
        
        private Stack<PoolableObject> poolQueue;

        public int MaxCount
        {
            get
            {
                return maxCount;
            }
        }

        private void Start()
        {
            Init(initCount, maxCount);
        }

        public PoolableObject Allocate()
        {
            if (poolQueue.Count > 0)
            {
                
                var obj = poolQueue.Pop();
                obj.gameObject.SetActive(true);
                return obj;
            }
            else
            {
                var obj = Instantiate(prefab);
                obj.pool = this;
                return obj;
            }
        }

        public bool Recycle(PoolableObject obj)
        {
            if (poolQueue.Count >= maxCount)
            {
                Destroy(obj);
                return false;
            }
            obj.OnRecycle();
            obj.gameObject.SetActive(false);
            poolQueue.Push(obj);
            return true;
        }
        
        /// <summary>
        /// Initialize a pool with a max size and initial set of objects
        /// </summary>
        /// <param name="initPoolSize"></param>
        /// <param name="maxPoolSize"></param>
        /// <returns></returns>
        public bool Init(int initPoolSize, int maxPoolSize)
        {
            if (_poolState == State.Initialized)
                return false;
            
            name = "Object Pool: " + prefab.name;
            if (Pools.ContainsKey(name))
            {
                //gameObject.SetActive(false);
                Destroy(gameObject);
            }
            else
            {
                Pools[name] = this;
            }
            
            if (initPoolSize > maxPoolSize || maxPoolSize <= 0)
            {
                return false;
            }
            
            maxCount = maxPoolSize;

            poolQueue = new Stack<PoolableObject>(maxPoolSize);
            
            for (int i = 0; i < initPoolSize; i++)
            {
                var obj = Instantiate(prefab);
                obj.pool = this;
                obj.gameObject.SetActive(false);
                poolQueue.Push(obj);
            }

            _poolState = State.Initialized;
            return true;
        }

        public bool Init(PoolableObject prefab, int initPoolSize, int maxPoolSize)
        {
            this.prefab = prefab;
            return Init(initPoolSize, maxPoolSize);
        }

        public static GameObjectPool Create(PoolableObject prefab, int initCount, int maxCount)
        {
            var pool = new GameObject().AddComponent<GameObjectPool>();
            if (pool.Init(prefab, initCount, maxCount))
            {
                return pool;
            }
            return null;
        }
        
        public static GameObjectPool CreateBasic(PoolableObject prefab)
        {
            return Create(prefab, 10, 200);
        }

        private enum State
        {
            NotInitialized,
            Initialized
        }
    }
}
