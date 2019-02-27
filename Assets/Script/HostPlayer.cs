using UnityEngine;
using System.Collections;
using TetrisSupport;
using UnityEngine.UI;

public class HostPlayer : BasePlayer
{
    static float autoDownInter = 1;

    static float speedChangeTime = 7;

    static float speedDownT = 0.5f;

    static float speedUpT = 2;

    HostShape curShape;

    HostShape nextShape;

    HostShape nextNextShape;

    ItemContainer iContainer;

    Button readyBtn;

    TeamSwitcher teamSwitcher;

    float readyTime;

    bool waitFailure = false;

    float curDownRemain = autoDownInter;

    float curSpeedT = 1;

    float recoverRemain = 0;

    public HostPlayer(RectTransform hostRoot, int relativeTbIndex)
        : base(hostRoot.Find("Map_pt/PlayerPanel") as RectTransform, relativeTbIndex)
    {
        curShape = new HostShape(hostRoot.FindChild("Map_pt/CurrentShape") as RectTransform);
        nextShape = new HostShape(hostRoot.FindChild("ShapeTip/NextShape_G/NextShape") as RectTransform);
        nextNextShape = new HostShape(hostRoot.FindChild("ShapeTip/NextNextShape_G/NextNextShape") as RectTransform);
        iContainer = new ItemContainer(hostRoot.FindChild("ItemContainer") as RectTransform);
        readyBtn = hostRoot.Find("Map_pt/Ready_btn").GetComponent<Button>();
        readyBtn.onClick.AddListener(this.OnClickReady);
        teamSwitcher = hostRoot.GetComponentInChildren<TeamSwitcher>();
        teamSwitcher.OnClickTeam += OnClickSwitchTeam;
    }

    public override void PlayerReadyOrCancel(bool isReady)
    {
        if (base.state == PlayerState.GameOver)
        {
            this.curShape.Shape = null;
            this.nextShape.Shape = null;
            this.nextNextShape.Shape = null;
        }
        base.PlayerReadyOrCancel(isReady);
        this.teamSwitcher.gameObject.SetActive(!isReady);
    }

    public override void GameReady()
    {
        base.GameReady();
        this.iContainer.Clear();
        this.readyBtn.gameObject.SetActive(false);
        readyTime = ConstData.GameReadyDuration;
    }

    public override void GameStart(AllShapeInfo allShape)
    {
        base.GameStart(allShape);
        RefreshAllShape(allShape);
        curDownRemain = autoDownInter;
    }



    public override void SPutShape(SPutShape sps)
    {
        if (nextNextShape.Shape == null)
        {
            nextNextShape.Shape = Shape.Create(sps.NextNextShapeFlag);
        }
        else if (nextShape.Shape == null)
        {
            nextShape.Shape = Shape.Create(sps.NextNextShapeFlag);
        }
        else
        {
            Debug.LogError("SPutShape but shape tip is full");
        }
        this.map.ApplyItemFlagOnMap(sps.ItemFlag);
    }

    public override void ReceiveItem(byte itemType)
    {
        base.ReceiveItem(itemType);

        float curT;
        switch ((ItemType)itemType)
        {
            case ItemType.SpeedDown:
                curT = speedDownT;
                break;
            case ItemType.SpeedUp:
                curT = speedUpT;
                break;
            default:
                return;
        }
        if (curT != this.curSpeedT)
        {
            this.recoverRemain = 0;
            this.curSpeedT = curT;
        }
        this.recoverRemain += speedChangeTime;
    }


    public override void GameOver()
    {
        base.GameOver();
        this.readyBtn.gameObject.SetActive(true);
        this.teamSwitcher.gameObject.SetActive(true);
        this.waitFailure = false;
    }



    public void RefreshHoleMap(SRefreshHoleMap sfhm)
    {
        this.RefreshAllShape(sfhm.AllShape);
        map.RefreshHoleMap(sfhm.AllLines, sfhm.Items);
    }

    public void AddItemFlag(byte itemFlag)
    {
        map.ApplyItemFlagOnMap(itemFlag);
    }

