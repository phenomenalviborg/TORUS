using UnityEngine;
using UnityEngine.Events;


public class ColorAnimator : ValueAnimator
{
    public ColorEvent fEvent;
    public ColorAnim[] anims;
    
    
    private ColorAnim a, b;
    private int count;

    private void Start()
    {
        count = anims.Length;
        a = anims[0];
        b = anims[count - 1];
    }
    
    private void OnValidate()
    {
        count = anims.Length;
        a = anims[0];
        b = anims[count - 1];
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
        
        Color p = a.a;
        for (int i = 0; i < count; i++)
            if (anims[i].GetValue(time, ref p))
                break;
            
        fEvent.Invoke(p);
    }
}


[System.Serializable]
public class ColorEvent : UnityEvent<Color> { }
