using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Probetraining : Basictraining
{

    public float continuedCollTime;
    public Transform collisionTarget;
    public Transform[] Balls;
    private CollisionDetection[] collisionDetections;
    private Transform curActivateBall;
    private int Index;
  


    
    // Start is called before the first frame update


    public override void Init(System.Action completeAciton)
    {
        this.completeAciton = completeAciton;
        collisionDetections = new CollisionDetection[Balls.Length];
        for (int i = 0; i < Balls.Length; i++)
        {
            collisionDetections[i] = Balls[i].GetComponent<CollisionDetection>();
            collisionDetections[i].SetCollisionTarget(collisionTarget);
            Balls[i].gameObject.SetActive(false);
        }
    }

    public override void StartTraining()
    {
        Index = 0;
        CompleteSteps();
    }
    public override void CompleteTraining()
    {
        for (int i = 0; i < Balls.Length; i++)
        {

            Balls[i].gameObject.SetActive(false);
        }
        if (completeAciton != null)
            completeAciton();
    }
    protected override void ExecuteSteps()
    {
        Balls[Index].transform.localEulerAngles = Vector3.zero;
        Balls[Index].gameObject.SetActive(true);
        StartCoroutine(CheckCollision());

    }

    protected override void CompleteSteps()
    {
        Balls[Index].gameObject.SetActive(false);
        Index++;
        if (Index >= Balls.Length)
        {
            CompleteTraining();
        }
        else
        {
            ExecuteSteps();
        }

    }
    IEnumerator CheckCollision()
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        float startTime=0;
        bool IsrecordTime=false;


        while (true)
        {
            if(collisionDetections[Index].IsCollision)
            {
                if (!IsrecordTime)
                {
                    startTime = Time.time;
                    IsrecordTime = true;
                }
            }
            else
            {
                IsrecordTime = false;
            }
         

            if(IsrecordTime)
            {
                if ((Time.time - startTime) >= continuedCollTime)
                {
                    CompleteSteps();
                    yield break;
                }
                //else
                //{
                //    Balls[Index].transform.Rotate(Vector3.up * 5 * Time.deltaTime);
                //}
             
            }
            Balls[Index].transform.Rotate(Vector3.right * 10 * Time.deltaTime);
            yield return waitForEndOfFrame;
        }
    }

    public override void ResetTraining()
    {
        throw new System.NotImplementedException();
    }
}
