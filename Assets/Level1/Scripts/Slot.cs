using BagDataManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public int Index { get; private set; }

    public bool IsChosen { get; set; }

    public Image Ico { get; set; }

    void Awake()
    {
        Index = transform.GetSiblingIndex();
        Ico = transform.Find("Ico").GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        BagUI.Instance.outline.SetActive(false);
        IsChosen = !IsChosen;
        BagUI.Instance.ClickSlot(Index);
    }
}
