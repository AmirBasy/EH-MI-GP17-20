using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LightControlMixerBehaviour : PlayableBehaviour
{
    LightControlTrack track;

    public void Init(LightControlTrack track)
    {
        this.track = track;
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        Light light = playerData as Light;

        int trackCount = playable.GetInputCount();

        float totalIntensity = 0;
        Color finalColor = Color.black;

        float totalWeight = 0;
                

        for (int i = 0; i < trackCount; i++)
        {
            float clipWeight = playable.GetInputWeight(i);
            totalWeight += clipWeight;
            var clip = (ScriptPlayable<LightControlBehaviuor>) playable.GetInput(i);
            double duration=clip.GetDuration();

            LightControlBehaviuor clipBehaviour = clip.GetBehaviour();

            totalIntensity += clipBehaviour.intensity * clipWeight;
            finalColor += clipBehaviour.color * clipWeight;
        }

        if (totalWeight==0)
        {
            light.color = track.defaultColor;
            light.intensity = track.defaultIntensity;
        }
        else
        {
            light.color = finalColor;
            light.intensity = totalIntensity;
        }
    }
}
