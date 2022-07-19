using Data;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PacketHandler
{
    public static void S_EnterGameHandler(PacketSession session, IMessage packet)
    {
        S_EnterGame enterGamePacket = packet as S_EnterGame;
        ServerSession serverSession = session as ServerSession;

        MapData mapData = null;

        Managers.Data.MapDict.TryGetValue(enterGamePacket.Player.PosInfo.MapId, out mapData);

        if (mapData == null) return;

        Managers.Map.DestroyMap();
        Managers.Map.LoadMap(mapData.name);
        Managers.Object.Add(enterGamePacket.Player, myPlayer: true);

        //Debug.Log("S_EnterGameHandler");
        //Debug.Log(enterGamePacket.Player);
    }

    public static void S_LeaveGameHandler(PacketSession session, IMessage packet)
    {
        S_LeaveGame leaveGamePacket = packet as S_LeaveGame;
        ServerSession serverSession = session as ServerSession;

        //Debug.Log("S_LeaveGameHandler");
        Managers.Object.Clear();
    }

    public static void S_SpawnHandler(PacketSession session, IMessage packet)
    {
        S_Spawn spawnPacket = packet as S_Spawn;

        foreach (ObjectInfo obj in spawnPacket.Objects)
        {
            Managers.Object.Add(obj, myPlayer: false);
        }
        //Debug.Log("S_SpawnHandler");
        //Debug.Log(spawnPacket.Players);
    }

    public static void S_DespawnHandler(PacketSession session, IMessage packet)
    {
        S_Despawn despawnPacket = packet as S_Despawn;

        foreach (int id in despawnPacket.ObjectIds)
        {
            Managers.Object.Remove(id);
        }
        //Debug.Log("S_DespawnHandler");
    }

    public static void S_MoveHandler(PacketSession session, IMessage packet)
    {
        S_Move movePacket = packet as S_Move;

        //Debug.Log("S_MoveHandler");
        GameObject go = Managers.Object.FindById(movePacket.ObjectId);

        if (go == null) return;

        if (Managers.Object.MyPlayer.Id == movePacket.ObjectId)
            return;

        BaseController bc = go.GetComponent<BaseController>();
        if (bc == null) return;

        bc.PosInfo = movePacket.PosInfo;
    }

    public static void S_SkillHandler(PacketSession session, IMessage packet)
    {
        S_Skill skillPacket = packet as S_Skill;

        GameObject go = Managers.Object.FindById(skillPacket.ObjectId);
        if (go == null)
            return;

        //Debug.Log($"skill {skillPacket.Info.SkillId}");

        CreatureController cc = go.GetComponent<CreatureController>();
        if (cc != null)
        {
            cc.useSkill(skillPacket.Info.SkillId);
        }
    }

    public static void S_ChangeHpHandler(PacketSession session, IMessage packet)
    {
        S_ChangeHp changePacket = packet as S_ChangeHp;

        GameObject go = Managers.Object.FindById(changePacket.ObjectId);

        if (go == null) return;

        CreatureController cc = go.GetComponent<CreatureController>();

        if (cc != null)
        {
            cc.Hp = changePacket.Hp;
        }
    }

    public static void S_DieHandler(PacketSession session, IMessage packet)
    {
        S_Die diePacket = packet as S_Die;

        GameObject go = Managers.Object.FindById(diePacket.ObjectId);

        if (go == null) return;

        CreatureController cc = go.GetComponent<CreatureController>();

        if (cc != null)
        {
            cc.Hp = 0;
            cc.OnDead();
        }
    }

    public static void S_ConnectedHandler(PacketSession session, IMessage packet)
    {
        Debug.Log("S_ConnectedHandler");

        C_Login loginPacket = new C_Login();

        string path = Application.dataPath;
        //loginPacket.UniqueId = SystemInfo.deviceUniqueIdentifier;
        loginPacket.UniqueId = path.GetHashCode().ToString();
        Managers.Network.Send(loginPacket);
    }

    public static void S_LoginHandler(PacketSession session, IMessage packet)
    {
        S_Login loginPacket = (S_Login)packet;

        Debug.Log($"LoginOk({loginPacket.LoginOk})");

        if (loginPacket.Players == null || loginPacket.Players.Count == 0)
        {
            C_CreatePlayer createPacket = new C_CreatePlayer();
            createPacket.Name = $"Player_{Random.Range(0, 10000).ToString("0000")}";
            Managers.Network.Send(createPacket);
        }
        else
        {
            LobbyPlayerInfo info = loginPacket.Players[0];
            C_EnterGame enterGamePacket = new C_EnterGame();
            enterGamePacket.Name = info.Name;
            Managers.Network.Send(enterGamePacket);
        }
    }

    public static void S_CreatePlayerHandler(PacketSession session, IMessage packet)
    {
        S_CreatePlayer createOkPacket = (S_CreatePlayer)packet;

        if (createOkPacket.Player == null)
        {
            C_CreatePlayer createPacket = new C_CreatePlayer();
            createPacket.Name = $"Player_{Random.Range(0, 10000).ToString("0000")}";
            Managers.Network.Send(createPacket);
        }
        else
        {
            C_EnterGame enterGamePacket = new C_EnterGame();
            enterGamePacket.Name = createOkPacket.Player.Name;
            Managers.Network.Send(enterGamePacket);
        }
    }

    public static void S_ItemListHandler(PacketSession session, IMessage packet)
    {
        S_ItemList itemList = (S_ItemList)packet;

        // UI_GameScene gameSceneUI = Managers.UI.SceneUI as UI_GameScene;

        // UI_Inventory invenUI = gameSceneUI.InvenUI;

        Managers.Inven.Clear();

        foreach (ItemInfo itemInfo in itemList.Items)
        {
            //Debug.Log($"{item.TemplateId} : {item.Count}");
            Item item = Item.MakeItem(itemInfo);
            Managers.Inven.Add(item);
        }

        if (Managers.Object.MyPlayer != null)
            Managers.Object.MyPlayer.RefreshAdditionalStat();

        // invenUI.gameObject.SetActive(true);
        // invenUI.RefreshUI();
    }

    public static void S_AddItemHandler(PacketSession session, IMessage packet)
    {
        S_AddItem itemList = (S_AddItem)packet;

        foreach (ItemInfo itemInfo in itemList.Items)
        {
            //Debug.Log($"{item.TemplateId} : {item.Count}");
            Item item = Item.MakeItem(itemInfo);
            Managers.Inven.Add(item);
        }

        UI_GameScene gameSceneUI = Managers.UI.SceneUI as UI_GameScene;
        UI_Inventory invenUI = gameSceneUI.InvenUI;
        invenUI.RefreshUI();
        gameSceneUI.equipUI.RefreshUI();

        if (Managers.Object.MyPlayer != null)
            Managers.Object.MyPlayer.RefreshAdditionalStat();
    }

    public static void S_EquipItemHandler(PacketSession session, IMessage packet)
    {
        S_EquipItem equipItemOk = (S_EquipItem)packet;

        Item item = Managers.Inven.Get(equipItemOk.ItemDbId);
        if (item == null)
            return;

        item.Equipped = equipItemOk.Equipped;
        Debug.Log("아이템 착용 변경");

        if (Managers.Object.MyPlayer != null)
            Managers.Object.MyPlayer.RefreshAdditionalStat();

        UI_GameScene gameSceneUI = Managers.UI.SceneUI as UI_GameScene;
        UI_Inventory invenUI = gameSceneUI.InvenUI;
        invenUI.RefreshUI();
        gameSceneUI.equipUI.RefreshUI();
        gameSceneUI.statUI.RefreshUI();
    }


    public static void S_ChangeStatHandler(PacketSession session, IMessage packet)
    {
        S_ChangeStat itemList = (S_ChangeStat)packet;
    }

    public static void S_PingHandler(PacketSession session, IMessage packet)
    {
        C_Pong pongPacket = new C_Pong();

        Managers.Network.Send(pongPacket);
    }

    public static void S_IncreaseExpHandler(PacketSession session, IMessage packet)
    {
        S_IncreaseExp increaseExpPacket = (S_IncreaseExp)packet;

        GameObject go = Managers.Object.FindById(increaseExpPacket.ObjectId);

        if (go == null) return;

        PlayerController pc = go.GetComponent<PlayerController>();

        if (pc != null)
        {
            pc.Stat.Level = increaseExpPacket.Level;
            pc.Stat.TotalExp = increaseExpPacket.TotalExp;


            if (increaseExpPacket.LevelUp)
            {
                pc.LevelUp();
                pc.Stat.BonusStat = increaseExpPacket.BonusStat;
            }

            UI_GameScene gameSceneUI = Managers.UI.SceneUI as UI_GameScene;
            gameSceneUI.statUI.RefreshUI();
        }
    }

    public static void S_UseItemHandler(PacketSession session, IMessage packet)
    {
        S_UseItem usePacket = (S_UseItem)packet;

        Managers.Inven.SetFindItemSlot(usePacket.ItemSlot, usePacket.ItemNum);

        UI_GameScene gameSceneUI = Managers.UI.SceneUI as UI_GameScene;
        UI_Inventory invenUI = gameSceneUI.InvenUI;
        invenUI.RefreshUI();
        //gameSceneUI.equipUI.RefreshUI();
    }

    public static void S_StatPlusminusHandler(PacketSession session, IMessage packet)
    {
        S_StatPlusminus statPacket = (S_StatPlusminus)packet;

        if (Managers.Object.MyPlayer != null)
        {
            switch (statPacket.StatType)
            {
                case 1:
                    {
                        Managers.Object.MyPlayer.Stat.Str = statPacket.StatNum;
                    }
                    break;
                case 2:
                    {
                        Managers.Object.MyPlayer.Stat.Dex = statPacket.StatNum;
                    }
                    break;
                case 3:
                    {
                        Managers.Object.MyPlayer.Stat.Mag = statPacket.StatNum;
                    }
                    break;
                case 4:
                    {
                        Managers.Object.MyPlayer.Stat.Vit = statPacket.StatNum;
                    }
                    break;
            }

            Managers.Object.MyPlayer.Stat.BonusStat = statPacket.BonusStat;

            Managers.Object.MyPlayer.RefreshAdditionalStat();
        }

        UI_GameScene gameSceneUI = Managers.UI.SceneUI as UI_GameScene;
        gameSceneUI.statUI.RefreshUI();
    }

    public static void S_ChatHandler(PacketSession session, IMessage packet)
    {
        S_Chat chatPacket = (S_Chat)packet;
        UI_GameScene gameSceneUI = Managers.UI.SceneUI as UI_GameScene;

        gameSceneUI.chatUI.SetChatText(chatPacket.ChatMsg);
    }

    public static void S_MoveItemHandler(PacketSession session, IMessage packet)
    {
        S_MoveItem moveItem = (S_MoveItem)packet;
        UI_GameScene gameSceneUI = Managers.UI.SceneUI as UI_GameScene;

        UI_Inventory invenUI = gameSceneUI.InvenUI;
        invenUI.ChangeSlotItem(moveItem.OriginSlot, moveItem.DestSlot);
        invenUI.RefreshUI();
    }
}