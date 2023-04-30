using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RDTimer : MonoBehaviour
{
    [SerializeField] private ReactionDiffusion rd;

    int frames = 1;
    int frameTimer;

    void Update()
    {
        if (frames == frameTimer)
            rd.enabled = true;

        frameTimer++;
    }
}
