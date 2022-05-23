using System.Collections.Generic;
using UnityEngine;


public class CopyRingTorus : RingTorus
{
    public AppearingRingTorus mate;
    public float vis;
    public int seed;
    
    private int[] id;
    
    private int mySeed;
    
    public RingControll copyThis;
    
    
    protected override void CreateRings()
    {
        base.CreateRings();
        
        CreateOrder();
    }
    
    
    protected override void UpdateRings()
    {
        spin = mate.spin;
        spinOffset = mate.spinOffset;
        
        base.UpdateRings();
    }


    protected override void UpdateRingStates()
    {
        for (int i = 0; i < ringCount; i++)
        {
            int value = id[i];
            states[i] = new RingState(1, vis * (value == 0? 1 : Mathf.Clamp01(completion * ringCount - (ringCount - 1 - value))));
        }
            
    }


    private void OnValidate()
    {
        if (mySeed != seed)
        {
            mySeed = seed;
            CreateOrder();
        }
    }


    private void CreateOrder()
    {
        if(rings == null || rings.Length < ringCount)
            return;
        
        List<int> order = new List<int>();
        for (int i = 0; i < ringCount; i++)
            order.Add(i);
        
        id = new int[ringCount];
        int index = 0;
        System.Random r = new System.Random(seed);
        while (order.Count > 0)
        {
            int pick = r.Range(0, order.Count);
            int value = order[pick];
            id[index] = value;
            order.RemoveAt(pick);

            if (value == 0)
            {
                copyThis = rings[index];
                //Debug.Log(index);
            }
                
                    
            index++;
        }
    }
}
