using System.Collections.Generic;
using ATVR;
using UnityEngine;
using UnityEngine.Events;


public class SoundInfo : Singleton<SoundInfo>
{
    public List<AnimTorus> allToruses;

    public List<RingControll> activeRings = new List<RingControll>();

    //public List<RingSoundTransform> activeRingSounds = new List<RingSoundTransform>();

    [Space] public int soundsPerRing;
    public float globalMulti = 1;
    public bool constantSpeed;

    public static int SoundsPerRing
    {
        get { return Inst.soundsPerRing; }
    }

  
    private bool showSounds;
    
    [Header("Press M or VR Button to toggle Visuals")]
    public Hand hand;
    public Button button;

    public static bool ShowSounds
    {
        get { return Inst.showSounds; }
    }

    public static bool ConstantSpeed
    {
        get { return Inst.constantSpeed; }
    }

    public static float GlobalMulti
    {
        get { return Inst.globalMulti; }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) || VRInput.Get(hand).GetPressDown(button))
            showSounds = !showSounds;
    }


    public static void SetTorus(AnimTorus torus)
    {
        Inst.allToruses.Add(torus);
    }

    public static void RingVisibleEvent(RingControll ring)
    {
        Inst.activeRings.Add(ring);
    }

    public static void RingInactiveEvent(RingControll ring)
    {
        Inst.activeRings.Remove(ring);
    }
}