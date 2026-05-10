using UnityEngine;
using TMPro;
using System.Collections;

public class TitleScreen : MonoBehaviour
{
    public GameObject titleScreenCanvas;
    public GameObject cutsceneCanvas;
    public TextMeshProUGUI cutsceneText;
    public Player player;
    public Camera cutSceneCamera;

    private string[] lines = {
        "In a small little village near the castle...",
        "a little witch with her frog decided to brew the elixir of life...",
        "And amidst all chaos she fell in her own cauldron...",
        "The cauldron turned alive and is really... realllyyy angry",
    };

    public void StartGame()
    {
        titleScreenCanvas.SetActive(false);
        StartCoroutine(PlayCutscene());
    }
    public void RetryGame()
    {
        titleScreenCanvas.SetActive(false);
        player.gameObject.SetActive(true);
    }

    private IEnumerator PlayCutscene()
    {
        cutsceneCanvas.SetActive(true);

        foreach (string line in lines)
        {
            cutsceneText.text = line;

            // fade in
            for (float t = 0; t < 1f; t += Time.deltaTime)
            {
                cutsceneText.alpha = t;
                yield return null;
            }

            yield return new WaitForSeconds(1.5f);

            // fade out
            for (float t = 1f; t > 0f; t -= Time.deltaTime)
            {
                cutsceneText.alpha = t;
                yield return null;
            }
        }
        cutSceneCamera.gameObject.SetActive(false);
        cutsceneCanvas.SetActive(false);
        player.gameObject.SetActive(true);

        // start your game here, e.g. enable player, start spawners etc.
    }
}


