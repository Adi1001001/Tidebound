using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class CurrentArrowResizer
{
#if UNITY_EDITOR

    [MenuItem("Tools/Apply Compact Arrow Size")]
    private static void ApplyCompactArrowSize()
    {
        CurrentArrowGenerator[] currents =
            Object.FindObjectsByType<CurrentArrowGenerator>();

        if (currents.Length == 0)
        {
            Debug.LogWarning(
                "No CurrentArrowGenerator objects found."
            );
            return;
        }


        foreach (CurrentArrowGenerator current in currents)
        {
            Undo.RecordObject(
                current,
                "Apply Compact Arrow Size"
            );

            current.arrowWidthPixels *= 0.8f;
            current.horizontalGapPixels *= 1.25f;
            current.arrowHeightPixels *= 0.8f * (11f / 18f);
            current.verticalGapPixels *= 1.25f * (18f / 11f);

            EditorUtility.SetDirty(current);
        }


        AssetDatabase.SaveAssets();

        Debug.Log(
            $"Applied compact arrow size to {currents.Length} CurrentArrowGenerator objects."
        );
    }

#endif
}