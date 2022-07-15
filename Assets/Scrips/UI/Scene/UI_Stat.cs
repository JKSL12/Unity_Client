using Data;
using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Stat : UI_Base
{
    [SerializeField]
    Button _strP;

    [SerializeField]
    Image _strM;

    [SerializeField]
    Image _dexP;

    [SerializeField]
    Image _dexM;

    [SerializeField]
    Image _magP;

    [SerializeField]
    Image _magM;

    [SerializeField]
    Image _vitP;

    [SerializeField]
    Image _vitM;

    enum Buttons
    {
        StrP,
        StrM,
        DexP,
        DexM,
        MagP,
        MagM,
        VitP,
        VitM,
    }

    enum Texts
    {
        NameText,
        Level,
        Exp,
        AttackValueText,
        DefenceValueText,
        StrText,
        DexText,
        MagText,
        VitText,
        BonusStatText,
    }

    bool _init = false;
    public override void Init()
    {
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        _init = true;
        RefreshUI();

        MyPlayerController player = Managers.Object.MyPlayer;

        _strP.gameObject.BindEvent((e) =>
        {
            C_StatPlusminus statPacket = new C_StatPlusminus();
            statPacket.Plus = true;
            statPacket.StatType = 1;
            statPacket.StatNum = 1;
            Managers.Network.Send(statPacket);
        });

        _strM.gameObject.BindEvent((e) =>
        {
            C_StatPlusminus statPacket = new C_StatPlusminus();
            statPacket.Plus = false;
            statPacket.StatType = 1;
            statPacket.StatNum = 1;
            Managers.Network.Send(statPacket);
        });

        _dexP.gameObject.BindEvent((e) =>
        {
            C_StatPlusminus statPacket = new C_StatPlusminus();
            statPacket.Plus = true;
            statPacket.StatType = 2;
            statPacket.StatNum = 1;
            Managers.Network.Send(statPacket);
        });

        _dexM.gameObject.BindEvent((e) =>
        {
            C_StatPlusminus statPacket = new C_StatPlusminus();
            statPacket.Plus = false;
            statPacket.StatType = 2;
            statPacket.StatNum = 1;
            Managers.Network.Send(statPacket);
        });

        _magP.gameObject.BindEvent((e) =>
        {
            C_StatPlusminus statPacket = new C_StatPlusminus();
            statPacket.Plus = true;
            statPacket.StatType = 3;
            statPacket.StatNum = 1;
            Managers.Network.Send(statPacket);
        });

        _magM.gameObject.BindEvent((e) =>
        {
            C_StatPlusminus statPacket = new C_StatPlusminus();
            statPacket.Plus = false;
            statPacket.StatType = 3;
            statPacket.StatNum = 1;
            Managers.Network.Send(statPacket);
        });

        _vitP.gameObject.BindEvent((e) =>
        {
            C_StatPlusminus statPacket = new C_StatPlusminus();
            statPacket.Plus = true;
            statPacket.StatType = 4;
            statPacket.StatNum = 1;
            Managers.Network.Send(statPacket);
        });

        _vitM.gameObject.BindEvent((e) =>
        {
            C_StatPlusminus statPacket = new C_StatPlusminus();
            statPacket.Plus = false;
            statPacket.StatType = 4;
            statPacket.StatNum = 1;
            Managers.Network.Send(statPacket);
        });
    }


    public void RefreshUI()
    {
        if (_init == false)
            return;

        MyPlayerController player = Managers.Object.MyPlayer;
        player.RefreshAdditionalStat();

        Get<Text>((int)Texts.NameText).text = player.name;

        int nextexp = player.GetRequiredExpNextLevel(player.Stat.Level);
        Get<Text>((int)Texts.Level).text = $"Level : {player.Stat.Level}";
        if (nextexp == -1)
        {
            Get<Text>((int)Texts.Exp).text = $"Exp : {player.Stat.TotalExp}";
        }
        else
        {
            Get<Text>((int)Texts.Exp).text = $"Exp : {player.Stat.TotalExp} / {nextexp}\r\n( { (float)player.Stat.TotalExp * 100 / nextexp } %)";
        }
            

        int totalDamage = player.Stat.Attack + player.WeaponDamage;                
        Get<Text>((int)Texts.AttackValueText).text = $"{totalDamage}(+{player.WeaponDamage})";
        Get<Text>((int)Texts.DefenceValueText).text = $"{player.ArmorDefence}";
        Get<Text>((int)Texts.StrText).text = $"Str : {player.Stat.Str}";
        Get<Text>((int)Texts.DexText).text = $"Dex : {player.Stat.Dex}";
        Get<Text>((int)Texts.MagText).text = $"Mag : {player.Stat.Mag}";
        Get<Text>((int)Texts.VitText).text = $"Vit : {player.Stat.Vit}";
        Get<Text>((int)Texts.BonusStatText).text = $"BonusStat : {player.Stat.BonusStat}";
    }
}
