using UnityEngine.Events;


public class FloatAnimator : ValueAnimator
{
    public FloatEvent fEvent;
    public FloatAnim[] anims;
    
    
    private FloatAnim a, b;
    private int count;

    private void Start()
    {
        count = anims.Length;
        a = anims[0];
        b = anims[count - 1];
    }


    private void OnValidate()
    {
        if(anims == null)
            return;
        
        count = anims.Length;

        if (count > 0)
        {
            a = anims[0];
            b = anims[count - 1];
        }
    }


    protected override void SetTime(float time)
    {
        if (time <= a.timeSpan.x)
        {
            fEvent.Invoke(a.a);
            return;
        }
        
        if (time >= b.timeSpan.x + b.timeSpan.y)
        {
            fEvent.Invoke(b.b);
            return;
        }
        
        float p = a.a;
        for (int i = 0; i < count; i++)
            if (anims[i].GetValue(time, ref p))
                break;
            
        fEvent.Invoke(p);
    }
}


[System.Serializable]
public class FloatEvent : UnityEvent<float> { }