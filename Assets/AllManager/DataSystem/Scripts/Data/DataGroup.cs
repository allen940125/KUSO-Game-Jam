using Datamanager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Datamanager
{
    public class DataGroup
    {
        //public DataBase<GameEffectTemplete> GameEffectDataBase => TryGetDataBase<GameEffectTemplete>(); 
        //public DataBase<PlayerDataBaseTemplete> PlayerDataBase => TryGetDataBase<PlayerDataBaseTemplete>(); //最重要
        //public DataBase<MapDataStringTemplete> MapDataBase => TryGetDataBase<MapDataStringTemplete>(); //遊戲
        //public DataBase<CubeDataTemplete> CubeDataBase => TryGetDataBase<CubeDataTemplete>(); //遊戲
        public DataBase<UIDataBaseTemplete> UIDataBase => TryGetDataBase<UIDataBaseTemplete>();
        public DataBase<ItemDataBaseTemplete> ItemDataBase => TryGetDataBase<ItemDataBaseTemplete>(); //遊戲
        public DataBase<StoreDataBaseTemplete> StoreDataBase => TryGetDataBase<StoreDataBaseTemplete>();//商店
        //public DataBase<SoundEffectDatabaseTemplete> SoundEffectDatabase => TryGetDataBase<SoundEffectDatabaseTemplete>(); //最重要
        //public DataBase<AnimationDetailDatabaseTemplete> AnimationDetailDatabase => TryGetDataBase<AnimationDetailDatabaseTemplete>(); //遊戲
        //public DataBase<GameEndConditionTemplete> GameEndConditionDatabase => TryGetDataBase<GameEndConditionTemplete>(); //遊戲
        //public DataBase<HitEventConditionTemplete> HitEventConditionTemplete => TryGetDataBase<HitEventConditionTemplete>(); //遊戲
        //public DataBase<SkillTemplete> SkillDatabase => TryGetDataBase<SkillTemplete>(); //遊戲
        //public DataBase<EffectTemplete> EffectDatabase => TryGetDataBase<EffectTemplete>(); //遊戲
        //public DataBase<GuardianSpeedPercentageByCoinThresholdTemplete> GuardianSpeedPercentageDataBase => TryGetDataBase<GuardianSpeedPercentageByCoinThresholdTemplete>(); 
        //public DataBase<PlayerDropedCoinPercentageByCoinThresholdTemplete> PlayerDropedCoinPercentageDataBase => TryGetDataBase<PlayerDropedCoinPercentageByCoinThresholdTemplete>();

        List<IDataBase> databases = new List<IDataBase>();

        public DataBase<T> TryGetDataBase<T>() where T : class
        {
            DataBase<T> result = null;
            foreach (var item in databases)
            {
                if (item is DataBase<T> data)
                {
                    return data;
                }
            }
            if (result == null)
            {
                result = new DataBase<T>();
                databases.Add(result);
            }
            return result;
        }

    }

    public class DataBase<T> : IDataBase where T : class
    {
        public object[] DataArray { get; set; }
        public Type ThisDataType
        {
            get => typeof(T);
            set => throw new Exception();
        }
    }
    
    public interface IWithIdData
    {
        public int Id { get; set; }
    }
    public interface IWithNameData
    {
        public string Name { get; set; }
    }

    [Serializable]
    public class DatasPath
    {
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public string Path { get; set; }

        private UIType[] o;
    }
}

#region Boomer

/// <summary>
/// UI名稱
/// </summary>
public enum UIType
{
    MainMenu = 110, //主介面選單
    BagMenu = 120,                //背包選單
    StoreMenu = 130,             //商店選單
    GameOverMenu = 140,
    GameEndMenu = 150,
    
    SceneLoadingTransitionPanel = 210,
    GameStartTransitionPanel = 220,

    FadeInOutPanels = 230,
    
    SettingsWindow = 310,   //設定介面
    DialogueWindow = 320,
    
