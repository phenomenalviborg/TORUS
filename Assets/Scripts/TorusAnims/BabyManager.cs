using UnityEngine;


public class BabyManager : MonoBehaviour
{
    public HoopBabyTorus torus;
    
    public int seed;
    
    
    
    [Space]
    
    public TorusGroup[] groups;
    private float[] offsets;

    [System.Serializable]
    public class TorusGroup
    {
        public AppearingRingTorus ring;
        public BabyHoops hoop;
        public Transform trans;

        public TorusGroup(Transform trans, AppearingRingTorus ring, BabyHoops hoop)
        {
            this.trans = trans;
            this.ring = ring;
            this.hoop = hoop;
            
            ring.SetCompletion(1f / 5);
            ring.SetSpinOffset(1f / 5 * 360f);
            
            this.hoop.SetSpin(.5f);
        }


        public void SetPlacement(Placement placement)
        {
            placement.Apply(trans);
        }


        public void SetCompletion(float completion)
        {
            //ring.SetCompletion(completion);
            hoop.SetCompletion(completion);
        }
        
        
        public void SetSpin(float spin)
        {
            float l = Mathf.Max(0, spin - 180) / 360f;
            hoop.SetCompletion(l);
            ring.SetSpin(spin);
            
            ring.SetCompletion(Mathf.Floor((1f / 5 + l) * 5) / 5.0f);
        }
    }
    
    
    private const int count = 8;

    private void Start()
    {
        groups = new TorusGroup[count];
        
        offsets = new float[count];
        System.Random r = new System.Random(seed);
        for (int i = 0; i < count; i++)
        {
            Transform child = transform.GetChild(i);
            groups[i]  = new TorusGroup(child, child.GetChild(2).GetComponent<AppearingRingTorus>(), child.GetChild(1).GetComponent<BabyHoops>());
            offsets[i] = r.Range(-0f, 1f);
        }
    }


    private void LateUpdate()
    {
        for (int i = 0; i < count; i++)
            groups[i].SetPlacement(torus.GetPlacement(i));
    }


    public void SetCompletion(float completion)
    { 
        /*const float o = .3f, p = 1 - o;

        for (int i = 0; i < count; i++)
        {
            float offset = offsets[i] * o;
            float lerp = Mathf.Max(0, completion - offset) / p;
            
            groups[i].SetCompletion(lerp);
        }*/
    }
    
    
    public void SetSpin(float spin)
    { 
        float l = spin / 540f;
        const float o = .2f, p = 1 - o;

        for (int i = 0; i < count; i++)
        {
            float offset = offsets[i] * o;
            float lerp   = Mathf.Max(0, l - offset) / p;
            groups[i].SetSpin(Mathf.SmoothStep(0, 540f, lerp));
        }
           
    }
}
