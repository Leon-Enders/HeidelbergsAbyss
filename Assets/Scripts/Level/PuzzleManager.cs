using System;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{

    public Action OnPuzzleReset;
    public Action OnPuzzleSucceeded;

    [SerializeField] private DoorManager doorToOpen;
    [SerializeField] private List<int> validButtonIndices;

    

    private int generalButtonCounter = 0;
    private int validButtonCounter = 0;
    public void AddTrigger(PuzzleTrigger puzzleTrigger)
    {
        generalButtonCounter++;
        
        if (validButtonIndices.Contains(puzzleTrigger.TriggerIndex))
        {
            validButtonCounter++;
        }

        if (validButtonCounter == validButtonIndices.Count)
        {
            OnPuzzleSucceeded?.Invoke();
            doorToOpen.HandleOpenDoor();
            return;
        }

        if (generalButtonCounter == validButtonIndices.Count)
        {
            generalButtonCounter = 0;
            validButtonCounter = 0;
            OnPuzzleReset?.Invoke();
        }
    }
}
