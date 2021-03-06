using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Player_L1 : Player
{
    #region 流程数据
    LevelManager_L1 l1Manager;
    #endregion

    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;

        l1Manager = (LevelManager_L1)levelManager;
    }


    public bool isHos;
    protected override void Update()
    {
        base.Update();
        //特殊交互物品，在子类中写
        if (Input.GetKeyDown(KeyCode.E) && coll && coll.CompareTag(INTERACTIVEITEM)) {
            switch (coll.name) {
                case "镜子":
                    l1Manager.OpenMirror();
                    break;
                case "书":
                    l1Manager.OpenBook();
                    break;
                case "草莓花":
                    Talk();
                    break;
                case "密码盒":
                    l1Manager.OpenLockedBoxUI();
                    break;
                case "公鸡的雕像":
                    l1Manager.OpenEggUI();
                    break;
                case "死门":
                    l1Manager.audioManager.PlaySE(4);
                    break;
                case "孤儿院大门":
                    if (!froze) l1Manager.audioManager.PlaySE(4);
                    break;
                case "办公室大门":
                    if (!froze) l1Manager.audioManager.PlaySE(5);
                    break;

            }
        }


        if (coll != null && coll.name == "医院门" &&!isHos) {
            l1Manager.OpenHospitalGate();
            isHos = true;
        }
    }

    bool bFirstToCook = true;
    bool bFirstToSoldier = true;
    protected override void Talk()
    {
        base.Talk();
        switch (coll.name) {
            case "小孩A":
                flowChart.ExecuteBlock("小孩A");
                break;
            case "小孩B":
                flowChart.ExecuteBlock("小孩B");
                break;
            case "厨师":
                if (bFirstToCook) {
                    flowChart.ExecuteBlock("Cook_First");
                    bFirstToCook = false;
                } else {
                    if (l1Manager.isGetEgg && l1Manager.isGetStrawbbry) {
                        flowChart.ExecuteBlock("Cook_Both");

                    } else {
                        flowChart.ExecuteBlock("Cook_Repeat");
                    }
                }
                break;
            case "草莓花":
                flowChart.ExecuteBlock("ROBO_草莓花");
                break;
            case "士兵":
                if (bFirstToSoldier) {
                    flowChart.ExecuteBlock("士兵_First");
                    bFirstToSoldier = false;
                } else {
                    if (l1Manager.isGetTomato) {
                        flowChart.ExecuteBlock("士兵_获得红颜料");

                    } else {
                        flowChart.ExecuteBlock("士兵_Repeat");
                    }
                }
                break;
            case "番茄":
                flowChart.ExecuteBlock("番茄");
                break;
            case "红皇后":
                flowChart.ExecuteBlock("红皇后");
                break;
        }
    }
}

