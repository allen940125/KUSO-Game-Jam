using System.Linq;
using UniRx;

namespace Gamemanager
{
    using System.Collections.Generic;
    using System;
    using UnityEngine;

    interface IEventPublisher
    {
        void Send(GameEventMessageBase msg);
    }

    public abstract class GameEventPack : IEventPublisher
    {
        Dictionary<Type, IEventPublisher> eventPublishers_ = new();
        // 修改為儲存 (IDisposable, Action) 的列表
        Dictionary<Type, List<(IDisposable disposable, Delegate action)>> subscriptions_ = new();

        public void Send(GameEventMessageBase msg)
        {
            foreach (var publisher in eventPublishers_.Values)
            {
                publisher.Send(msg);
            }
        }

        protected IObservable<T> getSubject<T>()
        {
            if (!eventPublishers_.TryGetValue(typeof(T), out var publisher))
            {
                var subject = new GameMessageSubject<T>();
                eventPublishers_.Add(typeof(T), subject);
                publisher = subject;
            }

            if (publisher is IGameMessageObservable<T> observable)
            {
                return observable.Observable;
            }

            throw new UnityException($"Get Subject Error, Msg Type: {typeof(T)}");
        }

        public IDisposable SetSubscribe<T>(IObservable<T> target, Action<T> action)
        {
            var disposable = target.Subscribe(action);
            
            if (!subscriptions_.TryGetValue(typeof(T), out var list))
            {
                list = new List<(IDisposable, Delegate)>();
                subscriptions_.Add(typeof(T), list);
            }
            
            list.Add((disposable, action));
            GameManager.Instance.MainGameMediator.AddToDisposables(disposable);
            return disposable;
        }

        /// <summary>
        /// 取消整個事件類型的所有訂閱
        /// </summary>
        public void Unsubscribe<T>()
        {
            if (subscriptions_.TryGetValue(typeof(T), out var list))
            {
                foreach (var item in list)
                {
                    item.disposable.Dispose();
                }
                list.Clear();
                subscriptions_.Remove(typeof(T));
            }
        }

        /// <summary>
        /// 取消特定 Action 的訂閱
        /// </summary>
        public void Unsubscribe<T>(Action<T> action)
        {
            if (subscriptions_.TryGetValue(typeof(T), out var list))
            {
                var toRemove = list.Where(x => x.action.Equals(action)).ToList();
                foreach (var item in toRemove)
                {
                    item.disposable.Dispose();
                    list.Remove(item);
                }

                if (list.Count == 0)
                {
                    subscriptions_.Remove(typeof(T));
                }
            }
        }
    }

    interface IGameMessageObservable<T>
    {
        public IObservable<T> Observable { get; }
    }

    class GameMessageSubject<T> : IEventPublisher, IGameMessageObservable<T>
    {
        Subject<T> subject_ = new();
        Dictionary<object, IDisposable> subscriptions_ = new();

        public IObservable<T> Observable => subject_;

        public void Send(GameEventMessageBase msg)
        {
            if (msg is T focusMessage)
            {
                subject_.OnNext(focusMessage);
            }
        }

        public void Subscribe(object key, Action<T> action)
        {
            if (subscriptions_.ContainsKey(key))
            {
                subscriptions_[key].Dispose();
            }
            subscriptions_[key] = subject_.Subscribe(action);
        }

        public void Unsubscribe(object key)
        {
            if (subscriptions_.TryGetValue(key, out var disposable))
            {
                disposable.Dispose();
                subscriptions_.Remove(key);
            }
        }
    }
}