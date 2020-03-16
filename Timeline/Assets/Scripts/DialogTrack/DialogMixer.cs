using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialogMixer : PlayableBehaviour
{

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        float w = 0;
        for (int i = 0; i < playable.GetInputCount(); i++)
        {
            w += playable.GetInputWeight(i);
        }

        if (w == 0) (playerData as TMPro.TextMeshProUGUI).text = "";
        
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        Debug.LogFormat("DialogMixer.OnBehaviourPlay");
        base.OnBehaviourPlay(playable, info);
    }

}
