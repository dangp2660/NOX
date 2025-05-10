using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] GameObject gamecheck;
    public static SceneController instance;
    public Animator Transition;
    [SerializeField] GameObject player;
    [SerializeField] GameObject spawnPoint;
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
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Destroy(gamecheck);
        }
    }
    public void NextLevel()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
        StartCoroutine(LoadLevel());
        player.transform.position = spawnPoint.transform.position;  
    }

    public void LoadScene(string name)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
        StartCoroutine(LoadSceneFade(name));
        player.transform.position = spawnPoint.transform.position;
    }
    public void LoadScene(int id)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
        StartCoroutine(LoadSceneFade(id));
        player.transform.position = spawnPoint.transform.position;
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
}
