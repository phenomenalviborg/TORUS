using UnityEngine;


public class BabyManager : MonoBehaviour
{
    public int seed;
    
    [Range(0, 1)]
    public float completion;
    
    [Space]
    
    public TorusGroup[] groups;
    private float[] offsets;

    [System.Serializable]
    public class TorusGroup
    {
        public RingTorus ring;
        public BabyHoops hoop;

        public TorusGroup(RingTorus ring, BabyHoops hoop)
        {
            this.ring = ring;
            this.hoop = hoop;
        }


        public void SetCompletion(float completion)
        {
            ring.SetCompletion(completion);
            hoop.SetCompletion(completion);
        }
    }
    
    
    private int count;

    private void Start()
    {
        count = transform.childCount;
        groups = new TorusGroup[count];
        
        offsets = new float[count];
        System.Random r = new System.Random(seed);
        for (int i = 0; i < count; i++)
        {
            Transform child = transform.GetChild(i);
            groups[i]  = new TorusGroup(child.GetChild(2).GetComponent<RingTorus>(), child.GetChild(1).GetComponent<BabyHoops>());
            offsets[i] = r.Range(-0f, 1f);
        }
    }

    
    public void SetCompletion(float completion)
    { 
        this.completion = completion;
        
        const float o = .3f, p = 1 - o;

        for (int i = 0; i < count; i++)
        {
            float offset = offsets[i] * o;
            float lerp = Mathf.Max(0, completion - offset) / p;
            
            groups[i].SetCompletion(lerp);
        }
    }
}
