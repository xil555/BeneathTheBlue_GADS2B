using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DoorTriggerInteraction : TriggerInteractionBase
{
    public enum DoorToSpawnAt
    { 
       None,
       One,
       Two,
       Three,
       Four,
       Five,
       Six,
    }

    [Header("Spawn TO")]
    [SerializeField] private DoorToSpawnAt DoorToSpawnTo;

    [SerializeField] private SceneField _sceneToLoad;

    [Space(10f)]
    [Header("THIS Door")]
    public DoorToSpawnAt CurrentDoorPosition;
    public override void Interact()
    {
        

        Debug.Log($"Door used: going to {DoorToSpawnTo} in scene {_sceneToLoad.SceneName}");
        SceneSwapManager.SwapSceneFromDoorUse(_sceneToLoad, DoorToSpawnTo);
    }
}
