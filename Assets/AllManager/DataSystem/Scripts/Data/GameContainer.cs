using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Datamanager;
public class GameContainer : Architecture<GameContainer>
{
    protected override void Init()
    {
        Register(new DataManager());
        Get<DataManager>().InitDataMananger();
    }
}
