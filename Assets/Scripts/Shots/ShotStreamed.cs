using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Child of Shot class that fires bullets in a bullet-hell "stream"
/// </summary>
public class ShotStreamed : Shot
{
    // fields
    [SerializeField] private bool triggered;
    [SerializeField] private int maxCount = 40;
    [SerializeField] private float minBulletSpeed = 5f;

    private Vector2 direction;
    
    // methods
    /// <summary>
    /// Shoot bullets at a set rate at a specified direction.
    /// </summary>
    /// <param name="dir"></param>
    public override void Shooting(Vector2 dir)
    {
        direction = dir;
        if(!triggered) StartCoroutine(StreamedShooting());
    }

    /// <summary>
    /// continue firing bullets at an interval until the max count is hit.
    /// </summary>
    /// <returns></returns>
    private IEnumerator StreamedShooting()
    {
        triggered = true;
        for (int i = 0; i < maxCount; i++)
        {
            SpawnBullet(direction, ((bulletSpeed - minBulletSpeed)/maxCount * i) + minBulletSpeed, transform, bulletPool);
            
            yield return new WaitForSeconds(shotInterval);
        }
        StopCoroutine(StreamedShooting());
    }
    
    public override void OnRecycle()
    {
        // reset bool
        triggered = false;
    }
}
