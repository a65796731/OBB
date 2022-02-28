using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawLine : MonoBehaviour
{
    public Transform TargetPoint;
    public BezierDrawLine BezierDrawLine;
    private List<Vector3> linePointList;
    private List<bool> CollsionPoint;
    private LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        this.linePointList=  BezierDrawLine.linePointList;
        lineRenderer.positionCount = linePointList.Count;
        CollsionPoint = new List<bool>(linePointList.Count);
        for (int i=0;i< linePointList.Count;i++)
        {
            CollsionPoint.Add(false);
            lineRenderer.SetPosition(i, linePointList[i]);
        }

    }
    private void Update()
    {
        int CollIndex=0;
        for (int i = 0; i < linePointList.Count; i++)
        {
           float dis= Vector3.Distance(linePointList[i], TargetPoint.position);
            if(dis<1)
            {
                CollsionPoint[i]=true;
            }
            if(CollsionPoint[i]==true)
            {
                CollIndex++;
            }
        }
      bool IsSuccess=   CollIndex / linePointList.Count > 0.9f ? true : false;
      if(IsSuccess)
        {
              Debug.Log("³É¹¦");
        }
    }
    // Update is called once per frame

}
