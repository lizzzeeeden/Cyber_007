using BagDataManager;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//物品hold都由此检测，再传递数据给Player
public class BagUI : MonoBehaviour
{
    public static BagUI Instance { get; protected set; }

    [Header("显示")]
    [SerializeField] protected Color chosenColor;
    [SerializeField] protected Transform slotParent;
    protected Slot[] slots;
    [SerializeField] protected Transform bagPanel;
    [HideInInspector] public GameObject outline;


    [Header("数据")]
    protected Bag bag;

    protected virtual void Awake()
    {
        //创建单例
        if (Instance != null) Destroy(gameObject);
        Instance = this;
    }
    protected virtual void Start()
    {
        //数据初始化
        outline = transform.GetChild(0).Find("Outline").gameObject;
        bag = LevelManager.GetBag();
        slots = slotParent.GetComponentsInChildren<Slot>();
        ReFresh();
    }


    #region hold/use/pick
    public virtual void ClickSlot(int index)
    {
        //阅读的物品
        if (index >= 0 && index < bag.GetItemList().Count && bag.GetItemList()[index].isRead) {
            bag.GetItemList()[index].Read();
        } else {
            if (index >= 0 && slots[index].IsChosen) {
                //调用bag的HoldItem函数
                Player.Instance.HoldItem(index);
                //重置slot的IsChosen
                for (int i = 0; i < slots.Length; i++) {
                    if (i != index) {
                        slots[i].IsChosen = false;
                        slots[i].GetComponent<Image>().color = Color.white;
                    } else {
                        slots[i].GetComponent<Image>().color = chosenColor;
                    }
                }

            } else {
                Player.Instance.HoldItem(-1);

                for (int i = 0; i < slots.Length; i++) {
                    slots[i].IsChosen = false;
                    slots[i].GetComponent<Image>().color = Color.white;
                }

            }
        }
    }
    //use，player调用
    public void Refresh_UseItem()
    {
        ReFresh();
    }
    //Pick，player调用
    public void Refresh_PickItem(int lastindex)
    {
        //if (lastindex >= 0) {
        //    slots[lastindex].IsChosen = true;
        //}
        //ClickSlot(lastindex);
        if(lastindex == -1) {
            ClickSlot(-1);
        }
        ReFresh();
    }
    #endregion

    #region 刷新
    protected void ReFresh()
    {
        List<Item> items = bag.GetItemList();
        if (items.Count > slots.Length) print("持有物长度超过背包长度");
        for (int i = 0; i < slots.Length; i++) {
            if (i < items.Count) {
                slots[i].Ico.sprite = items[i].ico;
                slots[i].Ico.gameObject.SetActive(true);
            } else {
                slots[i].Ico.sprite = null;
                slots[i].Ico.gameObject.SetActive(false);
            }
        }
        //重置slot的IsChosen
        for (int i = 0; i < Instance.slots.Length; i++) {
            if (i != bag.HoldingItemIndex) {
                Instance.slots[i].IsChosen = false;
                Instance.slots[i].GetComponent<Image>().color = Color.white;
            } else {
                Instance.slots[i].GetComponent<Image>().color = Instance.chosenColor;
            }
        }
    }
    #endregion

}
