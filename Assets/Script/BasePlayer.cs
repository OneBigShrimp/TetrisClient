using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TetrisSupport;

public abstract class BasePlayer
{
    public byte TbIndex
    {
        get
        {
            return this.tbIndex;
        }
    }

    public int RelativeIndex { get; private set; }

    protected ClientMap map;

    protected bool isReady { get; private set; }

    protected byte teamId { get; private set; }

    protected PlayerState state { get; private set; }

    protected Text readyTxt { get; private set; }

    public bool HasPlayer
    {
        get
        {
            return this.tbIndex != byte.MaxValue;
        }
    }

    byte tbIndex = byte.MaxValue;

    GameObject titleGo;

    Image teamImg;

    Text nameTxt;

    string _name;


    public BasePlayer(RectTransform playerPanel, int relativeIndex)
    {
        map = new ClientMap(playerPanel);
        Transform titleTran = playerPanel.Find("PlayerInfo");
        titleGo = titleTran.gameObject;
        teamImg = titleTran.Find("Team").GetComponent<Image>();
        nameTxt = titleTran.Find("Name").GetComponent<Text>();
        readyTxt = playerPanel.Find("Ready_txt").GetComponent<Text>();
        this.RelativeIndex = relativeIndex;
        titleTran.Find("TbIndex").GetComponent<Text>().text = relativeIndex.ToString();
        this.titleGo.SetActive(false);
    }

    public void InitOnePlayer(byte tbIndex, string name, byte teamId)
    {
        this.tbIndex = tbIndex;
        this._name = name;
        this.titleGo.SetActive(true);
        this.SetName(name);
        this.SetTeamId(teamId);
    }

    public void SetName(string name)
    {
        this.nameTxt.text = name;
    }

    public void SetTeamId(byte teamId)
    {
        this.teamId = teamId;
        Color useColor;
        if (teamId == 0)
        {
            useColor = GameColor.Green;
        }
        else if (teamId == 1)
        {
            useColor = GameColor.Red;
        }
        else if (teamId == 2)
        {
            useColor = GameColor.Blue;
        }
        else
        {
            useColor = GameColor.Yellow;
        }
        teamImg.color = useColor;
    }

    public virtual void GameReady()
    {
        state = PlayerState.Ready;
        readyTxt.text = "Ready";
    }

    public virtual void GameStart(AllShapeInfo allShape)
    {
        this.readyTxt.enabled = false;
        state = PlayerState.Gaming;
    }

    public virtual void Failure()
    {
        state = PlayerState.Failure;
        readyTxt.text = "Lose";
        //处理地图
        this.map.GrayMap();
    }

    public virtual void Win()
    {
        readyTxt.text = "Win";

    }

    public virtual void GameOver()
    {
        Debug.Log("GameOver");
        state = PlayerState.GameOver;
    }

    public virtual void PlayerReadyOrCancel(bool isReady)
    {
        this.map.ClearMap();
        this.readyTxt.text = "Ready";
        this.readyTxt.enabled = isReady;
        this.state = isReady ? PlayerState.Ready : PlayerState.None;
    }

    public abstract void SPutShape(SPutShape sps);

    public void ApplyItemFlagOnMap(byte itemFlag)
    {
        this.map.ApplyItemFlagOnMap(itemFlag);
    }

    public void UnderAttack(byte addLine)
    {
        this.map.ChangeLine(addLine);
    }

    public virtual void ReceiveItem(byte itemType)
    {
        this.map.ReceiveItem(itemType);
    }

    public void ClearPlayer()
    {
        this._name = null;
        this.titleGo.SetActive(false);
        this.readyTxt.enabled = false;
        this.tbIndex = byte.MaxValue;
        this.map.ClearMap();
    }

}
