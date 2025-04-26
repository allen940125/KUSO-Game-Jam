using Datamanager;
using System;
using UniRx;
using UnityEngine;

[System.Serializable]
public class MainGameMediator
{
    CompositeDisposable disposable_ = new CompositeDisposable();

    [field: SerializeField] public RealTimePlayerData RealTimePlayerData { get; private set; } = new RealTimePlayerData();

    public void MainGameMediatorInit()
    {
        RealTimePlayerData = GameContainer.Get<DataManager>().realTimePlayerData;
    }

    public void DisposeObserber()
    {
        disposable_.Dispose();
    }
    public void AddToDisposables(IDisposable disposable)
    {
        disposable_.Add(disposable);
    }
}
