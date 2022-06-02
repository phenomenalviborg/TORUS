using UnityEngine;


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
    
    private AnimTorus torus;
    private float spin;
    
    [HideInInspector]
    public RingSoundTransform[] sounds;
    private Transform spinTrans;
    private int soundCount;
    
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


    public RingControll Setup(AnimTorus torus)
    {
        this.torus = torus;
        
        trans = transform;
        
        spinTrans = new GameObject("SpinTrans").transform;
        spinTrans.SetParent(trans, false);
        
        soundCount = SoundInfo.SoundsPerRing;
        sounds = new RingSoundTransform[soundCount];
        float step = 360f / soundCount;
        for (int i = 0; i < soundCount; i++)
            sounds[i] = Instantiate(SoundDummy, spinTrans).GetComponent<RingSoundTransform>().
                Setup(this, Quaternion.AngleAxis(i * step, Vector3.up) * Vector3.right);
        
        spin = Random.Range(0, 360f);
        
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


    private void UpdateSounds()
    {
        if(torus == null)
            return;
        
        if(SoundInfo.ConstantSpeed)
            spin += Time.deltaTime * torus.soundSettings.spinSpeed / (radius * 2 * Mathf.PI) * SoundInfo.GlobalMulti;
        else
            spin += Time.deltaTime * torus.soundSettings.spinSpeed * SoundInfo.GlobalMulti;
        
        spin = spin.Wrap(-360f, 360f);
        
        float volume = mR.enabled? vis : 0;
        
        if(mR.enabled)
            spinTrans.localRotation = Quaternion.AngleAxis(spin, Vector3.up);
        
        float step = 360f / soundCount;
        const float multi = 1f / 360f;
        for (int i = 0; i < soundCount; i++)
        {
           float angle = spin + i * step;
           float vol = (1f - (angle + 180).Wrap(0, 360) * multi <= anim? 1 : 0) * volume;
           sounds[i].UpdateSound(vol);
        }
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
