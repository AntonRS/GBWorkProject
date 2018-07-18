using Game;
using Game.CommandUI;
using UnityEngine;

public class BuildBase : MonoBehaviour, IRangeMarkerAssignee, ICommandButtonActuator
{

    /// <summary>
    /// текущий уровень апргрейда объекта
    /// </summary>
    public string RangeUpgrade;
    /// <summary>
    /// текущий радиус действия объекта
    /// </summary>
    public float Range;
    /// <summary>
    /// фейковый радиус, возвращаемый объектом, если работаем с превью команды Upgrade
    /// </summary>
    private float? _fakeRange = null;
    private float _rocketTowerBuildRange;
    private float _lazerTowerBuildRange;
    private float _gunTowerBuildRange;
    
    
    private const string RangeUpgrade2 = "RangeUpgrade2";
    

    void Start()
    {
        _rocketTowerBuildRange = GameManager.Instance.GetTowersManager.rocketTowers[0].AttackRange;
        _lazerTowerBuildRange = GameManager.Instance.GetTowersManager.lazerTowers[0].AttackRange;
        _gunTowerBuildRange = GameManager.Instance.GetTowersManager.gunTowers[0].AttackRange;

    }
    ///???????????????????????????????
    public bool TestCommandButtonShouldShow(CommandType ofType, CommandButton viaButton)
    {
        if (ofType == CommandType.Upgrade)
            return this.RangeUpgrade != RangeUpgrade2;
        
        return true;
    }

    public void PreviewCommandBegan(CommandType ofType, GameObject forObject, CommandButton viaButton)
    {
        if (ofType == CommandType.Build)
        {
            switch(viaButton.Meta)
            {
                case "BuildRocketTower":
                    _fakeRange = _rocketTowerBuildRange;
                    break;
                case "BuildLazerTower":
                    _fakeRange = _lazerTowerBuildRange;
                    break;
                case "BuildGunTower":
                    _fakeRange = _gunTowerBuildRange;
                    break;
            }
        }
        
    }
    
    public void PreviewCommandEnd(CommandType ofType, GameObject forObject, CommandButton viaButton)
    {
        this._fakeRange = null;
    }
    public void ExecuteCommand(CommandType ofType, GameObject forObject, CommandButton viaButton)
    {
        if (ofType == CommandType.Build)
        {
            this._fakeRange = null;

            switch (viaButton.Meta)
            {
                case "BuildRocketTower":
                    GameManager.Instance.GetTowersManager.BuildRocketTower(transform);
                    
                    break;
                case "BuildLazerTower":
                    GameManager.Instance.GetTowersManager.BuildLazerTower(transform);
                    break;
                case "BuildGunTower":
                    GameManager.Instance.GetTowersManager.BuildGunTower(transform);
                    break;
            }
        }
    }
    public float OnRangeRequested()
    {
        return this._fakeRange ?? this.Range;
    }
}
