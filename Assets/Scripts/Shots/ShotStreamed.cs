using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotStreamed : Shot
{
    [SerializeField] private bool triggered;
    [SerializeField] private int maxCount = 40;
    [SerializeField] private float minBulletSpeed = 5f;

    private Vector2 direction;
    
    /// <summary>
    /// Shoot bullets at a set rate at a specified direction.
    /// </summary>
    /// <param name="dir"></param>
    public override void Shooting(Vector2 dir)
    {
        direction = dir;
        if(!triggered) StartCoroutine(StreamedShooting());
    }

    private IEnumerator StreamedShooting()
    {
        triggered = true;
        for (int i = 0; i < maxCount; i++)
        {
            var newBullet = bulletPool.Allocate();
            newBullet.transform.position = transform.position;
            var newBullet1 = (newBullet as Bullet);
            newBullet1.Direction = direction;
            newBullet1.speed = ((bulletSpeed - minBulletSpeed)/maxCount * i) + minBulletSpeed;
            newBullet1.affiliation = affiliation;
            newBullet1.owner = this;
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, -Vector3.forward);
            newBullet1.transform.rotation = rotation;
            
            yield return new WaitForSeconds(1f/shotRate);
        }
        StopCoroutine(StreamedShooting());
    }
    
    public override void OnRecycle()
    {
        triggered = false;
    }
}
