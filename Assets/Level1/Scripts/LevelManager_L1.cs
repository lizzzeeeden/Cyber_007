using BagDataManager;
using DG.Tweening;
using Fungus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class LevelManager_L1 : LevelManager
{
    public Transform startPos;
    public BoxCollider2D cook;
    //花-草莓
    public BoxCollider2D flower;
    public GameObject strawberry;
    public bool isGetStrawbbry = false;
    //公鸡雕像-鸡蛋
    public GameObject eggUI;
    public BoxCollider2D cock;
    public GameObject egg;
    public bool isGetEgg = false;
    //书
    public GameObject book;
    //院长办公室
    public GameObject officeDoor_locked;
    public GameObject officeDoor_unlocked;
    //密码盒&孤儿院大门
    public GameObject lockedBoxUI;
    public GameObject corridor3;
    public GameObject garden;
    public Transform gardenLPos;
    public GameObject gateLock;


    //镜子
    public GameObject kidvva;
    public GameObject hurtvva;


    private void Awake()
    {
        Instance = this;
        player.transform.position = startPos.position;
    }
    protected override void Start()
    {
        base.Start();
        Manager.ChangeBGM(2);
    }
    public override void InitItems()
    {
        item_dict.Add("床下的罐子", new Item("床下的罐子", true, Resources.Load<Sprite>("Texture_L1/床下的罐子"), GetBottle, UseBottle, null));
        item_dict.Add("草莓", new Item("草莓", true, Resources.Load<Sprite>("Texture_L1/草莓"), GetStrawberry, null, null));
        item_dict.Add("锤子", new Item("锤子", false, Resources.Load<Sprite>("Texture_L1/锤子"), null, UseHammer, null));
        item_dict.Add("院长办公室钥匙", new Item("院长办公室钥匙", true, Resources.Load<Sprite>("Texture_L1/院长办公室钥匙"), null, OpenOffice, null));
        item_dict.Add("鸡蛋", new Item("鸡蛋", true, Resources.Load<Sprite>("Texture_L1/鸡蛋"), null, null, null));
        item_dict.Add("孤儿院大门钥匙", new Item("孤儿院大门钥匙", true, Resources.Load<Sprite>("Texture_L1/孤儿院大门钥匙"), null, OpenGate, null));
        item_dict.Add("红颜料", new Item("红颜料", true, Resources.Load<Sprite>("Texture_L1/红颜料"), null, null, null));
        item_dict.Add("相片", new Item("相片", true, Resources.Load<Sprite>("Texture_L1/相片"), null, null, null, true, OpenPhoto));

        redQueen.GetComponent<BoxCollider2D>().enabled = false;
    }

    #region 相片
    [Header("相片")]
    public GameObject photo;
    public bool OpenPhoto()
    {
        photo.SetActive(true);
        player.Froze();
        return true;
    }
    public void GetPhoto()
    {
        bag.AddItem(item_dict["相片"]);
        BagUI.Instance.Refresh_UseItem();
    }
    #endregion

    #region 镜子
    public GameObject mirrorImg;
    //bool isFirstOpenMirror = true;
    public void OpenMirror()
    {
        //if (!isFirstOpenMirror) return;

        mirrorImg.SetActive(true);
        player.Froze();
        kidvva.SetActive(false);
        hurtvva.SetActive(true);
        player.animator = player.GetComponentInChildren<Animator>();
        flowChart.ExecuteBlock("镜子");
    }
    public void CloseMirrorImg()
    {
        mirrorImg.SetActive(false);
    }
    #endregion

    #region 罐子
    public bool UseBottle(string toname)
    {
        if (toname != "草莓花") return false;

        flower.enabled = false;
        strawberry.SetActive(true);
        audioManager.PlaySE(1);

        return true;
    }

    public bool GetBottle()
    {
        player.Froze();
        flowChart.ExecuteBlock("ROBO_床下的罐子");
        return true;
    }

    public bool GetStrawberry()
    {
        isGetStrawbbry = true;
        return true;
    }
    #endregion

    #region 锤子
    public bool UseHammer(string toname)
    {
        if (toname != "公鸡的雕像" || isGetEgg) return false;
        if (!eggUI.activeSelf) return false;
        audioManager.PlaySE(2);
        egg.SetActive(true);
        isGetEgg = true;
        cock.enabled = false;
        eggUI.SetActive(false);
        player.DeFrozeMove();
        return true;
    }

    public void OpenEggUI()
    {
        player.FrozeMove();
        eggUI.SetActive(true);
    }
    #endregion

    #region 书
    public void OpenBook()
    {
        book.SetActive(true);
        player.Froze();
    }
    #endregion

    #region 获得钥匙
    public void GetOfficeKey()
    {
        bag.AddItem(item_dict["院长办公室钥匙"]);
        BagUI.Instance.Refresh_PickItem(bag.GetItemList().Count - 1);//背包UI更新
        cook.enabled = false;
    }
    #endregion

    #region 失去草莓鸡蛋
    public void LoseStrawberryAndEgg()
    {
        bag.RemoveItem(item_dict["草莓"]);
        bag.RemoveItem(item_dict["鸡蛋"]);
        BagUI.Instance.Refresh_PickItem(-1);//背包UI更新
    }
    #endregion

    #region 院长办公室
    public bool OpenOffice(string toname)
    {
        if (toname != "办公室大门") return false;
        officeDoor_locked.SetActive(false);
        officeDoor_unlocked.SetActive(true);
        return true;
    }
    #endregion

    #region 密码盒&孤儿院大门
    public void OpenLockedBoxUI()
    {
        lockedBoxUI.SetActive(true);
        player.Froze();
    }
    public void GetGateKey()
    {
        bag.AddItem(item_dict["孤儿院大门钥匙"]);
        BagUI.Instance.Refresh_PickItem(bag.GetItemList().Count - 1);//背包UI更新
    }
    public bool OpenGate(string toname)
    {
        if (toname != "孤儿院大门") return false;
        gateLock.SetActive(false);
        player.Froze();
        Invoke(nameof(OnOpenGate), 1.5f);
        return true;
    }
    private void OnOpenGate()
    {
        corridor3.SetActive(false);
        garden.SetActive(true);
        player.transform.position = gardenLPos.position;
        player.DeFroze();

        Manager.ChangeBGM(0);
    }

    #endregion

    #region 番茄
    //红颜料
    public BoxCollider2D tomato;
    public bool isGetTomato = false;
    public GameObject redRose;
    public GameObject ironRose;
    public Animator soldierAAnimator;
    public void GetTomato()
    {
        audioManager.PlaySE(11);
        bag.AddItem(item_dict["红颜料"]);//背包数据更新
        BagUI.Instance.Refresh_PickItem(bag.GetItemList().Count - 1);
        tomato.enabled = false;
        isGetTomato = true;
    }
    public void LoseTomato()
    {
        bag.RemoveItem(item_dict["红颜料"]);
        BagUI.Instance.Refresh_PickItem(-1);//背包UI更新
        redRose.SetActive(true);
        redQueen.GetComponent<BoxCollider2D>().enabled = true;
        soldierAAnimator.SetBool("paint", true);
    }
    #endregion

    #region 红皇后
    public Transform redQueen;
    public Transform toRosePos;
    public GameObject soldier;
    public GameObject hospitalBox;
    public Animator redQueenAnimator;
    public void GotoRose()
    {
        Invoke("IronRose", 3f);
        soldierAAnimator.SetBool("paint", false);
        player.footstepAudio.Play();
        redQueen.GetComponent<BoxCollider2D>().enabled = false;
        player.transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        player.animator.SetBool("xMove", true);
        redQueenAnimator.SetBool("xMove", true);
        player.transform.DOMoveX(toRosePos.position.x, 4f);
        redQueen.DOMoveX(toRosePos.position.x + 3, 4f).OnComplete(() => {
            hospitalBox.SetActive(true);
            flowChart.ExecuteBlock("红皇后2");
            player.animator.SetBool("xMove", false);
            redQueenAnimator.SetBool("xMove", false);
            player.footstepAudio.Stop();
        });
    }
    public void IronRose()
    {
        ironRose.SetActive(true);
    }
    public void StartChaseVva()
    {
        soldier.SetActive(true);
    }

    public Transform hospitalGate;
    public Image black;
    public GameObject hospitalUI;
    public void OpenHospitalGate()
    {
        player.Froze();
        soldier.GetComponent<Soldier>().isFin = true;
        //soldier.SetActive(false);
        hospitalGate.DOMoveX(hospitalGate.transform.position.x + 3.5f, 1).OnComplete(() => {
            player.gameObject.SetActive(false);
            hospitalGate.DOMoveX(hospitalGate.transform.position.x - 3.5f, 1).OnComplete(() => {
                black.color = new Color(0, 0, 0, 0);
                black.gameObject.SetActive(true);
                black.DOColor(new Color(0, 0, 0, 1), 1f).OnComplete(() => {
                    player.gameObject.SetActive(false);
                    garden.SetActive(false);
                    hospitalUI.SetActive(true);
                    black.DOColor(new Color(0, 0, 0, 0), 1.5f).OnComplete(() => {
                        black.gameObject.SetActive(false);
                        flowChart.ExecuteBlock("进入医院");
                        Manager.ChangeBGM(-1);
                    });
                });
            });

        });
    }
    #endregion

    #region 医院
    public GameObject carAccident;
    public void OpenTV()
    {
        audioManager.PlaySE(8);
        carAccident.SetActive(true);
    }
    public Transform lDoor;
    public Transform lDoorPos;
    public Transform rDoor;
    public Transform rDoorPos;
    public GameObject[] doctors;
    public void ContinueTV()
    {
        Manager.ChangeBGM(1);
        black.color = new Color(0, 0, 0, 0);
        black.gameObject.SetActive(true);
        black.DOColor(new Color(0, 0, 0, 1), 1.5f).OnComplete(() => {
            carAccident.SetActive(false);
            doctors[0].SetActive(true);
            doctors[1].SetActive(true);
            doctors[2].SetActive(true);
            black.DOColor(new Color(0, 0, 0, 0), 1.5f).OnComplete(() => {
                lDoor.DOMoveX(lDoorPos.position.x, 1.5f);
                rDoor.DOMoveX(rDoorPos.position.x, 1.5f);
                black.gameObject.SetActive(false);
                flowChart.ExecuteBlock("电视");
            }).SetDelay(0.5f);
        });
    }


    #endregion

    #region 舞台
    public GameObject chooseHatUI;

    public void ChoseHat()
    {
        black.color = new Color(0, 0, 0, 0);
        black.gameObject.SetActive(true);
        black.DOColor(new Color(0, 0, 0, 1), 1.5f).OnComplete(() => {
            hospitalUI.SetActive(false);
            chooseHatUI.SetActive(true);

            black.DOColor(new Color(0, 0, 0, 0), 1.5f).OnComplete(() => {
                black.gameObject.SetActive(false);
                flowChart.ExecuteBlock("选择帽子前");
            }).SetDelay(0.5f);
        });
    }

    public GameObject stage;
    public void End()
    {
        black.color = new Color(0, 0, 0, 0);
        black.gameObject.SetActive(true);
        black.DOColor(new Color(0, 0, 0, 1), 3f).OnComplete(() => {
            Manager.ChangeBGM(3);
            stage.SetActive(true);
            black.DOColor(new Color(0, 0, 0, 0), 3f).OnComplete(() => {
                black.gameObject.SetActive(false);
                flowChart.ExecuteBlock("End");
            }).SetDelay(0.5f);
        });
    }
    #endregion

    public void GoInterlude2()
    {
        SceneManager.LoadScene("Interlude2");
    }
}
