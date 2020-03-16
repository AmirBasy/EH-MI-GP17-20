using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackClipType(typeof(LightControlAsset))]
[TrackBindingType(typeof(Light))]
[TrackColor(.7f,.5f,.1f)]
public class LightControlTrack : TrackAsset
{
    public Color defaultColor = Color.white;
    public float defaultIntensity = 1;

    LightControlMixerBehaviour cache;

    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        var p= ScriptPlayable<LightControlMixerBehaviour>.Create(graph, inputCount);

        cache = p.GetBehaviour();

        cache.Init(this);

        return p;
    }


}
