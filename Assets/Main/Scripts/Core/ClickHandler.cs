using UnityEngine;

public abstract class ClickHandler : System.IDisposable
{
    protected UserInputController Input { get; private set; }

    protected ClickHandler()
    {
        Input = ServiceLocator.Get<UserInputController>();

        if (Input != null)
            Input.OnMouseClick += HandleClick;
        else
            Debug.LogError("UserInputController not found for ClickHandler");
    }
    public virtual void Dispose()
    {
        if (Input != null)
            Input.OnMouseClick -= HandleClick;
    }

    protected abstract void HandleClick(UnityEngine.InputSystem.InputAction.CallbackContext ctx);
}
