using System.Collections.Generic;
using UnityEngine;


public class BabyManager : MonoBehaviour
{
    public HoopBabyTorus torus;
    
    [Range(0, 1)]
    public float collapse;
    
    public int seed;
    
    
    [Space]
    public float vortexSpinAmount;
    public float rotationAmount;
    public float rotorMulti;
    
    private TorusGroup[] groups;
    private float[] offsets;
    

    [System.Serializable]
    public class TorusGroup
    {
        public MutatedRingTorus ring;
        public MutatedHoopTorus hoop;
        public Transform trans;
        
        private readonly RingControll main, mutation;
        private readonly RingControll[] secondary;
        
        private readonly int id;
        
        private readonly Vector3 mainRot, mainRot2;
        private readonly Vector3 mutationRot;
        private readonly Vector3[] secondaryRot;
        
        private readonly float mutationSpeed;
        private readonly float[] secondarySpeed;
        
        private readonly int secondaryCount;
        private float vis;


        private static Vector3 RandomRot(System.Random r, float rotationAmount)
        {
            return new Vector3(r.Range(-rotationAmount, rotationAmount),
                               r.Range(-rotationAmount, rotationAmount),
                               r.Range(-rotationAmount, rotationAmount));
        }
        
        
        public TorusGroup(Transform trans, MutatedRingTorus ring, MutatedHoopTorus hoop, int id, int seed, float rotationAmount)
        {
            this.id = id;
            
            this.trans = trans;
            this.ring = ring;
            this.hoop = hoop;
            
            ring.SetCompletion(1f / 5);
            ring.SetSpinOffset(1f / 5 * 360f);
            
            this.hoop.SetSpin(.5f);
            
            List<RingControll> second = new List<RingControll>();
            ring.GetRings(second, ref main, ref mutation);
            hoop.GetRings(second, ref mutation);
            
            secondary = second.ToArray();
            secondaryCount = secondary.Length;
            
            System.Random r = new System.Random(seed + 23456 + id * 34);
            mutationRot = RandomRot(r, rotationAmount);
            
            secondaryRot = new Vector3[secondaryCount];
            for (int i = 0; i < secondaryCount; i++)
                secondaryRot[i] = RandomRot(r, rotationAmount);
            
            mutationSpeed = r.Range(.5f, 1f);
            secondarySpeed = new float[secondaryCount];
            for (int i = 0; i < secondaryCount; i++)
                secondarySpeed[i] = r.Range(.5f, 1f);
            
            mainRot  = new Vector3(r.Range(-1, 2) * 360, r.Range(-1, 2) * 360, r.Range(-1, 2) * 360);
            mainRot2 = new Vector3(r.Range(-60f, 60f), r.Range(-60f, 60f), r.Range(-60f, 60f));
            //Debug.Log(mainRot);
        }


