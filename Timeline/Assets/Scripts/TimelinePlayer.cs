using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelinePlayer : MonoBehaviour
{
    public PlayableDirector apertura;
    public PlayableDirector chiusura;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in apertura.playableAsset.outputs)
        {
            Debug.LogFormat("Traccia: {0} ", item.streamName);
            //apertura.SetGenericBinding(item.sourceObject, this.gameObject);
        }



    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            apertura.Play();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            chiusura.Play();
        }

    }
}
