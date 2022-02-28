using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ײ���
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
        Debug.Log("��ײ��" + other.transform.name);
        if (Target == other.transform)
        IsCollision = true;
    }
   
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("�뿪��ײ��" + other.transform.name);
        if (Target == other.transform)
            IsCollision = false;
    }
}
