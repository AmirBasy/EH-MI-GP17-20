using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackClipType(typeof(DialogAsset))]
[TrackBindingType(typeof(TextMeshProUGUI))]
[TrackColor(.7f, .5f, .1f)]
public class DialogTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        PlayableDirector playableDirector = go.GetComponent<PlayableDirector>();

        var target = playableDirector.GetGenericBinding(this) as TextMeshProUGUI;

        foreach (var clip in GetClips())
        {
            var asset = clip.asset as DialogAsset;
            asset.Setup(target);
        }

        // creo li mixer
        var p = ScriptPlayable<DialogMixer>.Create(graph, inputCount);
        var b = p.GetBehaviour() as DialogMixer;

        return p;
    }
}
