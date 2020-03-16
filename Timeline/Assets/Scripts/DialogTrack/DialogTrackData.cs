using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class DialogTrackData : PlayableBehaviour
{
    public string message;
    public float fade;
    public float characterPerSeconds = 20;

    TextMeshProUGUI target;

    public void Setup(TextMeshProUGUI target)
    {
        this.target = target;
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        //Debug.LogFormat("Process frame at time:{0}", playable.GetTime());

        TextMeshProUGUI target = playerData as TextMeshProUGUI;
        
        target.maxVisibleCharacters =(int)( target.textInfo.characterCount * playable.GetTime() * characterPerSeconds);


        base.ProcessFrame(playable, info, playerData);
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        target.text = message;
        Debug.LogFormat("DialogTrackData.OnBehaviourPlay");
        base.OnBehaviourPlay(playable, info);
    }

    
}
