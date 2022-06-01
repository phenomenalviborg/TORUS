using System;
using atomtwist.AudioNodes;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class AudioBlendClip : PlayableAsset, ITimelineClipAsset
{
    public AudioBlendBehaviour template = new AudioBlendBehaviour();
    public ExposedReference<Transform> playAtTransform = new ExposedReference<Transform>();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<AudioBlendBehaviour>.Create(graph, template);

        var clone = playable.GetBehaviour();

        // set guid
        //var guid = Guid.NewGuid().ToString();

        /*var guid =SoundSystem.GetUniqueInt;
        clone.soundSettings.soundId = guid;*/

        // playAtTransform
        clone.soundSettings.playAtTransform = playAtTransform.Resolve(graph.GetResolver());

        //Debug.Log("[AudioBlendClip] CreatePlayable " + owner.name +  owner);
        return playable;
    }

}
