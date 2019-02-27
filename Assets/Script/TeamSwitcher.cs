using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamSwitcher : MonoBehaviour
{

    public event Action<byte> OnClickTeam;

    public void OnClick(int index)
    {
        if (OnClickTeam != null)
        {
            OnClickTeam((byte)index);
        }
    }

}

