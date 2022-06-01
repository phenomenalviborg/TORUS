using System.Collections.Generic;
using UnityEngine;


public class SoundInfo : Singleton<SoundInfo>
{
    public List<AnimTorus> allToruses;
    
    public List<RingControll> activeRings = new List<RingControll>();
    
    //public List<RingSoundTransform> activeRingSounds = new List<RingSoundTransform>();
    
    [Space]
    public int soundsPerRing;

    public static int SoundsPerRing { get { return Inst.soundsPerRing; } }
    
    public bool showSounds;
    
    public static bool ShowSounds { get { return Inst.showSounds; } }

    
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
