using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CreatureController : BaseController
{
    HpBar _hpBar;
 
    public override StatInfo Stat
    {
        get { return base.Stat; }
        set
        {
            base.Stat = value;
            UpdateHpBar();
        }
    }

    public override int Hp
    {
        get { return Stat.Hp; }
        set
        {
            base.Hp = value;
            UpdateHpBar();
        }
    }

    protected void AddHpBar()
    {
        GameObject go = Managers.Resource.Instantiate("UI/SubItem/HpBar", transform);
        go.transform.localPosition = new Vector3(0, 0.5f, 0);
        go.name = "HpBar";
        _hpBar = go.GetComponent<HpBar>();
        UpdateHpBar();
    }

    protected void HideHpBar()
    {
        Debug.Log("HpBar Check");

        if (_hpBar == null) return;

        Debug.Log("HpBar DeActive");
        _hpBar.gameObject.SetActive(false);
    }

    void UpdateHpBar()
    {
        if (_hpBar == null)
            return;

        float ratio = 0.0f;
        if(Stat.MaxHp > 0 )
        {
            ratio = ((float)Hp / Stat.MaxHp);            
        }
        
        _hpBar.SetHpBar(ratio);
    }

    public void ShowDamage(int damage, bool critical)
    {
        UI_GameScene gameSceneUI = Managers.UI.SceneUI as UI_GameScene;
        GameObject go = Managers.Resource.Instantiate("UI/SubItem/Damage", transform);
        Vector3 uiPosition = Camera.main.WorldToScreenPoint(transform.position);
        //go.transform.localPosition = uiPosition;
        go.transform.position = new Vector3( uiPosition.x, uiPosition.y + 0.6f, uiPosition.z);
        //go.transform.position = transform.position;
        go.name = "Damage";
        //go.transform.position = new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z);

        go.transform.SetParent(gameSceneUI.transform);
        

        go.GetComponent<Damage>().SetDamage(damage, critical);
    }

    protected override void Init()
    {
        base.Init();
        
        AddHpBar();

        UpdateAnimation();
    }

    public virtual void OnDamaged()
    {

    }

    public virtual void OnDead()
    {
        State = CreatureState.Dead;

        GameObject effect = Managers.Resource.Instantiate("Effect/DieEffect");
        effect.transform.position = transform.position;
        effect.GetComponent<Animator>().Play("START");
        GameObject.Destroy(effect, 0.5f);
    }

    public virtual void useSkill(int skillId)
    {

    }
}
