using UnityEngine;

namespace Pool
{
    /// <summary>
    /// Represents a game object that can be pooled.
    /// </summary>
    public class PoolableObject : MonoBehaviour
    {
        // reference to the pool
        public GameObjectPool pool;

        // Logic called when object is recycled.
        public virtual void OnRecycle(){}
    }
}