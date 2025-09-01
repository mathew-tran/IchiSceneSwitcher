# IchiSceneSwitcher
A unity scene switcher plugin

# How to use

Example of how to use it

```csharp
using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        var sceneSwitcher = SceneSwitcher.GetInstance();
        sceneSwitcher.OnSceneSwitchStart += OnSceneSwitchStart;
        sceneSwitcher.OnSceneSwitchEnd += OnSceneSwitchEnd;
        sceneSwitcher.SwitchToScene("SampleScene2", new SwitchInfo(Color.white, 
        1.2f, 
        1.3f, 
        SwitchInfo.FADE_TYPE.EASE_IN, 
        SwitchInfo.FADE_TYPE.LINEAR));
    }

    public void OnSceneSwitchStart()
    {
        Debug.Log("Hide UI");
    }

    public void OnSceneSwitchEnd()
    {
        Debug.Log("Show UI");
    }

    private void OnDestroy()
    {
        var sceneSwitcher = SceneSwitcher.GetInstance();
        sceneSwitcher.OnSceneSwitchStart -= OnSceneSwitchStart;
        sceneSwitcher.OnSceneSwitchEnd -= OnSceneSwitchEnd;
    }

}
```