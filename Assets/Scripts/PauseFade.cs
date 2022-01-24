using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseFade : MonoBehaviour
{
    public Animator anim;

    public void Pause()
    {
        anim.SetBool("Fade", true);
    }

    public void Resume()
    {
        anim.SetBool("Fade", false);
    }

}
