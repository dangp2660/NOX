using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    [SerializeField] private Animator Transition;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void NextLevel()
    {
        StartCoroutine(LoadLevel());
    }

    public void LoadScene(string name)
    {
        StartCoroutine(LoadSceneFade(name));
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    IEnumerator LoadLevel()
    {
        Transition.SetTrigger(AnimationStringList.End);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        Transition.SetTrigger(AnimationStringList.Start);
    }
    IEnumerator LoadSceneFade(string name)
    {
        Transition.SetTrigger(AnimationStringList.End);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(name);
        Transition.SetTrigger(AnimationStringList.Start);
    }

}
