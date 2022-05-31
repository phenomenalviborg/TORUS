using System;
using UnityEngine;

public class ValueAnimator : MonoBehaviour
{
    private void OnEnable()
    {
        AndyAnimator.AllAnims.Add(this);
    }
    
    
    private void OnDisable()
    {
        AndyAnimator.AllAnims.Add(this);
    }


    public void Evaluate(float time){ SetTime(offset != null? time - offset.start : time); }
    
    
    protected virtual void SetTime(float time){}
    
    
    private AnimOffset offset;


    public void SetAnimOffset(AnimOffset offset)
    {
        this.offset = offset;
    }
}


[Serializable]
public class FloatAnim
{
    public Vector2 timeSpan;
    public AnimationCurve curve;
    [Space]
    public float a;
    public float b;
    

    public bool GetValue(float time, ref float setValue)
    {
        if(time <= timeSpan.x)
            return false;

        if (time >= timeSpan.x + timeSpan.y)
        {
            setValue = b;
            return false;
        }
        
        setValue = Mathf.Lerp(a, b, curve.Evaluate((time - timeSpan.x) / timeSpan.y));
        return true;
    }
}


[Serializable]
public class VectorAnim
{
    public Vector2 timeSpan;
    public AnimationCurve curve;
    [Space]
    public Vector3 a;
    public Vector3 b;

    
    public bool GetValue(float time, ref Vector3 setValue)
    {
        if(time <= timeSpan.x)
            return false;

        if (time >= timeSpan.x + timeSpan.y)
        {
            setValue = b;
            return false;
        }
            
        
        setValue = Vector3.Lerp(a, b, curve.Evaluate((time - timeSpan.x) / timeSpan.y));
        return true;
    }
}
