using System;
using System.Collections;
using System.Collections.Generic;
using Pool;
using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    public GameObjectPool pool;

    public virtual void OnRecycle(){}
}