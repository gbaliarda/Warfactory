using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    #region UI_ACTIONS
    public event Action<int> OnHotbarSlotChange;
    public event Action<GameObject> OnHotbarItemSelect;
    public void EventHotbarSlotChange(int idx)
    {
        OnHotbarSlotChange?.Invoke(idx);
    }
    
    public void EventHotbarItemSelect(GameObject idx)
    {
        OnHotbarItemSelect?.Invoke(idx);
    }
    #endregion
}
