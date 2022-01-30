using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FadeController : MonoBehaviour
{
    private bool mFaded = false;
    public float duration;
    public float waitingTime;
    private CanvasGroup canvGroup;
    private CanvasGroup canvGroup2;


    [SerializeField] private Animator anim;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject level;
    [SerializeField] private GameObject highscores;
    [SerializeField] private GameObject panel;


    public void OnClicked(Button button)
    {
        canvGroup = GetComponent<CanvasGroup>();
        if (gameObject.name == "Highscores" && button.name.StartsWith("Level")) canvGroup = GameObject.Find("Scoring").GetComponent<FadeController>().menu.GetComponent<CanvasGroup>();
        Debug.Log(canvGroup);
        canvGroup.blocksRaycasts = false;
        panel.GetComponent<Image>().raycastTarget = true;
        AudioManager.Play("UI_Click");
        StartCoroutine(Lock((waitingTime + duration)*0.7f));
        StartCoroutine(DoFade(canvGroup, canvGroup.alpha, mFaded ? 1 : 0));
        mFaded = true;
        StartCoroutine(Wait(waitingTime, mFaded, button));
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

    public IEnumerator Wait(float waitingTime, bool mFaded, Button button)
    {
        yield return new WaitForSeconds(waitingTime);

        if (button.name == "SelectLevel") canvGroup2 = level.GetComponent<CanvasGroup>();
        else if (button.name == "Highscores") canvGroup2 = highscores.GetComponent<CanvasGroup>();
        else if (button.name.StartsWith("Level"))
        {
            GameObject.Find("Scoring/Lv/Text").GetComponent<Text>().text = button.GetComponentInChildren<Text>().text;
            GameObject.Find("Scoring/Deaths/Text").GetComponent<Text>().text = Random.Range(1, 30).ToString();
            GameObject.Find("Scoring/TimeToComplete/Text").GetComponent<Text>().text = Random.Range(1, 10).ToString("00") + ":" + Random.Range(1, 60).ToString("00") + ":" + Random.Range(1, 100).ToString("00");
            canvGroup2 = GameObject.Find("Scoring").GetComponent<FadeController>().menu.GetComponent<CanvasGroup>();
        }
        else canvGroup2 = menu.GetComponent<CanvasGroup>();

        GameObject.Find("Scoring").GetComponent<CanvasGroup>().alpha = 0;
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
        // UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

}
