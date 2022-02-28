using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circular : MonoBehaviour
{
    public LineRenderer lineRenderer = null;
    public float _radius_length;
    public float _angle_speed;

    private float temp_angle;

    private Vector3 _pos_new;

    public Vector3 _center_pos;

    public bool _round_its_center;

    public int Angle;
    // Use this for initialization
    void Start()
    {
        if (_round_its_center)
        {
            _center_pos = transform.localPosition;
        }
        lineRenderer.positionCount = Angle;
     float offset  = Angle / 360f;
        for (int i=0;i< Angle;i++)
        {
            _pos_new.x = _center_pos.x + Mathf.Cos(offset*i) * _radius_length;
            _pos_new.y = _center_pos.y + Mathf.Sin(offset * i) * _radius_length;
            _pos_new.z = transform.localPosition.z;
            lineRenderer.SetPosition(i, _pos_new);
            lineRenderer.startWidth = 0.5f;
            lineRenderer.endWidth = 0.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //temp_angle += _angle_speed * Time.deltaTime; // 

        //_pos_new.x = _center_pos.x + Mathf.Cos(temp_angle) * _radius_length;
        //_pos_new.y = _center_pos.y + Mathf.Sin(temp_angle) * _radius_length;
        //_pos_new.z = transform.localPosition.z;

        //transform.localPosition = _pos_new;
    }
}
