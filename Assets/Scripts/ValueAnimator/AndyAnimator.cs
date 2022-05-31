using System.Collections.Generic;
using UnityEngine;

public class AndyAnimator : MonoBehaviour
{
    public float time;
    
    public static readonly List<ValueAnimator> AllAnims = new List<ValueAnimator>();


    private void LateUpdate()
    {
        int count = AllAnims.Count;
        for (int i = 0; i < count; i++)
            AllAnims[i].Evaluate(time);
    }
}
