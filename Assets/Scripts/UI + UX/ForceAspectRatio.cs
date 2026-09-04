using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ForceAspectRatio : MonoBehaviour
{
    private const float TargetAspect = 16f / 9f;

    private Camera cam;

    private int lastScreenWidth;
    private int lastScreenHeight;

    private void Start()
    {
        cam = GetComponent<Camera>();

        UpdateAspectRatio();

        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
    }

    private void Update()
    {
        if (Screen.width != lastScreenWidth ||
            Screen.height != lastScreenHeight)
        {
            UpdateAspectRatio();

            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;
        }
    }

    private void UpdateAspectRatio()
    {
        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / TargetAspect;

        if (scaleHeight < 1f)
        {
            cam.rect = new Rect(0f, (1f - scaleHeight) / 2f, 1f, scaleHeight);
        }
        else
        {
            float scaleWidth = 1f / scaleHeight;
            cam.rect = new Rect((1f - scaleWidth) / 2f, 0f, scaleWidth, 1f);
        }
    }
}
