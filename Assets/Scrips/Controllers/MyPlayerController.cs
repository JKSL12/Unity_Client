using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MyPlayerController : PlayerController
{
    bool _moveKeyPressed = false;
    bool _inputChatPressed = false;

    public int WeaponDamage { get; private set; }
    public int ArmorDefence { get; private set; }

    protected override void Init()
    {
        base.Init();
    }

    protected override void UpdateController()
    {
        GetUIKeyInput();
         
        switch (State)
        {
            case CreatureState.Idle:
                UpdateIdle();
                GetDirInput();
                break;
            case CreatureState.Moving:
                GetDirInput();
                break;
        }

        base.UpdateController();
    }

    protected override void UpdateIdle()
    {
        if (_moveKeyPressed)
        {
            State = CreatureState.Moving;
            return;
        }

        if (_coSkillCooltime == null && Input.GetKey(KeyCode.Space))
        {
            

            C_Skill skill = new C_Skill() { Info = new SkillInfo() };
            skill.Info.SkillId = 2;
            Managers.Network.Send(skill);

            _coSkillCooltime = StartCoroutine("CoInputCooltime", 0.2f);
            //State = CreatureState.Skill;
            ////_coSkill = StartCoroutine("CoStartPunch");
            //_coSkill = StartCoroutine("CoStartShootArrow");
        }
    }

    Coroutine _coSkillCooltime;
    IEnumerator CoInputCooltime(float time)
    {
        yield return new WaitForSeconds(time);
        _coSkillCooltime = null;
    }

    private void LateUpdate()
    {
        {
            Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        }
    }

    void GetUIKeyInput()
    {    
        if (Input.GetKeyUp(KeyCode.Return))
        {
            UI_GameScene gameSceneUI = Managers.UI.SceneUI as UI_GameScene;
            UI_Chat chatUI = gameSceneUI.chatUI;

            _inputChatPressed = chatUI.SendMsgCheck();
        }

        if (_inputChatPressed == true) return;

        if (Input.GetKeyUp(KeyCode.I))
        {
            UI_GameScene gameSceneUI = Managers.UI.SceneUI as UI_GameScene;
            UI_Inventory invenUI = gameSceneUI.InvenUI;

            if( invenUI.gameObject.activeSelf)
            {
                invenUI.gameObject.SetActive(false);
            }
            else
            {
                invenUI.gameObject.SetActive(true);
                invenUI.RefreshUI();
            }
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            UI_GameScene gameSceneUI = Managers.UI.SceneUI as UI_GameScene;
            UI_Equip equipUI = gameSceneUI.equipUI;

            if (equipUI.gameObject.activeSelf)
            {
                equipUI.gameObject.SetActive(false);
            }
            else
            {
                equipUI.gameObject.SetActive(true);
                equipUI.RefreshUI();
            }
        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            UI_GameScene gameSceneUI = Managers.UI.SceneUI as UI_GameScene;
            UI_Stat statUI = gameSceneUI.statUI;

            if (statUI.gameObject.activeSelf)
            {
                statUI.gameObject.SetActive(false);
            }
            else
            {
                statUI.gameObject.SetActive(true);
                statUI.RefreshUI();
            }
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            C_PlayerKill killPacket = new C_PlayerKill();
            Managers.Network.Send(killPacket);
        }
    }

    void GetDirInput()
    {
        if (_inputChatPressed == true) return;

        _moveKeyPressed = true;

        if (Input.GetKey(KeyCode.W))
        {
            //transform.position += Vector3.up * Time.deltaTime * _speed;
            Dir = MoveDir.Up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            //transform.position += Vector3.down * Time.deltaTime * _speed;
            Dir = MoveDir.Down;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            //transform.position += Vector3.left * Time.deltaTime * _speed;
            Dir = MoveDir.Left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //transform.position += Vector3.right * Time.deltaTime * _speed;
            Dir = MoveDir.Right;
        }
        else
        {
            _moveKeyPressed = false;
        }
    }

    protected override void MoveToNextPos()
    {
        if (_moveKeyPressed == false)
        {
            State = CreatureState.Idle;
            CheckUpdatedFlag();
            return;
        }

        Vector3Int destPos = CellPos;
        switch (Dir)
        {
            case MoveDir.Up:
                destPos += Vector3Int.up;
                break;
            case MoveDir.Down:
                destPos += Vector3Int.down;
                break;
            case MoveDir.Left:
                destPos += Vector3Int.left;
                break;
            case MoveDir.Right:
                destPos += Vector3Int.right;
                break;
        }

        //State = CreatureState.Moving;

        if (Managers.Map.CanGo(destPos))
        {
            if (Managers.Object.FindCreature(destPos) == null)
            {
                CellPos = destPos;
            }
        }


        //base.MoveToNextPos();        


        //CreatureState prevState = State;
        //Vector3Int prevCellPos = CellPos;

        CheckUpdatedFlag();
    }
    
    protected override void CheckUpdatedFlag()
    {
        if(_updated)
        {
            C_Move movePacket = new C_Move();
            movePacket.PosInfo = PosInfo;
            Managers.Network.Send(movePacket);
            _updated = false;
        }
    }

    public void RefreshAdditionalStat()
    {
        WeaponDamage = 0;
        ArmorDefence = 0;

        foreach (Item item in Managers.Inven.Items.Values)
        {
            if (item.Equipped == false)
                continue;

            switch (item.ItemType)
            {
                case ItemType.Weapon:
                    WeaponDamage += ((Weapon)item).Damage;
                    break;
                case ItemType.Armor:
                    ArmorDefence += ((Armor)item).Defence;
                    break;
            }
        }
    }

    public int GetRequiredExpNextLevel(int level)
    {
        StatInfo statData = null;
        Managers.Data.StatDict.TryGetValue(level + 1, out statData);

        if (statData == null) return -1;

        return statData.TotalExp;
    }
}
