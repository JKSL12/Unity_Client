using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class NpcController : CreatureController
{
    Coroutine _coSkill;    

    protected override void Init()
    {
        base.Init();

        State = CreatureState.Idle;

        HideHpBar();
        //Dir = MoveDir.Down;  
    }

    protected override void UpdateIdle()
    {
        base.UpdateIdle();
    }
        
    public override void OnDamaged()
    {      
      //  Managers.Object.Remove(Id);
      //  Managers.Resource.Destroy(gameObject);
    }

    public override void useSkill(int skillId)
    {
        if (skillId == 1)
        {
            State = CreatureState.Skill;
        }
    }
}
