using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    /// <summary>
    /// Class representing a pool of poolable game objects.
    /// </summary>
    public class GameObjectPool : MonoBehaviour
    {
        // field
        private static Dictionary<string, GameObjectPool> _pools;
        
        [SerializeField] private PoolableObject prefab;
        [SerializeField] private int maxCount;
        [SerializeField] private int initCount;

        private State _poolState = State.NotInitialized;
        
        private Stack<PoolableObject> poolQueue;
        
        // properties
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

        public int MaxCount
        {
            get
            {
                return maxCount;
            }
        }
        
        // methods
        // initialize the pool
        private void Start()
        {
            Init(initCount, maxCount);
        }

        // allocate an object from the queue of unused game objects.
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

        /// <summary>
        /// Add the object back into the pool so long as it does not exceed the max size of the pool
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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
            
            name = "Object Pool: " + prefab.poolID;
            if (Pools.ContainsKey(prefab.poolID))
            {
                //gameObject.SetActive(false);
                Destroy(gameObject);
            }
            else
            {
                Pools[prefab.poolID] = this;
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

        /// <summary>
        /// Init that also sets a specific prefab.
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="initPoolSize"></param>
        /// <param name="maxPoolSize"></param>
        /// <returns></returns>
        public bool Init(PoolableObject prefab, int initPoolSize, int maxPoolSize)
        {
            this.prefab = prefab;
            return Init(initPoolSize, maxPoolSize);
        }

        /// <summary>
        /// Create a game object pool.
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="initCount"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public static GameObjectPool Create(PoolableObject prefab, int initCount, int maxCount)
        {
            var pool = new GameObject().AddComponent<GameObjectPool>();
            if (pool.Init(prefab, initCount, maxCount))
            {
                return pool;
            }
            return null;
        }
        
        /// <summary>
        /// Create a game object pool with default settings.
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public static GameObjectPool CreateBasic(PoolableObject prefab)
        {
            return Create(prefab, 10, 200);
        }

        /// <summary>
        /// Indicates whether the pool is properly initialized yet.
        /// </summary>
        private enum State
        {
            NotInitialized,
            Initialized
        }
    }
}