        public void SetPlacement(Placement placement, float collapse, float vortexSpinAmount, float rotorMulti, float offset)
        {
            placement.Apply(trans);
            
            const float o = .4f, p = 1 - o;
            float off  = offset * o;
            float lerp = Mathf.Max(0, collapse - off) / p;
            
            float extraSpin = Mathf.Min(-3 + lerp * 5.5f, 1);
            float allignment = Mathf.SmoothStep(0, 1, extraSpin);
            
            float spinCollaps = Mathf.Lerp(lerp, collapse, allignment);
            
            
            Vector3 center = trans.InverseTransformPoint(Vector3.zero);
            Vector3 up     = trans.InverseTransformDirection(Vector3.up);
            Vector3 dir    = trans.InverseTransformDirection(Quaternion.AngleAxis(id * 360f / 8f, Vector3.up) * Vector3.right);
            
        //  Main  //
            RingSave save = main.save;
            float pieceSpin = Mathf.Pow(spinCollaps, 3);
            Quaternion vortexRot = Quaternion.AngleAxis(pieceSpin * vortexSpinAmount * -1 * rotorMulti, up);
            Quaternion flatRot = Quaternion.Inverse(trans.rotation) * Quaternion.AngleAxis(180, Vector3.forward);
            
            Quaternion extra = Quaternion.Euler(mainRot * extraSpin);
            
            float bbb = Mathf.Clamp01(Mathf.Abs((.73f - collapse) * 9));
                  bbb = Mathf.Pow(bbb, 4);
                  bbb = 1 - bbb;
                       extra *= Quaternion.Euler(mainRot2 * Mathf.SmoothStep(0, 1, bbb));
            save.rot = (vortexRot * (Quaternion.Slerp(save.rot, Quaternion.AngleAxis(id * 360f / 8f, up) * flatRot, allignment))) * extra;// * Quaternion.AngleAxis(Mathf.Pow(collapse, 10) * 720f, Vector3.right));
            
            Vector3 ringOffset = -trans.InverseTransformDirection(trans.localPosition) + dir * -.5f;
            save.pos = Vector3.Lerp(save.pos, ringOffset, allignment);
            save.pos += up * (allignment + spinCollaps * 2.25f);
            save.pos = center + vortexRot * (save.pos - center);
            
            main.LoadIt((1f - allignment * .6f) * (1f - Mathf.Pow(spinCollaps, 15)), vis);
            
            
            collapse = Mathf.Pow(spinCollaps * 1.15f, 1.2f);
            pieceSpin = Mathf.Pow(collapse, 2);
            
            float animOverride = Mathf.Pow(1f - Mathf.Pow(Mathf.Clamp01(collapse * 1.05f), 2), 2); //Mathf.Sin(Time.realtimeSinceStartup * 2) * .5f + .5f;
                  animOverride = Mathf.Clamp01(animOverride * (.6f + .4f * Mathf.Pow(animOverride, 100)));
            {
                RingControll r = mutation;
                save = r.save;
                
                Quaternion rR = Quaternion.Euler(mutationRot * collapse);
                save.pos = (rR * save.pos) * (1f - collapse);
                save.rot = rR * save.rot;
                
                float speed = mutationSpeed;
                
                vortexRot = Quaternion.AngleAxis(pieceSpin * vortexSpinAmount * -1 * .8f * speed, up);
                save.pos = center + vortexRot * (save.pos - center) * ((1f - collapse) + .2f * Mathf.Pow(collapse * 3, 2));
                save.pos += up * collapse * 2.75f * speed;
                
                r.LoadIt(animOverride, vis);
            }
            
            for (int i = 0; i < secondaryCount; i++)
            {
                RingControll r = secondary[i];
                save = r.save;
                
                Quaternion rR = Quaternion.Euler(secondaryRot[i] * collapse);
                save.pos = (rR * save.pos) * (1f - collapse);
                save.rot = rR * save.rot;
                
                float speed = secondarySpeed[i];
                
                vortexRot = Quaternion.AngleAxis(pieceSpin * vortexSpinAmount * -1 * .8f * speed, up);
                save.pos = center + vortexRot * (save.pos - center) * ((1f - collapse) + .2f * Mathf.Pow(collapse * 3, 2));
                save.pos += up * collapse * 2.75f * speed;
                
                r.LoadIt(animOverride, vis);
            } 
        }
        
        
        public void SetSpin(float spin)
        {
            float l = Mathf.Max(0, spin - 180) / 360f;
            hoop.SetCompletion(l);
            ring.SetSpin(spin);
            
            ring.SetCompletion(Mathf.Floor((1f / 5 + l) * 5) / 5.0f);
        }


        public void SetVis(float vis)
        {
            this.vis = vis;
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
            groups[i]  = new TorusGroup(child, child.GetChild(2).GetComponent<MutatedRingTorus>(), child.GetChild(1).GetComponent<MutatedHoopTorus>(), i, seed, rotationAmount);
            offsets[i] = r.Range(-0f, 1f);
        }
    }


    private void LateUpdate()
    {
        for (int i = 0; i < count; i++)
            groups[i].SetPlacement(torus.GetPlacement(i), collapse, vortexSpinAmount, rotorMulti, offsets[i]);
    }


    public void SetVis(float vis)
    { 
        for (int i = 0; i < count; i++)
            groups[i].SetVis(vis);
        
    }
    
    
    public void SetSpin(float spin)
    { 
        float l = spin / 576f;
        const float o = .2f, p = 1 - o;

        for (int i = 0; i < count; i++)
        {
            float offset = offsets[i] * o;
            float lerp   = Mathf.Max(0, l - offset) / p;
            groups[i].SetSpin(Mathf.SmoothStep(0,  576f, lerp));
        }
           
    }
    
    
    public void SetCollapse(float collapse)
    { 
        this.collapse = collapse;
    }
}


public class RingSave
{
    public Vector3 pos;
    public Quaternion rot;
    public float scale;
    public float anim;
    public float vis;

    public void Store(Vector3 pos, Quaternion rot, float scale, float anim, float vis)
    {
        this.pos   = pos;
        this.rot   = rot;
        this.scale = scale;
        this.anim  = anim;
        this.vis   = vis;
    }
    
    
}