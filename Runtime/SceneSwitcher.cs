using Codice.CM.Client.Differences.Merge;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static SwitchInfo;

public class SwitchInfo
{
    public enum FADE_TYPE
    {
        LINEAR,
        EASE_IN,
        EASE_OUT
    }

    public Color ColorToFadeInto;
    public float InTime;
    public float OutTime;

    public FADE_TYPE InFadeType;
    public FADE_TYPE OutFadeType;

    public SwitchInfo(Color color, float inTime, float outTime, FADE_TYPE inFadeType, FADE_TYPE outFadeType)
    {
        ColorToFadeInto = color;
        InTime = inTime;
        OutTime = outTime;
        InFadeType = inFadeType;
        OutFadeType = outFadeType;
    }
}
public class FadeData
{
    public Color InColor;
    public float TransitionTime;
    public FADE_TYPE FadeType;

    public FadeData(Color inColor, float transitionTime, FADE_TYPE fadeType)
    {
        InColor = inColor;
        TransitionTime = transitionTime;
        FadeType = fadeType;
    }
}
public class SceneSwitcher : MonoBehaviour
{
    public static SceneSwitcher Instance;

    [SerializeField] 
    private UIDocument Document;

    private VisualElement RootElement;

    private bool IsTransitioning = false;

    public Action OnSceneSwitchStart;
    public Action OnSceneSwitchEnd;

    public static SceneSwitcher GetInstance()
    {
        if (Instance == null)
        {
            GameObject obj = Resources.Load<GameObject>("SceneSwitch");
            Instantiate(obj);
        }
        return Instance;
    }
    private void Awake()
    {
        if (Instance)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        RootElement = Document.rootVisualElement;

        if (RootElement == null)
        {
            LogWarning("Root Element was null");
        }
        DontDestroyOnLoad(this.gameObject);

    }

    private void LogWarning(string message)
    {
        Debug.LogWarning("IchiSceneSwitcher=>" + message);
    }
    public void SwitchToScene(string sceneName, SwitchInfo switchInfo)
    {
        if (IsTransitioning)
        {
            LogWarning("Scene transition already in progress");
            return;
        }
        if (RootElement == null)
        {
            LogWarning("Root Element was null, cannot switch scene");
            return;
        }

        StartCoroutine(CompleteTransition(sceneName, 
            new FadeData(RootElement.style.backgroundColor.value, switchInfo.InTime, switchInfo.InFadeType), 
            new FadeData(switchInfo.ColorToFadeInto, switchInfo.OutTime, switchInfo.OutFadeType)));


    }

    private IEnumerator CompleteTransition(string nextScene, FadeData inData, FadeData outData)
    {
        IsTransitioning = true;
        OnSceneSwitchStart?.Invoke();

        yield return FadeToColor(inData.InColor, outData.InColor, inData.TransitionTime, inData.FadeType);
        Debug.Log("Move to new scene");

        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(nextScene);
        asyncOp.allowSceneActivation = true;

        while (!asyncOp.isDone)
            yield return null;

        yield return FadeToColor(outData.InColor, inData.InColor, outData.TransitionTime, outData.FadeType);
        IsTransitioning = false;
        OnSceneSwitchEnd?.Invoke();

    }

    public IEnumerator FadeToColor(Color original, Color target, float timeToTransition = 1.2f, FADE_TYPE fadeType = FADE_TYPE.LINEAR)
    {
        float progress = 0.0f;
        while (timeToTransition > progress)
        {
            progress += Time.deltaTime;
            float weight = (progress / timeToTransition);

            RootElement.style.backgroundColor = Color.Lerp(original, target, GetWeightFromFadeType(weight, fadeType));
            yield return null;
        }
    }

    private float GetWeightFromFadeType(float weight, FADE_TYPE fadeType)
    {
        switch (fadeType)
        {
            case FADE_TYPE.LINEAR:
                return weight;

            case FADE_TYPE.EASE_IN:
                return 1 - Mathf.Cos(weight * Mathf.PI * 0.5f);

            case FADE_TYPE.EASE_OUT:
                return Mathf.Sin(weight * Mathf.PI * 0.5f);
        }
        return weight;
    }
}
