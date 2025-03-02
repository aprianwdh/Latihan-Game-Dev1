using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class GameManager : MonoBehaviour
{
    //untuk menyimpan data
    public int current_coin_count = 0;
    public int current_health_player = 3;
    public bool interactable = false;

    //untuk sceen manager
    public static GameManager instance;
    [SerializeField] Animator transition;
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

    public void NextSceen()
    {
        StartCoroutine(LoadSceen());
    }

    IEnumerator LoadSceen()
    {
        transition.SetTrigger("GameEnd");
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        transition.SetTrigger("GameStart");
    }
}
