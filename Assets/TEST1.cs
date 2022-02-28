using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST1 : MonoBehaviour
{
    public bool IsDrawGizmos;
    public Transform Target;
    public Vector3 size;
    private Vector3[] PosArr = null;
    private List<Transform> ponints = new List<Transform>();
   
    Vector3 P0 { get { return transform.localToWorldMatrix.MultiplyPoint3x4(new Vector3(-size.x * 0.5f, -size.y * 0.5f, -size.z * 0.5f)); } }
    Vector3 P1 { get { return transform.localToWorldMatrix.MultiplyPoint3x4(new Vector3(size.x * 0.5f, -size.y * 0.5f, -size.z * 0.5f)); } }
    Vector3 P2 { get { return transform.localToWorldMatrix.MultiplyPoint3x4(new Vector3(size.x * 0.5f, -size.y * 0.5f, size.z * 0.5f)); } }
    Vector3 P3 { get { return transform.localToWorldMatrix.MultiplyPoint3x4(new Vector3(-size.x * 0.5f, -size.y * 0.5f, size.z * 0.5f)); } }

    Vector3 P4 { get { return transform.localToWorldMatrix.MultiplyPoint3x4(new Vector3(-size.x * 0.5f, size.y * 0.5f, -size.z * 0.5f)); } }
    Vector3 P5 { get { return transform.localToWorldMatrix.MultiplyPoint3x4(new Vector3(size.x * 0.5f, size.y * 0.5f, -size.z * 0.5f)); } }
    Vector3 P6 { get { return transform.localToWorldMatrix.MultiplyPoint3x4(new Vector3(size.x * 0.5f, size.y * 0.5f, size.z * 0.5f)); } }
    Vector3 P7 { get { return transform.localToWorldMatrix.MultiplyPoint3x4(new Vector3(-size.x * 0.5f, size.y * 0.5f, size.z * 0.5f)); } }
    // Start is called before the first frame update
    private void Awake()
    {
        //GameObject CollisionBoxRoot = new GameObject();
        //CollisionBoxRoot.name = "CollisionBoxRoot";
        //CollisionBoxRoot.transform.SetParent(transform);
        //CollisionBoxRoot.transform.localPosition = Vector3.zero;
        //CollisionBoxRoot.transform.localEulerAngles = Vector3.zero;
        //CollisionBoxRoot.transform.localScale = Vector3.one;
        //for (int i = 0; i < 8; i++)
        //{
        //    GameObject go = new GameObject();
        //    ponints.Add(go.transform);
        //   go.transform.SetParent(CollisionBoxRoot.transform);
        //}
        //ponints[0].position = P0;
        //ponints[1].position = P1;
        //ponints[2].position = P2;
        //ponints[3].position = P3;
        //ponints[4].position = P4;
        //ponints[5].position = P5;
        //ponints[6].position = P6;
        //ponints[7].position = P7;
        //PosArr = new Vector3[ponints.Count];
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Color[] colors = new Color[mesh.vertexCount];
        for(int i=0;i< colors.Length;i++)
        {
            colors[i] = Color.red;
        }
        mesh.colors = colors;

    }
    //private void Update()
    //{
    //    if(IsCollision(collisionPoint))
    //    {
    //        Debug.Log("碰撞");
    //    }
    //}
    public void UpdatePos()
    {
        for (int i = 0; i < PosArr.Length; i++)
        {
            PosArr[i] = ponints[i].position;
        }
    }
    public bool IsCollisionTest(Vector3 collisionPoint)
    {
        bool isColl = true;
     
        //下面4个条边
        isColl &= Vector3.Dot((collisionPoint - PosArr[0]).normalized, (PosArr[1] - PosArr[0]).normalized) >= 0;
        isColl &= Vector3.Dot((collisionPoint - PosArr[1]).normalized, (PosArr[2] - PosArr[1]).normalized) >= 0;
        isColl &= Vector3.Dot((collisionPoint - PosArr[2]).normalized, (PosArr[3] - PosArr[2]).normalized) >= 0;
        isColl &= Vector3.Dot((collisionPoint - PosArr[3]).normalized, (PosArr[0] - PosArr[3]).normalized) >= 0;

        //上面4条边
        isColl &= Vector3.Dot((collisionPoint - PosArr[4]).normalized, (PosArr[5] - PosArr[4]).normalized) >= 0;
        isColl &= Vector3.Dot((collisionPoint - PosArr[5]).normalized, (PosArr[6] - PosArr[5]).normalized) >= 0;
        isColl &= Vector3.Dot((collisionPoint - PosArr[6]).normalized, (PosArr[7] - PosArr[6]).normalized) >= 0;
        isColl &= Vector3.Dot((collisionPoint - PosArr[7]).normalized, (PosArr[4] - PosArr[7]).normalized) >= 0;

        //中间4条边，每条边需要判断2个方向
        isColl &= Vector3.Dot((collisionPoint - PosArr[0]).normalized, (PosArr[4] - PosArr[0]).normalized) >= 0;
        isColl &= Vector3.Dot((collisionPoint - PosArr[1]).normalized, (PosArr[5] - PosArr[1]).normalized) >= 0;
        isColl &= Vector3.Dot((collisionPoint - PosArr[2]).normalized, (PosArr[6] - PosArr[2]).normalized) >= 0;
        isColl &= Vector3.Dot((collisionPoint - PosArr[3]).normalized, (PosArr[7] - PosArr[3]).normalized) >= 0;

        isColl &= Vector3.Dot((collisionPoint - PosArr[4]).normalized, (PosArr[0] - PosArr[4]).normalized) >= 0;
        isColl &= Vector3.Dot((collisionPoint - PosArr[5]).normalized, (PosArr[1] - PosArr[5]).normalized) >= 0;
        isColl &= Vector3.Dot((collisionPoint - PosArr[6]).normalized, (PosArr[2] - PosArr[6]).normalized) >= 0;
        isColl &= Vector3.Dot((collisionPoint - PosArr[7]).normalized, (PosArr[3] - PosArr[7]).normalized) >= 0;


        return isColl;

    }
    public bool IsCollision(Vector3 collisionPoint)
    {
        bool isColl = true;
        for(int i=0;i< PosArr.Length; i++)
        {
            PosArr[i] = ponints[i].position;
        }
        //下面4个条边
        isColl &= Vector3.Dot((collisionPoint - ponints[0].position).normalized, (ponints[1].position - ponints[0].position).normalized) >= 0;
        isColl &= Vector3.Dot((collisionPoint - ponints[1].position).normalized, (ponints[2].position - ponints[1].position).normalized) >= 0;
        isColl &= Vector3.Dot((collisionPoint - ponints[2].position).normalized, (ponints[3].position - ponints[2].position).normalized) >= 0;
        isColl &= Vector3.Dot((collisionPoint - ponints[3].position).normalized, (ponints[0].position - ponints[3].position).normalized) >= 0;

        //上面4条边
        isColl &= Vector3.Dot((collisionPoint - ponints[4].position).normalized, (ponints[5].position - ponints[4].position).normalized) >= 0;
        isColl &= Vector3.Dot((collisionPoint - ponints[5].position).normalized, (ponints[6].position - ponints[5].position).normalized) >= 0;
        isColl &= Vector3.Dot((collisionPoint - ponints[6].position).normalized, (ponints[7].position - ponints[6].position).normalized) >= 0;
        isColl &= Vector3.Dot((collisionPoint - ponints[7].position).normalized, (ponints[4].position - ponints[7].position).normalized) >= 0;

        //中间4条边
        isColl &= Vector3.Dot((collisionPoint - ponints[0].position).normalized, (ponints[4].position - ponints[0].position).normalized) >= 0;
        isColl &= Vector3.Dot((collisionPoint - ponints[1].position).normalized, (ponints[5].position - ponints[1].position).normalized) >= 0;
        isColl &= Vector3.Dot((collisionPoint - ponints[2].position).normalized, (ponints[6].position - ponints[2].position).normalized) >= 0;
        isColl &= Vector3.Dot((collisionPoint - ponints[3].position).normalized, (ponints[7].position - ponints[3].position).normalized) >= 0;

        isColl &= Vector3.Dot((collisionPoint - ponints[4].position).normalized, (ponints[0].position - ponints[4].position).normalized) >= 0;
        isColl &= Vector3.Dot((collisionPoint - ponints[5].position).normalized, (ponints[1].position - ponints[5].position).normalized) >= 0;
        isColl &= Vector3.Dot((collisionPoint - ponints[6].position).normalized, (ponints[2].position - ponints[6].position).normalized) >= 0;
        isColl &= Vector3.Dot((collisionPoint - ponints[7].position).normalized, (ponints[3].position - ponints[7].position).normalized) >= 0;


        return isColl;
       
    }

    private void OnDrawGizmos()
    {
        if (!IsDrawGizmos)
            return;
        for(int i=0;i< ponints.Count;i++)
        {
            Gizmos.DrawSphere(ponints[i].position, 0.1f);
        }
  
    }
}
