using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLocalization : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int sold = Random.Range(2000, 230123);

        Debug.Log( "#MENU_SOLD".Localized(sold) );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
