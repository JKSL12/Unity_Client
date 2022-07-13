using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerController : CreatureController
{
    protected Coroutine _coSkill;
    protected bool _rangedSkill = false;

    protected override void Init()
    {
        base.Init();
    }

    protected override void UpdateAnimation()
    {
        if (_animator == null || _sprite == null)
            return;

        if (State == CreatureState.Idle)
        {
            switch (Dir)
            {

                case MoveDir.Up:
                    {
                        _animator.Play("IDLE_BACK");
                        _sprite.flipX = false;
                    }
                    break;
                case MoveDir.Down:
                    {
                        _animator.Play("IDLE_FRONT");
                        _sprite.flipX = false;
                    }
                    break;
                case MoveDir.Left:
                    {
                        _animator.Play("IDLE_RIGHT");
                        _sprite.flipX = true;
                    }
                    break;
                case MoveDir.Right:
                    {
                        _animator.Play("IDLE_RIGHT");
                        _sprite.flipX = false;
                    }
                    break;
            }
        }
        else if (State == CreatureState.Moving)
        {
            switch (Dir)
            {
                case MoveDir.Up:
                    _sprite.flipX = false;
                    _animator.Play("WALK_BACK");
                    break;
                case MoveDir.Down:
                    _sprite.flipX = false;
                    _animator.Play("WALK_FRONT");
                    break;
                case MoveDir.Left:
                    _sprite.flipX = true;
                    _animator.Play("WALK_RIGHT");
                    break;
                case MoveDir.Right:
                    _sprite.flipX = false;
                    _animator.Play("WALK_RIGHT");
                    break;
            }
        }
        else if (State == CreatureState.Skill)
        {
            switch (Dir)
            {
                case MoveDir.Up:
                    _sprite.flipX = false;                    
                    _animator.Play(_rangedSkill ? "ATTACK_WEAPON_BACK" : "ATTACK_BACK");
                    break;
                case MoveDir.Down:
                    _sprite.flipX = false;
                    _animator.Play(_rangedSkill ? "ATTACK_WEAPON_FRONT" : "ATTACK_FRONT");
                    break;
                case MoveDir.Left:
                    _sprite.flipX = true;
                    _animator.Play(_rangedSkill ? "ATTACK_WEAPON_RIGHT" : "ATTACK_RIGHT");
                    break;
                case MoveDir.Right:
                    _sprite.flipX = false;
                    _animator.Play(_rangedSkill ? "ATTACK_WEAPON_RIGHT" : "ATTACK_RIGHT");
                    break;
            }
        }
        else
        {

        }
    }

    protected override void UpdateController()
    {      
        base.UpdateController();
    }

    public override void useSkill(int skillId)
    {
        if( skillId == 1 )
        {
            _coSkill = StartCoroutine("CoStartPunch");
        }
        else if( skillId == 2)
        {
            _coSkill = StartCoroutine("CoStartShootArrow");
        }
    }

    protected virtual void CheckUpdatedFlag()
    {

    }

    IEnumerator CoStartPunch()
    {
        //GameObject go = Managers.Object.Find(GetFrontCellPos());
        //if( go != null )
        //{
        //    CreatureController cc = go.GetComponent<CreatureController>();
        //    if (cc != null)
        //        cc.OnDamaged();
        //}

        _rangedSkill = false;
        State = CreatureState.Skill;
        yield return new WaitForSeconds(0.5f);
        State = CreatureState.Idle;
        _coSkill = null;
        CheckUpdatedFlag();
    }

    IEnumerator CoStartShootArrow()
    {
        //GameObject go = Managers.Resource.Instantiate("Creature/Arrow");
        //ArrowController ac = go.GetComponent<ArrowController>();
        //ac.Dir = Dir;
        //ac.CellPos = CellPos;

        _rangedSkill = true;
        State = CreatureState.Skill;
        yield return new WaitForSeconds(0.2f);
        State = CreatureState.Idle;
        _coSkill = null;
        CheckUpdatedFlag();
    }

    public override void OnDamaged()
    {
        Debug.Log("Player hit");
    }

    public virtual void LevelUp()
    {
        GameObject effect = Managers.Resource.Instantiate("Effect/LevelUpEffect");
        effect.transform.position = transform.position;
        effect.GetComponent<Animator>().Play("START");
        GameObject.Destroy(effect, 0.5f);
    }
}
