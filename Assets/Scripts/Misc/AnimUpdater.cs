using UnityEngine;
using UnityEngine.Playables;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AnimUpdater : MonoBehaviour
{
    public float time;
    
    [Header("CurrentTime")]
    public float current;
    public int frame;
    
    private PlayableDirector director;
    
    private bool playing;
    
    
    private void Start()
    {
        director = GetComponent<PlayableDirector>();
        director.Evaluate();
        director.Stop();
    }

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            playing = !playing;

            if (playing)
            {
                director.Play();
                director.time = time;
                current = time;
                director.Evaluate();
            }
            else
                director.Stop();
        }

        if (playing)
        {
            current += Time.deltaTime;
            frame = Mathf.RoundToInt(current * 60);
        }
         
        
        /*
         if (Input.GetKeyDown(KeyCode.Alpha8) )
        {
            playing = !playing;

            if (playing)
            {
                current = time;
                director.time = current;
                director.RebuildGraph();
                director.Play();
            }
            else
            {
                director.Stop();
            }
        }
        

        if (playing)
        {
            current += Time.deltaTime;
            frame = Mathf.RoundToInt(current * 60);
            
           director.Evaluate();
        }
         */
    }


    public void SetTime(int frame)
    {
        time = frame / 60f;

        if (!Application.isPlaying)
        {
            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
            return;
        }

        if (playing)
            director.Stop();
        
        current = time;
        director.time = current;
        director.RebuildGraph();
        director.Play();
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(AnimUpdater))]
public class AnimUpdaterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        AnimUpdater anim = target as AnimUpdater;
        
        GUILayout.Space(20);
        GUILayout.Label("------------------");
        if(GUILayout.Button("Falling Orb"))
            anim.SetTime(0);
        if(GUILayout.Button("AppearingRingTorus"))
            anim.SetTime(1240);
        if(GUILayout.Button("FlowerTorus"))
            anim.SetTime(2490);
        if(GUILayout.Button("MoveBoth"))
            anim.SetTime(5393);
        if(GUILayout.Button("HugeRingTorus"))
            anim.SetTime(6321);
        
    }
}
#endif

