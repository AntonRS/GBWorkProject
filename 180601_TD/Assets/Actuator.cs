using System.Collections;
using Game.CommandUI;
using UnityEngine;

public class Actuator : MonoBehaviour, IRangeMarkerAssignee, ICommandButtonActuator
{
    public void ExecuteCommand(CommandType ofType, GameObject forObject, CommandButton viaButton)
    {
        if (true)
        {

        }
    }

    public float OnRangeRequested()
    {
        throw new System.NotImplementedException();
    }

    public void PreviewCommandBegan(CommandType ofType, GameObject forObject, CommandButton viaButton)
    {
        throw new System.NotImplementedException();
    }

    public void PreviewCommandEnd(CommandType ofType, GameObject forObject, CommandButton viaButton)
    {
        throw new System.NotImplementedException();
    }

    public bool TestCommandButtonShouldShow(CommandType ofType, CommandButton viaButton)
    {
        throw new System.NotImplementedException();
    }

    
}
