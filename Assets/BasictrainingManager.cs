using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BasictrainingManager : MonoBehaviour
{
    [SerializeField]
    private Basictraining[] basictrainings;
    int stepIndex;
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i< basictrainings.Length;i++)
        {
            basictrainings[i].Init(CompleteTraining);
        }
        stepIndex = 0;
        basictrainings[stepIndex].StartTraining();
    }

   
     private void CompleteTraining()
    {
        stepIndex++;

        if(stepIndex>= basictrainings.Length)
        {
            Debug.Log("完成所有的训练");
        }
        else
        {
            basictrainings[stepIndex++].StartTraining();
        }
      
    }
}