    GameHUD = 410,
}

/// <summary>
/// UI類型
/// </summary>
public enum UIGroup
{
    Menu = 10,       //有明確「進入/退出」行為，包含多個子選項
    Panel = 20,    //持續顯示或切換的子視圖
    Window = 30,  //彈出式介面，需遮罩背景
    HUD = 40,        //常駐於畫面邊角的疊加層
}

public class UIDataBaseTemplete :IWithIdData, IWithNameData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string UIGroup { get; set; }
    public GameObject PrefabPath { get; set; }

    public UIDataBaseTemplete Clone()
    {
        return new UIDataBaseTemplete()
        {
            Id = Id,
            Name = Name,
            UIGroup = UIGroup,
            PrefabPath = PrefabPath,
        };
    }
}

public class MapDataStringTemplete : IWithIdData, IWithNameData
{
    public string Name { get; set; }
    public int Id { get; set; }
    public string[][] MapaDataString { get; set; }
    public MapDataStringTemplete Clone()
    {
        var result = new MapDataStringTemplete()
        {
            Name = Name,
            Id = Id,
            MapaDataString = MapaDataString,
        };
        return result;
    }
}

public class GameEffectTemplete : IWithIdData, IWithNameData
{
    public string Name { get; set; }
    public int Id { get; set; }
    public GameObject PrefabPath { get; set; }

    public GameEffectTemplete Clone()
    {
        var result = new GameEffectTemplete()
        {
            Name = Name,
            Id = Id,
            PrefabPath = PrefabPath
        };
        return result;
    }
}

/// <summary>
/// 物品類型枚舉
/// </summary>
public enum ItemControllerType
{
    [Description("裝備")]
    Equipment = 10,
    [Description("消耗品")]
    Consumable = 20,
    [Description("材料")]
    Material = 30,
    [Description("重要物品")]
    KeyItem = 40,
}

/// <summary>
/// 物品品質種類
/// </summary>
public enum ItemRarityType
{
    [Description("劣等")]
    Poor = 10,
    [Description("普通")]
    Common = 20,
    [Description("優良")]
    Uncommon = 30,
    [Description("稀有")]
    Rare = 40,
}
public class ItemDataBaseTemplete :IWithIdData, IWithNameData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public GameObject PrefabPath { get; set; }
    public Sprite ItemIconPath { get; set; }
    public string ItemDescription { get; set; }
    public ItemControllerType ItemControllerType { get; set; }
    public ItemRarityType ItemRarityType { get; set; }
    public int ItemUseTimes { get; set; }
    public float LifeTime { get; set; }
    public int BaseValue { get; set; }
    public GameObject BuffPrefabPath { get; set; }

    public ItemDataBaseTemplete Clone()
    {
        return new ItemDataBaseTemplete()
        {
            Id = Id,
            Name = Name,
            PrefabPath = PrefabPath,
            ItemDescription = ItemDescription,
            ItemIconPath = ItemIconPath,
            ItemControllerType = ItemControllerType,
            ItemRarityType = ItemRarityType,
            ItemUseTimes = ItemUseTimes,
            LifeTime = LifeTime,
            BaseValue = BaseValue,
            BuffPrefabPath = BuffPrefabPath,
        };
    }
}

public class StoreDataBaseTemplete :IWithIdData, IWithNameData
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public string Name { get; set; }
    public int ItemId { get; set; }
    public int ItemBasePrice { get; set; }
    public int ItemQuantity { get; set; }
    public float Discount { get; set; }
    public int MaxPurchase { get; set; }
    public int RestockInterval { get; set; }

    public StoreDataBaseTemplete Clone()
    {
        return new StoreDataBaseTemplete()
        {
            Id = Id,
            StoreId = StoreId,
            Name = Name,
            ItemId = ItemId,
            ItemBasePrice = ItemBasePrice,
            ItemQuantity = ItemQuantity,
            Discount = Discount,
            MaxPurchase = MaxPurchase,
            RestockInterval = RestockInterval,
        };
    }
}

