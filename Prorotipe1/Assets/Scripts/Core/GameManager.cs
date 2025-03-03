using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // Untuk menyimpan data
    public int current_coin_count = 0;
    public int current_health_player = 3;
    public bool interactable = false;

    // Singleton instance
    public static GameManager instance;

    [SerializeField] private Animator transition;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (transition != null)
            {
                DontDestroyOnLoad(transition.gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NextScene(int buildIndex)
    {
        StartCoroutine(LoadScene(buildIndex));
    }

    private IEnumerator LoadScene(int buildIndex)
    {
        if (transition != null)
        {
            transition.SetTrigger("GameEnd");
        }

        yield return new WaitForSeconds(1);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(buildIndex);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f); // Tambahan delay agar transisi lebih smooth

        if (transition != null)
        {
            transition.SetTrigger("GameStart");
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Cari ulang Animator jika hilang saat scene berganti
        if (transition == null)
        {
            transition = FindAnyObjectByType<Animator>();
        }
    }
}
