using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitlesScirpt : MonoBehaviour
{
    [SerializeField] private RectTransform textTransform;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TitlesProcess());
    }

    private IEnumerator TitlesProcess()
    {
        while (textTransform.localPosition.y < 1500)
        {
            textTransform.localPosition = new Vector3(0, textTransform.localPosition.y+100*Time.deltaTime, 0);
            yield return null;
        }
        GoToMenu();
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
