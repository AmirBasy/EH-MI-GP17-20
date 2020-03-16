using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class DialogAsset : PlayableAsset
{
    public DialogTrackData template;

    TextMeshProUGUI target;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var p= ScriptPlayable<DialogTrackData>.Create(graph, template);
        var b= p.GetBehaviour();

        b.Setup(target);

        return p;
    }

    public void Setup(TextMeshProUGUI target)
    {
        this.target = target;
    }
}
