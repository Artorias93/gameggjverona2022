using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FadeController : MonoBehaviour
{
    //public Animator anim;
    private bool mFaded = false;
    public float duration = 0.4f;

    public void OnClicked()
    {
        //anim = button.animator;
        //anim.SetBool("Run", true);
        var canvGroup = GetComponent<CanvasGroup>();
        StartCoroutine(DoFade(canvGroup, canvGroup.alpha, mFaded ? 1 : 0));
        mFaded = true;
        //anim.SetBool("Clicked", true);
    }

    public IEnumerator DoFade(CanvasGroup canvGroup, float start, float end)
    {
        float counter = 0f;

        while(counter < duration)
        {
            counter += Time.deltaTime;
            canvGroup.alpha = Mathf.Lerp(start, end, counter / duration);

            yield return null;
        }
    }

    public void Exit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

}
