using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Chat : UI_Base
{
    [SerializeField]
    Button AllChatButton;

    [SerializeField]
    Button AreaChatButton;

    [SerializeField]
    Button MaxMinButton;

    enum Texts
    {
        ChatText
    }

    enum GameObjects
    {
        ChatScrollView,
        InputChat,
    }

    int chatType = 0;
    bool bMini = false;

    public override void Init()
    {
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        SetChatType(0);

        AllChatButton.gameObject.BindEvent((e) =>
        {
            SetChatType(0);
        });

        AreaChatButton.gameObject.BindEvent((e) =>
        {
            SetChatType(1);
        });

        MaxMinButton.gameObject.BindEvent((e) =>
        {
            Text text = MaxMinButton.gameObject.GetComponentInChildren<Text>();
            RectTransform rt = GetComponent<RectTransform>();
            if (rt == null) return;

            bMini = !bMini;
            if ( text != null )
            {
                if (bMini)
                {
                    text.text = "+";
                    rt.position = new Vector3(rt.position.x, 30, 0);
                    rt.sizeDelta = new Vector2Int(400, 60);                    
                }
                else
                {
                    text.text = "-";
                    rt.position = new Vector3(rt.position.x, 110, 0);
                    rt.sizeDelta = new Vector2Int(400, 220);
                }
            }
        });
    }

    void SetChatType(int type)
    {        
        GameObject frame1 = AllChatButton.transform.Find("Frame").gameObject;
        GameObject frame2 = AreaChatButton.transform.Find("Frame").gameObject;
        if (frame1 == null || frame2 == null)
        {
            return;
        }

        chatType = type;

        switch ( type )
        {
            case 0:
                frame1.gameObject.SetActive(true);
                frame2.gameObject.SetActive(false);
                break;
            case 1:
                frame1.gameObject.SetActive(false);
                frame2.gameObject.SetActive(true);
                break;
        }
    }

    void SendChatMsg()
    {

    }

    public void SetChatText(string msg)
    {
        Get<Text>((int)Texts.ChatText).text += msg + "\n";
        Get<GameObject>((int)GameObjects.ChatScrollView).GetComponent<ScrollRect>().verticalNormalizedPosition = 0.0f;

        Debug.Log(msg);
    }

    public bool SendMsgCheck()
    {
        InputField input = Get<GameObject>((int)GameObjects.InputChat).GetComponent<InputField>();

        if (input == null) return false;

        if ( input.text != "" )
        {
            string msg = input.text;
            input.text = "";

            C_Chat chatPacket = new C_Chat();
            chatPacket.ChatType = chatType;
            chatPacket.ChatMsg = msg;

            Managers.Network.Send(chatPacket);

            input.Select();

            Debug.Log($"{msg}, { input.text} ");
        }
        else
        {
            //input.ActivateInputField();
            input.Select();
            return true;
        }

        return false;
    }
}
