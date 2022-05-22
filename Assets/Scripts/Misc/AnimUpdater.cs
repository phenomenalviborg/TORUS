using UnityEngine;
using UnityEngine.Playables;


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
        if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetButtonDown("Fire2"))
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
            
    }
}
