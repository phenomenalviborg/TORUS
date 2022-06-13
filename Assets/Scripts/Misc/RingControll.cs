using UnityEngine;
using Random = UnityEngine.Random;


public class RingControll : MonoBehaviour
{
    public float anim;
    public float vis = 1;
    
    private MeshRenderer mR;
    private MaterialPropertyBlock block;
    
    private Transform trans;
    
    [Header("Audio ReadOut")]
    public Vector3 center;
    public float radius;
    public float hoopines;
    
    public AnimTorus torus;
    private float spin;
    
    [HideInInspector]
    public RingSoundTransform[] sounds;
    private Transform spinTrans;
    private int soundCount;
    
    [HideInInspector]
    public RingSave save = new RingSave();
    
    
    private void Start()
    {
        mR = GetComponent<MeshRenderer>();
        block = new MaterialPropertyBlock();
        mR.GetPropertyBlock(block);
        mR.enabled = false;
        trans = transform;
    }

    
    private void LateUpdate()
    {
        if (block != null)
        {
            block.SetFloat("_Anim", anim);
            block.SetFloat("_Vis", vis);
            mR.SetPropertyBlock(block);
        }
        
        UpdateSounds();
    }


    public RingControll Setup(AnimTorus torus, bool mutation = false)
    {
        this.torus = torus;
        
        trans = transform;
        
        spinTrans = new GameObject("SpinTrans").transform;
        spinTrans.SetParent(trans, false);
        
        soundCount = SoundInfo.SoundsPerRing + torus.soundSettings.soundAdd;
        sounds = new RingSoundTransform[soundCount];
        float step = 360f / soundCount;
        for (int i = 0; i < soundCount; i++)
            sounds[i] = Instantiate(SoundDummy, spinTrans).GetComponent<RingSoundTransform>().
                Setup(this, Quaternion.AngleAxis(i * step, Vector3.up) * Vector3.right);
        
        spin = Random.Range(0, 360f);
        
        if(mutation)
            gameObject.name = "Mutation";
        
        return this;
    }


    public void UpdateRing(Vector3 pos, Quaternion rot, float scale, float anim, float vis, bool local = false)
    {
        Transform parent = trans.parent;
        bool hasParent = parent != null;
        center = hasParent ? parent.TransformPoint(pos) : pos;
        
        Vector3 up = rot * Vector3.up;
        hoopines = 1f - Mathf.Abs((hasParent ? parent.TransformDirection(up) : up).y);
        
        radius = hasParent? parent.TransformVector(Vector3.right * scale).magnitude : scale;
        
        
        bool shouldBeVisible = anim * vis > .0001f;

        if (shouldBeVisible && CameraPosition.Distance(center) - radius > 28)
            shouldBeVisible = false;
        
        if (mR.enabled != shouldBeVisible)
        {
            mR.enabled = shouldBeVisible;
            
            if(shouldBeVisible)
                SoundInfo.RingVisibleEvent(this);
            else
                SoundInfo.RingInactiveEvent(this);
        }
            
        this.anim = anim;
        this.vis  = vis;
        
        if(!shouldBeVisible)
            return;
        
        if (local)
        {
            trans.localPosition = pos;
            trans.localRotation = rot;
        }
        else
        {
            trans.position = pos;
            trans.rotation = rot;
        }
            
        trans.localScale = Vector3.one * scale;
    }
    
    
    public void SaveIt(Vector3 pos, Quaternion rot, float scale, float anim, float vis)
    {
        save.Store(pos, rot, scale, anim, vis);
    }

    public void LoadIt(float animOverride, float visOveride)
    {
        UpdateRing(save.pos, save.rot, save.scale, save.anim * animOverride, save.vis * visOveride, true);
        
        Vector3 p = trans.position;
        //if(p.y > radius)
        //    return;
        
        float d = Mathf.Abs(trans.up.y);
        if(d > .999f)
            return;
        
        float a = Mathf.Acos(d);
        float r = radius;
        float s = Mathf.Sin(a);
        
        trans.position = p.SetY(Mathf.Max(p.y, s * r));
    }
    
    private void UpdateSounds()
    {
        if(torus == null)
            return;
        
        if(SoundInfo.ConstantSpeed)
            spin += Time.deltaTime * torus.soundSettings.spinSpeed / ((Mathf.Max(.01f, radius) * 2 * Mathf.PI)) * SoundInfo.GlobalMulti;
        else
            spin += Time.deltaTime * torus.soundSettings.spinSpeed * SoundInfo.GlobalMulti;
        
        spin = spin.Wrap(-360f, 360f);
        
        float volume = mR.enabled? vis : 0;
        
        
        float step = 360f / soundCount;
        const float multi = 1f / 360f;
        
        float maxVol = 0;
        for (int i = 0; i < soundCount; i++)
        {
           float angle = spin + i * step;
           float vol = (1f - (angle + 180).Wrap(0, 360) * multi <= anim? 1 : 0) * volume;
           maxVol = Mathf.Max(maxVol, sounds[i].UpdateSound(vol));
        }
        
        //if(maxVol > .000001f)
            spinTrans.localRotation = Quaternion.AngleAxis(spin, Vector3.up);
    }
    

    private static GameObject ringDummy;
    public static GameObject RingRingDummy
    {
        get
        {
            if(ringDummy == null)
                ringDummy = Resources.Load<GameObject>("Ring");
            
            return ringDummy;
        }
    }
    
    private static GameObject soundDummy;
    private static GameObject SoundDummy
    {
        get
        {
            if(soundDummy == null)
                soundDummy = Resources.Load<GameObject>("RingSoundTransform");
            
            return soundDummy;
        }
    }
}
