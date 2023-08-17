using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class POs : MonoBehaviour
{
    VisualEffect visualEffect;
    void Start()
    {
        visualEffect = this.GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        visualEffect.SetVector3("Pos", this.transform.position);
    }
}