public class SoundEffectDatabaseTemplete : IWithNameData, IWithIdData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public AudioClip SoundEffect { get; set; }

    public SoundEffectDatabaseTemplete Clone()
    {
        return new SoundEffectDatabaseTemplete()
        {
            Id = Id,
            Name = Name,
            SoundEffect = SoundEffect,
        };
    }
}

public class AnimationDetailDatabaseTemplete : IWithNameData
{
    public string Name { get; set; }
    public int AnimationFrameDuration { get; set; }
    public AnimationDetailDatabaseTemplete Clone()
    {
        return new AnimationDetailDatabaseTemplete()
        {
            Name = Name,
            AnimationFrameDuration = AnimationFrameDuration,
        };
    }
}
public class GameEndConditionTemplete : IWithNameData
{
    public string Name { get; set; }
    public int GameEndNeedCoin { get; set; }
    public float GameTotalTime { get; set; }
    public int GameUsedMapID { get; set; }
    public GameEndConditionTemplete Clone()
    {
        return new GameEndConditionTemplete()
        {
            Name = Name,
            GameEndNeedCoin = GameEndNeedCoin,
            GameTotalTime = GameTotalTime,
            GameUsedMapID = GameUsedMapID
        };
    }
}

public class HitEventConditionTemplete : IWithNameData
{
    public string Name { get; set; } //AttackerName
    public List<object> ConditionBool { get; set; }
    public bool GetBeAttackerBool(HurtCondition beAttacker)
    {
        bool result = default;
        result = (bool)ConditionBool[(int)beAttacker];
        return result;
    }

    public HitEventConditionTemplete Clone()
    {
        return new HitEventConditionTemplete()
        {
            Name = Name,
            ConditionBool = ConditionBool
        };
    }
}

[Serializable]
public enum HurtCondition
{
    Ninja, Guard, Coin, Item, Cube
}
[Serializable]
public enum HitCondition
{
    Ninja, Guard, NinjaCollecter, GuardianCollecter
}
public class GuardianSpeedPercentageByCoinThresholdTemplete : IWithIdData
{
    public int Id { get; set; }
    public int CoinThreshold { get; set; }
    public float SpeedRatio { get; set; }
    public GuardianSpeedPercentageByCoinThresholdTemplete Clone()
    {
        return new GuardianSpeedPercentageByCoinThresholdTemplete()
        {
            Id = Id,
            CoinThreshold = CoinThreshold,
            SpeedRatio = SpeedRatio,
        };
    }
}

public class PlayerDropedCoinPercentageByCoinThresholdTemplete : IWithIdData
{
    public int Id { get; set; }
    public int CoinThreshold { get; set; }
    public float DropedRatio { get; set; }
    public PlayerDropedCoinPercentageByCoinThresholdTemplete Clone()
    {
        return new PlayerDropedCoinPercentageByCoinThresholdTemplete()
        {
            Id = Id,
            CoinThreshold = CoinThreshold,
            DropedRatio = DropedRatio,
        };
    }
}
public class SkillTemplete : IWithNameData
{
    public string Name { get; set; }
    public GameObject SkillPrefabPath { get; set; }
    public GameObject SkillEffectPrefabPath { get; set; }
    public SkillTemplete Clone()
    {
        return new SkillTemplete()
        {
            Name = Name,
            SkillPrefabPath = SkillPrefabPath,
            SkillEffectPrefabPath = SkillEffectPrefabPath
        };
    }
}

public class EffectTemplete : IWithNameData
{
    public string Name { get; set; }
    public GameObject EffectPrefabPath { get; set; }
    public EffectTemplete Clone()
    {
        return new EffectTemplete()
        {
            Name = Name,
            EffectPrefabPath = EffectPrefabPath,
        };
    }
}

#endregion

