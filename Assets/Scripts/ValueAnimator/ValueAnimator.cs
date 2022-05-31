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


    public virtual void Evaluate(float time){}
}


[Serializable]
public class FloatAnim
{
    public Vector2 timeSpan;
    public AnimationCurve curve;
    [Space]
    public float a, b;
    

    public bool GetValue(float time, ref float setValue)
    {
        if(time < timeSpan.x || time > timeSpan.x + timeSpan.y)
            return false;
        
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
    public Vector3 a, b;

    
    public bool GetValue(float time, ref Vector3 setValue)
    {
        if(time < timeSpan.x || time > timeSpan.x + timeSpan.y)
            return false;
        
        float l = (time - timeSpan.x) / timeSpan.y;
        Debug.Log(l);
        setValue = Vector3.Lerp(a, b, curve.Evaluate(l));
        return true;
    }
}
