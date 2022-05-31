using UnityEngine;

public class PositionAnimator : ValueAnimator
{
    public VectorAnim[] anims;
    
    [Space]
    public Transform trans;
    
    private VectorAnim a, b;
    private int count;

    
    private void Start()
    {
        if(trans == null)
            trans = transform;
        
        count = anims.Length;
        a = anims[0];
        b = anims[count - 1];
    }


    protected override void SetTime(float time)
    {
        if (time <= a.timeSpan.x)
        {
            trans.localPosition = a.a;
            return;
        }
        
        if (time >= b.timeSpan.x + b.timeSpan.y)
        {
            trans.localPosition = b.b;
            return;
        }
        
        Vector3 p = a.a;
        for (int i = 0; i < count; i++)
            if (anims[i].GetValue(time, ref p))
            {
                trans.localPosition = p;
                break;
            }
    }
}
