using UnityEngine;

public class ScaleAnimator : ValueAnimator
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
        if (time < a.timeSpan.x)
        {
            trans.localScale = a.a;
            return;
        }
        
        if (time > b.timeSpan.x + b.timeSpan.y)
        {
            trans.localScale = b.b;
            return;
        }
        
        Vector3 p = a.a;
        for (int i = 0; i < count; i++)
            if (anims[i].GetValue(time, ref p))
            {
                trans.localScale = p;
                break;
            }
    }
}
