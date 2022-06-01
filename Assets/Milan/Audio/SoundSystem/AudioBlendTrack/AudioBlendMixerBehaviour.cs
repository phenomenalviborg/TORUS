using System;
using System.Collections.Generic;
using atomtwist.AudioNodes;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class AudioBlendMixerBehaviour : PlayableBehaviour
{

    /// methodology from <see cref="VisualEffectActivationMixerBehaviour"/> from the unity demos.
    bool[] enabledStates;
    HashSet<int> guidsPlayed = new HashSet<int>();

    float m_DefaultVolume;

    float m_AssignedVolume;

    //AudioSource m_TrackBinding;
    
    public static HashSet<int> allGuidsPlayed = new HashSet<int>();

  

    public override void ProcessFrame(Playable playable,UnityEngine.Playables.FrameData info, object playerData)
    {
        //m_TrackBinding = playerData as AudioSource;

        //if (m_TrackBinding == null)
        //    return;

        //if (!Mathf.Approximately(m_TrackBinding.volume, m_AssignedVolume))
        //    m_DefaultVolume = m_TrackBinding.volume;
        // we set default volume to 0 so we can fade clips in/out.
        m_DefaultVolume = 0f;

        int inputCount = playable.GetInputCount();

        // blendedVolume is for the whole track.
        float blendedVolume = 0f;
        float totalWeight = 0f;
        float greatestWeight = 0f;

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            //Debug.Log("IW of " + m_TrackBinding + " " + inputWeight);
            // if inputWeight is not zero, this clip is being played.
            bool newEnabledState = inputWeight != 0.0f;

            ScriptPlayable<AudioBlendBehaviour> inputPlayable = (ScriptPlayable<AudioBlendBehaviour>)playable.GetInput(i);
            AudioBlendBehaviour input = inputPlayable.GetBehaviour();

            // clip volume is for this particular clip, includes the fade-in and out.
            var clipVolume = input.soundSettings.volume * inputWeight;
            blendedVolume += clipVolume;
            totalWeight += inputWeight;

            if (inputWeight > greatestWeight)
            {
                greatestWeight = inputWeight;
            }

            // play and stop when entering/exiting clip, so the sound continues when timeline is paused.
            if (enabledStates[i] != newEnabledState)
            {
                if (newEnabledState)
                {
                    var id = SoundSystem.Instance.Play(input.soundSettings);
                    input.soundID = id;
                    guidsPlayed.Add(id);
                    allGuidsPlayed.Add(id);
                }
                else
                {
                    SoundSystem.Instance.Stop(input.soundID);
                    guidsPlayed.Remove(input.soundID);
                    allGuidsPlayed.Remove(input.soundID);
                }

                enabledStates[i] = newEnabledState;
            }
            

            SoundSystem.Instance.SetVolume(input.soundID, clipVolume);

        
            

            
        }

        //audioSource.volume = blendedVolume;
        

    }

    public override void OnPlayableCreate(Playable playable)
    {
        enabledStates = new bool[playable.GetInputCount()];

    }

    public override void OnPlayableDestroy(Playable playable)
    {
        foreach (var g in guidsPlayed)
        {
            SoundSystem.Instance?.Stop(g);
        }

        //if (m_TrackBinding != null)
        //{
        //    m_TrackBinding.Stop();
        //}
        enabledStates = null;
    }


}
