using UnityEngine;


public class MeshRendererAnimator : ValueAnimator
{
    public Vector2 timeSpan;
    
    
    private MeshRenderer mR;


    private void Start()
    {
        mR = GetComponent<MeshRenderer>();
    }
    

    protected override void SetTime(float time)
    {   
        bool shouldBeActive = time >= timeSpan.x && time <= timeSpan.x + timeSpan.y;
        if(mR.enabled != shouldBeActive)
            mR.enabled = shouldBeActive;
    }
}
