using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public const string WebHost = "https://58.141.79.181:5001/api";

    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum UIEvent
    {
        LClick,
        RClick,
        Drag,
        EndDrag,
        LDbClick,
        RDbClick,
    }
}
