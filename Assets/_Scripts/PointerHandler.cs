using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData) => 
        CursorManager.Instance.SetGameCursor(Cursors.ButtonHover);
    
    public void OnPointerExit(PointerEventData eventData) => 
        CursorManager.Instance.SetGameCursor(Cursors.Basic);
}