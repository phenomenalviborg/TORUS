using System;
using System.Collections.Generic;
using ATVR;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;     
#endif

public class AndyAnimator : MonoBehaviour
{
    public float time;
    public Vector2 loopTime;
    
    [Space]
    public float speed = 1;
    
    [Space]
    public bool useSystemTime;
    
    [Header("AnimTime ReadOut")]
    public float animTime;
    
    private static readonly List<ValueAnimator> AllAnims = new List<ValueAnimator>();
    
    #if UNITY_EDITOR
    private static readonly List<GameObject> gameObjects = new List<GameObject>();
    private static int pick;
    private static GameObject last;
    #endif

    public static void AddAnim(ValueAnimator animator)
    {
        AllAnims.Add(animator);
        
        #if UNITY_EDITOR
        CollectGameObjects();
        #endif
    }
    
    public static void RemoveAnim(ValueAnimator animator)
    {
        AllAnims.Remove(animator);
        
        #if UNITY_EDITOR
        CollectGameObjects();
        #endif
    }

#if UNITY_EDITOR

    private static void CollectGameObjects()
    {
        gameObjects.Clear();
        
        int count = AllAnims.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject gO = AllAnims[i].gameObject;
            if(!gameObjects.Contains(gO))
                gameObjects.Add(gO);
        }
        
        pick = 0;
    }
#endif



    private void Update()
    {
        if (VRInput.RightHand.GetPressDown(Button.ButtonOne))
        {
            useSystemTime = !useSystemTime;
            speed = 1;
        }
        
        
        if (useSystemTime)
        {
            DateTime n = DateTime.Now;
            
            int hour = (24 + n.Hour - 4) % 24;
            time = (hour * 60 * 60 + n.Minute * 60 + n.Second) % Mathf.FloorToInt(loopTime.y) + n.Millisecond * .001f;
        }
        else
        {
            speed = Mathf.Clamp(speed + VRInput.RightHand.GetJoystick().x * Time.deltaTime * 5, -10, 10);
            if(VRInput.RightHand.GetPressDown(Button.Joystick))
                speed = 1;
            
            time += Time.deltaTime * speed;
        }
            
        
        animTime = time.Wrap(0, loopTime.y) + loopTime.x;
        
        int count = AllAnims.Count;
        for (int i = 0; i < count; i++)
            AllAnims[i].Evaluate(animTime);
        
        #if UNITY_EDITOR
        Select();
        #endif
    }

    #if UNITY_EDITOR
    private void Select()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
            Select(gameObject);
        if(Input.GetKeyDown(KeyCode.Alpha2))
            Select(gameObjects[pick++ % gameObjects.Count]);
        if(Input.GetKeyDown(KeyCode.Alpha3))
            Select(last);
    }
    
    
    private void Select(GameObject go)
    {
        if (go != gameObject)
            last = go;
            
        Selection.activeObject = go;

    }
    #endif
}