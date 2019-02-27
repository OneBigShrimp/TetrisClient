using UnityEngine;


public class InputControl
{
    public float FirstRepeatDelay = 0.2f;
    public float RepeatInterval = 0.1f;

    CommandType lastCmdType;
    float lastCmdTime;
    bool isFirstRepeat = false;

    public Command GetCommand()
    {
        CommandType curType;
        byte cmdArgs = 0;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            curType = CommandType.Turn;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            curType = CommandType.MoveLeft;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            curType = CommandType.MoveRight;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            curType = CommandType.MoveDown;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            curType = CommandType.DownDirectly;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            curType = CommandType.SwitchItem;
        }
        else if (Input.GetKey(KeyCode.Alpha1))
        {
            curType = CommandType.UseItem;
            cmdArgs = 1;
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            curType = CommandType.UseItem;
            cmdArgs = 2;
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            curType = CommandType.UseItem;
            cmdArgs = 3;
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            curType = CommandType.UseItem;
            cmdArgs = 4;
        }
        else if (Input.GetKey(KeyCode.Alpha5))
        {
            curType = CommandType.UseItem;
            cmdArgs = 5;
        }
        else if (Input.GetKey(KeyCode.Alpha6))
        {
            curType = CommandType.UseItem;
            cmdArgs = 6;
        }
        else
        {
            lastCmdType = CommandType.None;
            return null;
        }
        if (curType == lastCmdType)
        {
            float inter = Time.time - lastCmdTime;
            if (isFirstRepeat)
            {
                if (inter < FirstRepeatDelay)
                {
                    return null;
                }
                else
                {
                    isFirstRepeat = false;
                }
            }
            else if (inter < RepeatInterval)
            {
                return null;
            }
        }
        else
        {
            isFirstRepeat = true;
            lastCmdType = curType;
        }
        lastCmdTime = Time.time;
        return new Command(curType, cmdArgs);

    }
}

public class Command
{
    public CommandType CmdType = CommandType.None;
    public byte CmdArgs = 0;

    public Command(CommandType cmdType, byte cmdArgs)
    {
        this.CmdType = cmdType;
        this.CmdArgs = cmdArgs;
    }
}

public enum CommandType
{
    None = 0,
    Turn = 1,
    MoveLeft = 2,
    MoveRight = 3,
    MoveDown = 4,
    DownDirectly = 5,
    SwitchItem = 6,
    UseItem = 7,
}