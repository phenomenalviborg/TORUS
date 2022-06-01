using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActiveRings : Singleton<ActiveRings>
{
    public int count;
   
    private static List<RingControll> active = new List<RingControll>();


    public static void Add(RingControll ring)
    {
        active.Add(ring);
        Inst.count++;
    }
    
    
    public static void Remove(RingControll ring)
    {
        active.Remove(ring);
        Inst.count--;
    }
}
