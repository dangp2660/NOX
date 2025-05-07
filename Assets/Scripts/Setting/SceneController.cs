using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    public Animator Transition;
    private void Awake()
    {
        // Nếu đã tồn tại một instance, hủy game object mới
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Kiểm tra nếu là scene menu thì hủy đối tượng
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0 || scene.name == "Menu")
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
    public void LoadScene(int id)
    {
        StartCoroutine(LoadSceneFade(id));
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
    IEnumerator LoadSceneFade(int id)
    {
        Transition.SetTrigger(AnimationStringList.End);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(id);
        Transition.SetTrigger(AnimationStringList.Start);
    }

    public void LoadGame()
    {
        SaveManager.Load();
    }

}
