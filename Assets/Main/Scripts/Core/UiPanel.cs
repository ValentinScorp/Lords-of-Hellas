using UnityEngine;
public class UiPanel : MonoBehaviour
{
    protected virtual void Awake()
    {
        SceneUiRegistry.Register(GetType(), this);
    }

    protected virtual void OnDestroy()
    {
        SceneUiRegistry.Unregister(GetType());
    }
    public void Show(bool show)
    {
        gameObject.SetActive(show);
    }
}
