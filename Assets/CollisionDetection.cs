using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Åö×²¼ì²â
/// </summary>
public class CollisionDetection : MonoBehaviour
{
   public bool IsCollision=false;
    private Transform Target;
    public void SetCollisionTarget(Transform taget)
    {
        Target = taget;
    }
  
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Åö×²£º" + other.transform.name);
        if (Target == other.transform)
        IsCollision = true;
    }
   
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Àë¿ªÅö×²£º" + other.transform.name);
        if (Target == other.transform)
            IsCollision = false;
    }
}
