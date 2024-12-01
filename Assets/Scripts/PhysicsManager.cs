using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhysicsManager : MonoBehaviour
{
    // Singleton
    private static PhysicsManager instance;
    public static PhysicsManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new GameObject().AddComponent<PhysicsManager>();
            }
            
            return instance;
        }
    }

    private HashSet<PhysicsBody> collidables = new HashSet<PhysicsBody>();

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Check a given collider against all listed obstacles.
    /// </summary>
    /// <param name="body"></param>
    /// <returns></returns>
    public bool CheckCollisions(PhysicsBody body)
    {
        
        bool didCollide = false;

        // Reset collisions then calculate collisions.
        body.ResetCollide();
        foreach (PhysicsBody collision in collidables)
        {
            // TODO: Implement some form of Layer checking to reduce collision checks.
            if (collision.isActiveAndEnabled)
            {
                collision.ResetCollide();
                didCollide = didCollide || TryCollision(body, collision);
            }
        }

        return didCollide;
    }

    /// <summary>
    /// Perform collisions based on collision mode.
    /// </summary>
    /// <param name="body1"></param>
    /// <param name="body2"></param>
    /// <returns></returns>
    private bool TryCollision(PhysicsBody body1, PhysicsBody body2)
    {
        // Check collisions
        bool collision = false;
        collision = CircleCollision(body1, body2, true);

        // If any collision occured, run each colliding object's onCollide script.
        if (collision) {
            body1.OnCollide(body2);
            body2.OnCollide(body1);
        }

        return collision;
    }

    /// <summary>
    /// Check collision between two sprites using Circle collision checking.
    /// If checkLayers is enabled, then using body1's layerMask, check if this collision is even allowed.
    /// </summary>
    /// <param name="body1"></param>
    /// <param name="body2"></param>
    /// <returns></returns>
    private bool CircleCollision(PhysicsBody body1, PhysicsBody body2, bool checkLayers = false)
    {
        if (checkLayers && (body1.ContactLayers & (1 << body2.gameObject.layer)) == 0)
        {
            return false;
        }
        float dstSq = (body1.Center - body2.Center).sqrMagnitude;
        float radiiSq = Mathf.Pow(body1.Radius + body2.Radius, 2);

        return dstSq <= radiiSq;
    }

    public bool AddPhysicsBody(PhysicsBody pb)
    {
        return collidables.Add(pb);
    }
}