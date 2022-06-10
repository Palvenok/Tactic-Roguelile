using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Button attackButton;
    [SerializeField] private Button skipButton;
    [SerializeField] private Button leftArrow;
    [SerializeField] private Button rightArrow;

    public UnityEvent<KeyCode> OnKeyDown;

    private void Start()
    {
        leftArrow.onClick.AddListener(() => { OnKeyDown?.Invoke(Keys.leftKey); });
        rightArrow.onClick.AddListener(() => { OnKeyDown?.Invoke(Keys.rightKey); });
        attackButton.onClick.AddListener(() => { OnKeyDown?.Invoke(Keys.attackKey); });
        skipButton.onClick.AddListener(() => { OnKeyDown?.Invoke(Keys.skipKey); });
    }

    private void Update()
    {
        if (Input.GetKeyDown(Keys.leftKey)) OnKeyDown?.Invoke(Keys.leftKey);
        if (Input.GetKeyDown(Keys.rightKey)) OnKeyDown?.Invoke(Keys.rightKey);
        if (Input.GetKeyDown(Keys.attackKey)) OnKeyDown?.Invoke(Keys.attackKey);
        if (Input.GetKeyDown(Keys.skipKey)) OnKeyDown?.Invoke(Keys.skipKey);
    }

    private void OnDestroy()
    {
        OnKeyDown.RemoveAllListeners();
    }
}

public static class Keys
{
    public static KeyCode leftKey = KeyCode.A;
    public static KeyCode rightKey = KeyCode.D;
    public static KeyCode attackKey = KeyCode.Space;
    public static KeyCode skipKey = KeyCode.LeftControl;
}
