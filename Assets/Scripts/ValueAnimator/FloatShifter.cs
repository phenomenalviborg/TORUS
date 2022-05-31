public class FloatShifter : ValueAnimator
{
    public FloatEvent fEvent;
    
    public float shiftSpeed;
    public float startValue;
    
    
    protected override void SetTime(float time)
    {  
        fEvent.Invoke(startValue + time * shiftSpeed);
    }
}
