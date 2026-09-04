using UnityEngine;

public class ForceAspectRatio : MonoBehaviour
{
    private const float TargetAspect = 16f / 9f;

    private float resizeDelay = 0.15f;
    private float resizeTimer;

    private int lastWidth;
    private int lastHeight;

    void Start()
    {
        lastWidth = Screen.width;
        lastHeight = Screen.height;
    }

    void Update()
    {
        int width = Screen.width;
        int height = Screen.height;

        // Window hasn't changed.
        if (width == lastWidth && height == lastHeight)
            return;

        // Window is currently being resized.
        lastWidth = width;
        lastHeight = height;

        // Reset the timer every time the window changes.
        resizeTimer = resizeDelay;
    }

    void LateUpdate()
    {
        if (resizeTimer <= 0)
            return;

        resizeTimer -= Time.unscaledDeltaTime;

        // Wait until resizing has stopped.
        if (resizeTimer > 0)
            return;

        int width = Screen.width;
        int height = Screen.height;

        float aspect = (float)width / height;

        if (Mathf.Approximately(aspect, TargetAspect))
            return;

        int newWidth;
        int newHeight;

        if (aspect > TargetAspect)
        {
            // Too wide → reduce width.
            newHeight = height;
            newWidth = Mathf.RoundToInt(height * TargetAspect);
        }
        else
        {
            // Too tall → reduce height.
            newWidth = width;
            newHeight = Mathf.RoundToInt(width / TargetAspect);
        }

        Screen.SetResolution(
            newWidth,
            newHeight,
            FullScreenMode.Windowed
        );
    }
}
