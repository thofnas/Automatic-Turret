using UnityEngine;
using UnityEngine.UI;

public class ContentFitterRefresh : MonoBehaviour
{
    private void Start()
    {
        RefreshContentFitters();
    }
 
    public void RefreshContentFitters()
    {
        var rectTransform = (RectTransform)transform;
        RefreshContentFitter(rectTransform);
    }
 
    public static void RefreshContentFitter(RectTransform rectTransform)
    {
        if (rectTransform == null || !rectTransform.gameObject.activeSelf)
            return;
        
        foreach (RectTransform child in rectTransform)
        {
            RefreshContentFitter(child);
        }
 
        var layoutGroup = rectTransform.GetComponent<LayoutGroup>();
        var contentSizeFitter = rectTransform.GetComponent<ContentSizeFitter>();
        
        if (layoutGroup != null)
        {
            layoutGroup.SetLayoutHorizontal();
            layoutGroup.SetLayoutVertical();
        }
 
        if (contentSizeFitter != null) 
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }
}