    public void EatItem(byte itemType)
    {
        iContainer.AddItem(itemType);
    }

    public override void Failure()
    {
        base.Failure();
        this.waitFailure = false;
    }



    public void Tick(Command cmd)
    {
        if (waitFailure)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            this.ReceiveItem((byte)ItemType.SpeedDown);   
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            this.ReceiveItem((byte)ItemType.SpeedUp);   
        }

        if (state == PlayerState.Gaming)
        {
            RunCommand(cmd);
            curDownRemain -= Time.deltaTime * curSpeedT;
            if (curDownRemain < 0)
            {
                RunCommand(new Command(CommandType.MoveDown, 0));
            }
            if (curSpeedT != 1)
            {
                this.recoverRemain -= Time.deltaTime;
                if (this.recoverRemain < 0)
                {
                    curSpeedT = 1;
                }
            }
        }
    }

    public void TickReadyTime()
    {
        readyTime -= Time.deltaTime;
        string showTime;
        if (readyTime < 1)
        {
            showTime = "Go";
        }
        else
        {
            showTime = Mathf.FloorToInt(readyTime).ToString();
        }
        readyTxt.text = showTime;
    }


    void RunCommand(Command cmd)
    {
        if (cmd == null)
        {
            return;
        }
        switch (cmd.CmdType)
        {
            case CommandType.Turn:
                map.Turn(curShape, 1);
                break;
            case CommandType.MoveLeft:
                map.MoveByOffset(curShape, Position.Left);
                break;
            case CommandType.MoveRight:
                map.MoveByOffset(curShape, Position.Right);
                break;
            case CommandType.MoveDown:
                if (!map.MoveByOffset(curShape, Position.Down))
                {
                    PutCurShape();
                }
                curDownRemain = autoDownInter;
                break;
            case CommandType.DownDirectly:
                for (int i = 0; i < Map.RowCount; i++)
                {
                    if (!map.MoveByOffset(curShape, Position.Down))
                    {
                        PutCurShape();
                        break;
                    }
                }
                break;
            case CommandType.SwitchItem:
                iContainer.SwitchItem();
                break;
            case CommandType.UseItem:
                UseItem(cmd.CmdArgs);
                break;
            default:
                Debug.LogError("Command not support : " + cmd);
                break;
        }
    }

    private void RefreshAllShape(AllShapeInfo allShape)
    {
        Shape shape = Shape.Create(allShape.CurShapeFlag);
        _SetCurShape(shape);
        nextShape.Shape = Shape.Create(allShape.NextShapeFlag);
        nextNextShape.Shape = Shape.Create(allShape.NextNextShapeFlag);
    }

    void UseItem(byte realtiveIndex)
    {
        if (iContainer.ItemCount > 0)
        {
            int globalIndex = this.TbIndex + (realtiveIndex - 1);
            if (globalIndex < 0)
            {
                globalIndex += ConstData.PlayerCount;
            }
            byte itemType = iContainer.PullFirstItem();
            PlayerManager.Instance.ReqUseItem(realtiveIndex, itemType);
        }
    }

    void PutCurShape()
    {
        map.PutOneShapeOnMap(curShape.Shape);
        ProtocolsSender.PutShape((byte)curShape.Shape.CurTurnIndex, curShape.Shape.Pos);

        Shape nShape = nextShape.Shape;
        nextShape.Shape = nextNextShape.Shape;
        nextNextShape.Shape = null;
        _SetCurShape(nShape);
    }


    private void _SetCurShape(Shape shape)
    {
        if (shape != null)
        {
            shape.Pos = Map.StartPos;
        }
        curShape.Shape = shape;
        if (shape != null && !this.map.CheckShapeLegal(shape))
        {
            ProtocolsSender.SendFailure();
        }
    }

    void OnClickReady()
    {
        ProtocolsSender.Ready(base.state == PlayerState.Ready ? false : true);
    }

    void OnClickSwitchTeam(byte newTeamId)
    {
        if (base.teamId != newTeamId)
        {
            ProtocolsSender.ChangeTeam(newTeamId);
        }
    }
}

