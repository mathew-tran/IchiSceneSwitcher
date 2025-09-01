using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public Animator mAnimator;
    public static SceneSwitcher mInstance;

    public static SceneSwitcher GetInstance()
    {
        if (mInstance == null)
        {
            GameObject obj = Resources.Load<GameObject>("SceneSwitch");
            Instantiate(obj);
        }
        return mInstance;
    }
    private void Awake()
    {
        if (mInstance)
        {
            Destroy(this.gameObject);
            return;
        }
        mInstance = this;
        DontDestroyOnLoad(this.gameObject);

    }

    public void SwitchToScene(string sceneName)
    {
        Debug.Log("Switch to scene " + sceneName);
        StartCoroutine(CompleteTransition(sceneName));
        Debug.Log("Start Fade Transition");


    }

    private IEnumerator CompleteTransition(string nextScene)
    {
        yield return new WaitForSeconds(.1f);
        Debug.Log("Move to new scene");
        SceneManager.LoadScene(nextScene);
        
    }
}
