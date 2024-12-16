using Physics;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Shows the circle collision of the physics body.
/// </summary>
[CustomEditor(typeof(PhysicsBody))]
public class ColliderEditor : Editor
{
    private void OnSceneGUI()
    {
        PhysicsBody body = (PhysicsBody)target;
        Handles.color = Color.green;
        Handles.DrawWireDisc(body.transform.position, Vector3.forward, body.Radius);
    }
}
