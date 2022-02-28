using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Basictraining : MonoBehaviour
{
    protected System.Action completeAciton;
    public abstract void Init(System.Action completeAciton);
    public abstract void StartTraining();

    public abstract void CompleteTraining();
    public abstract void ResetTraining();
    protected abstract void ExecuteSteps();
    protected abstract void CompleteSteps();
    

}
