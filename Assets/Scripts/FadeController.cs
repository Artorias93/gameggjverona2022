using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FadeController : MonoBehaviour
{
    private bool mFaded = false;
    public float duration;
    public float waitingTime;

    [SerializeField] private Animator anim;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject panel;

    public void OnClickedStart()
    {
        var canvGroup = GetComponent<CanvasGroup>();
        canvGroup.blocksRaycasts = false;
        panel.GetComponent<Image>().raycastTarget = true;
        StartCoroutine(Lock((waitingTime + duration)*0.7f));
        StartCoroutine(DoFade(canvGroup, canvGroup.alpha, mFaded ? 1 : 0));
        mFaded = true;
        StartCoroutine(Wait(waitingTime, mFaded));
        mFaded = false;
    }

    public IEnumerator DoFade(CanvasGroup canvGroup, float start, float end)
    {
        float counter = 0f;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            canvGroup.alpha = Mathf.Lerp(start, end, counter / duration);
            yield return null;
        }
    }

    public IEnumerator Wait(float waitingTime, bool mFaded)
    {
        var canvGroup2 = menu.GetComponent<CanvasGroup>();
        yield return new WaitForSeconds(waitingTime);
        canvGroup2.blocksRaycasts = true;
        StartCoroutine(DoFade(canvGroup2, canvGroup2.alpha, mFaded ? 1 : 0));
        yield return null;
    }

    public IEnumerator Lock(float waitingLock)
    {
        yield return new WaitForSeconds(waitingLock);
        panel.GetComponent<Image>().raycastTarget = false;
    }

    public void StartGame()
    {
        anim.SetBool("Fade", true);
    }

    public void Exit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

}
