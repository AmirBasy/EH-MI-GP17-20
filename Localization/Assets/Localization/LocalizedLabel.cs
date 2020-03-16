using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizedLabel : MonoBehaviour
{
    public string key;
    public object[] values;

    // Start is called before the first frame update
    void OnEnable()
    {
        Text text = GetComponent<Text>();
        if (text!=null)
        {
            text.text = key.Localized();
        }
    }


}
