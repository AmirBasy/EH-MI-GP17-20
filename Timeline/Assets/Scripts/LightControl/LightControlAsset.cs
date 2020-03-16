using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LightControlAsset : PlayableAsset
{

    public LightControlBehaviuor template;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<LightControlBehaviuor>.Create(graph,template);

        /*
        var lightControlBehaviour = playable.GetBehaviour();
        //lightControlBehaviour.light = light.Resolve(graph.GetResolver());
        lightControlBehaviour.color = color;
        lightControlBehaviour.intensity = intesity;
        */

        return playable;
    }

}
