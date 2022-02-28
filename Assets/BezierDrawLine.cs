using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class BezierDrawLine : MonoBehaviour
{
    public bool DrawSphere;
    private bool OldDrawSphere;
    public List<Transform> wayPoint = new List<Transform>();   //·����Ϣ����β��ʾ�����յ㣬�м�Ϊ���n��ƫ�Ƶ㣩
    public int pointCount = 100;     //�����ϵ�ĸ���
    [HideInInspector]
    public List<Vector3> linePointList;
    [Range(0, 1)]
    public float _time = 0.01f;        //�������˶����
    public Transform player;           //�˶�����
    public Transform targetTransform;  //PlayĿ������
    private bool isMove = false;
    private float _curTimer = 0.0f;   //��ʱ
    private int lineItem = 1;          //Ŀ������


    void Awake()
    {
        //Init();
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(DrawSphere!=OldDrawSphere)
        {
            for(int i=0;i< wayPoint.Count;i++)
            {
                wayPoint[i].gameObject.SetActive(DrawSphere);
                OldDrawSphere = DrawSphere;
            }
        }
        //if (!isMove) return;
        //_curTimer += Time.deltaTime;
        //if (_curTimer > _time)
        //{
        //    _curTimer = 0;
        //    if (targetTransform)
        //        player.LookAt(targetTransform);
        //    else
        //        player.LookAt(linePointList[lineItem]);
        //    player.localPosition = Vector3.Lerp(linePointList[lineItem - 1], linePointList[lineItem], 1f);
        //    lineItem++;
        //    if (lineItem >= linePointList.Count)
        //        lineItem = 1;
       // }
    }
    // ����
    Vector3 Bezier(Vector3 p0, Vector3 p1, float t)
    {
        return (1 - t) * p0 + t * p1;
    }
    // ��������
    Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        Vector3 p0p1 = (1 - t) * p0 + t * p1;
        Vector3 p1p2 = (1 - t) * p1 + t * p2;
        Vector3 result = (1 - t) * p0p1 + t * p1p2;
        return result;
    }
    // ��������
    Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 result;
        Vector3 p0p1 = (1 - t) * p0 + t * p1;
        Vector3 p1p2 = (1 - t) * p1 + t * p2;
        Vector3 p2p3 = (1 - t) * p2 + t * p3;
        Vector3 p0p1p2 = (1 - t) * p0p1 + t * p1p2;
        Vector3 p1p2p3 = (1 - t) * p1p2 + t * p2p3;
        result = (1 - t) * p0p1p2 + t * p1p2p3;
        return result;
    }

    // n�����ߣ��ݹ�ʵ��
    public Vector3 Bezier(float t, List<Vector3> p)
    {
        if (p.Count < 2)
            return p[0];
        List<Vector3> newp = new List<Vector3>();
        for (int i = 0; i < p.Count - 1; i++)
        {
            Debug.DrawLine(p[i], p[i + 1], Color.yellow);
            Vector3 p0p1 = (1 - t) * p[i] + t * p[i + 1];
            newp.Add(p0p1);
        }
        return Bezier(t, newp);
    }
    // transformת��Ϊvector3���ڵ��ò���ΪList<Vector3>��Bezier����
    public Vector3 Bezier(float t, List<Transform> p)
    {
        if (p.Count == 0)
            return default;
        if (p.Count < 2)
            return p[0].position;
        List<Vector3> newp = new List<Vector3>();
        for (int i = 0; i < p.Count; i++)
        {
            newp.Add(p[i].position);
        }
        //return Bezier(t, newp);
        return MyBezier(t, newp);
    }
    //��������
    public Vector3 MyBezier(float t, List<Vector3> p)
    {
        if (p.Count < 2)
            return p[0];
        List<Vector3> newp = new List<Vector3>();
        for (int i = 0; i < p.Count - 1; i++)
        {
            //Debug.DrawLine(p[i], p[i + 1], Color.yellow);
            Vector3 p0p1 = (1 - t) * p[i] + t * p[i + 1];
            newp.Add(p0p1);
        }
        return MyBezier(t, newp);
    }

    void Init()
    {
        linePointList = new List<Vector3>();
        for (int i = 0; i < pointCount; i++)
        {
            var point = Bezier(i / (float)pointCount, wayPoint);
            linePointList.Add(point);
        }
        if (linePointList.Count == pointCount)
            isMove = true;
        //Debug.LogError("isMove == " + isMove);
    }
    //��scene��ͼ��ʾ
    public void OnDrawGizmos()
    {
        Init();
        Gizmos.color = Color.yellow;
        //Gizmos.DrawLine()
        for (int i = 0; i < linePointList.Count - 1; i++)
        {
            //var point_1 = Bezier(i/(float)pointCount, wayPoint);
            //var point_2 = Bezier((i+1) / (float)pointCount, wayPoint);
            //���ֻ��߷�ʽ�Կ�
            //Gizmos.DrawLine(point_1, point_2);
            Debug.DrawLine(linePointList[i], linePointList[i + 1], Color.yellow);
        }

    }

}