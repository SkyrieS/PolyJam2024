using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockReceiverSkill : BaseReceiveSkill
{
    public string mockMessage;

    public override void Perform()
    {
        Debug.Log(mockMessage);
    }
}
