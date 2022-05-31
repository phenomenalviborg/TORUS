public class SpinOffsetAnimator : ValueAnimator
{
    public float spinSpeed;
    
    private RingTorus torus;


    private void Start()
    {
        torus = GetComponent<RingTorus>();
    }

    
    protected override void SetTime(float time)
    {   
        torus.spinOffset = time * spinSpeed;
    }
}
