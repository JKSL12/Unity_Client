using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager
{
    public MyPlayerController MyPlayer { get; set; }
    Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>();
    //List<GameObject> _objects = new List<GameObject>();

    public static GameObjectType GetObjectTypeById(int id)
    {
        int type = (id >> 24) & 0x7F;
        return (GameObjectType)type;
    }

    public void Add(ObjectInfo info, bool myPlayer = false)
    {
        if (MyPlayer != null && MyPlayer.Id == info.ObjectId)
            return;
        if (_objects.ContainsKey(info.ObjectId))
            return;

        GameObjectType objectType = GetObjectTypeById(info.ObjectId);
        if( objectType == GameObjectType.Player)
        {
            if (myPlayer)
            {
                GameObject go = Managers.Resource.Instantiate("Creature/MyPlayer");
                go.name = info.Name;
                _objects.Add(info.ObjectId, go);

                MyPlayer = go.GetComponent<MyPlayerController>();
                MyPlayer.Id = info.ObjectId;
                //MyPlayer.CellPos = new Vector3Int(info.PosX, info.PosY, 0);
                MyPlayer.PosInfo = info.PosInfo;
                MyPlayer.Stat.MergeFrom(info.StatInfo);
                MyPlayer.SyncPos();
            }
            else
            {

                GameObject go = Managers.Resource.Instantiate("Creature/Player");
                go.name = info.Name;
                _objects.Add(info.ObjectId, go);

                PlayerController pc = go.GetComponent<PlayerController>();
                pc.Id = info.ObjectId;
                //pc.CellPos = new Vector3Int(info.PosX, info.PosY, 0);
                pc.PosInfo = info.PosInfo;
                pc.Stat.MergeFrom(info.StatInfo);
                pc.SyncPos();
            }
        }
        else if(objectType == GameObjectType.Monster )
        {
            GameObject go = Managers.Resource.Instantiate("Creature/Monster");
            go.name = info.Name;
            _objects.Add(info.ObjectId, go);

            MonsterController mc = go.GetComponent<MonsterController>();
            mc.Id = info.ObjectId;
            mc.PosInfo = info.PosInfo;
            mc.Stat = info.StatInfo;
            mc.SyncPos();
        }
        else if(objectType == GameObjectType.Projectile)
        {
            GameObject go = Managers.Resource.Instantiate("Creature/Arrow");
            go.name = "Arrow";
            _objects.Add(info.ObjectId, go);

            ArrowController ac = go.GetComponent<ArrowController>();
            ac.PosInfo = info.PosInfo;
            ac.Stat.MergeFrom(info.StatInfo);
            //ac.Dir = info.PosInfo.MoveDir;
            //ac.CellPos = new Vector3Int(info.PosInfo.PosX, info.PosInfo.PosY, 0);
            ac.SyncPos();
        }
       
    }

    //public void Add(int id, GameObject go )
    //{
    //    _objects.Add(id, go);
    //}

    public void Remove(int id)
    {
        if (MyPlayer != null && MyPlayer.Id == id)
            return;
        if (_objects.ContainsKey(id) == false)
            return;

        GameObject go = FindById(id);
        if (go == null) return;

        _objects.Remove(id);
        Managers.Resource.Destroy(go);
    }

    public void RemoveAll()
    {
        if (MyPlayer == null)
            return;

        Clear();
        MyPlayer = null;
    }

    public GameObject FindCreature(Vector3Int cellPos)
    {
        foreach( GameObject obj in _objects.Values)
        {
            CreatureController cc = obj.GetComponent<CreatureController>();
            if (cc == null)
                continue;

            if (cc.CellPos == cellPos)
                return obj;
        }

        return null;
    }

    public GameObject FindById(int id)
    {
        GameObject go = null;
        _objects.TryGetValue(id, out go);
        return go;
    }

    public GameObject Find(Func<GameObject, bool> condition)
    {
        foreach (GameObject obj in _objects.Values)
        {
            CreatureController cc = obj.GetComponent<CreatureController>();
            if (cc == null)
                continue;

            if (condition.Invoke(obj))
                return obj;
        }

        return null;
    }

    public void Clear()
    {
        foreach (GameObject obj in _objects.Values)
        {
            Managers.Resource.Destroy(obj);
        }

        _objects.Clear();
        MyPlayer = null;
    }
}
