using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace atomtwist.AudioNodes
{
    [RequireComponent(typeof(AudioSource))]
    [ExecuteInEditMode]
    public class SoundInstance : MonoBehaviour
    {
        public AudioSource audioSource;
        public SoundsSettings soundSettings;
        public int soundId;
        
        public bool IsPlaying
        {
            get { return audioSource.isPlaying; }
        }

        public float KeyToPitch(int midiKey, int transpose, int octave)
        {
            int c4Key = midiKey - 72;
            //apply transpose & octave
            c4Key += transpose;
            c4Key += octave * 12;
            float pitch = Mathf.Pow(2, c4Key / 12f);
            return pitch;
        }

        void ApplySettings(SoundsSettings soundsSettings)
        {
            this.soundSettings = soundsSettings;
            audioSource.outputAudioMixerGroup = this.soundSettings.mixerGroup;
            if(this.soundSettings.playAtTransform != null)
                transform.position = this.soundSettings.playAtTransform.position;
            if (this.soundSettings.playAtTransform == null)
                transform.position = this.soundSettings.positionToPlayAt;
            audioSource.volume = this.soundSettings.volume;
            audioSource.pitch = this.soundSettings.pitch * KeyToPitch(this.soundSettings.midiNote, this.soundSettings.transpose, this.soundSettings.octave);
            audioSource.spatialize = this.soundSettings.spatialized;
            audioSource.spatialBlend = this.soundSettings.spatialBlend;
            audioSource.loop = this.soundSettings.loop;
            audioSource.dopplerLevel = this.soundSettings.dopplerLevel;
        }

        public void Play(SoundsSettings soundSettings,int id, AudioClip clipToPlay)
        {
            soundId = id;
            ApplySettings(soundSettings);
            //spatialize post effects bruh important
            audioSource.spatializePostEffects = true;
            audioSource.clip = clipToPlay;
            if(this.soundSettings.startWithRandomOffset)
                audioSource.time = Random.Range(0, clipToPlay.length);
            if (this.soundSettings.delay != 0)
                audioSource.PlayScheduled(AudioSettings.dspTime + this.soundSettings.delay);
            else if (this.soundSettings.delay == 0)
                audioSource.Play();
        }

        public void Stop()
        {
            //Debug.Log("Instance stopped: "+soundId);
            audioSource.Stop();
        }

        public void SetVolume(float volume)
        {
            audioSource.volume = volume;
        }
        
        public void SetPitch(float pitch)
        {
            audioSource.pitch = pitch;
        }

        public void SetSpatialBlend(float blend)
        {
            audioSource.spatialBlend = blend;
        }

        public void SetDoppler(float doppler)
        {
            audioSource.dopplerLevel = doppler;
        }

        float[] clipSampleData = new float[1024];
        private float clipLoudness;
        public float GetLoudness()
        {
            audioSource.clip.GetData(clipSampleData, audioSource.timeSamples); //I read 1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
            clipLoudness = 0f;
            foreach (var sample in clipSampleData) {
                clipLoudness += Mathf.Abs(sample);
            }
            clipLoudness /= 1024;
            return clipLoudness;
        }
    }
}