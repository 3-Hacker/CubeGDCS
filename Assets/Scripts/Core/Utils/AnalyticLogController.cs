using UnityEngine;

public class AnalyticLogController : MonoBehaviour
{
    public void LevelStart(int _levelID)
    {
        //Gameguru.Analytics.LogLevelStarted(((1000 * _levelID) + 1).ToString());
        Debug.Log("Data Start:" + ((1000 * _levelID) + 1));
    }


    public void LevelSucces(int _levelID)
    {
        //Gameguru.Analytics.LogLevelSucceeded(((1000 * _levelID) + 1).ToString());
        Debug.Log("Data Succes :" + ((1000 * _levelID) + 1));
    }

    public void LevelFail(int _levelID)
    {
       // Gameguru.Analytics.LogLevelFailed(((1000 * _levelID) + 1).ToString());
        Debug.Log("Data Fail :" + ((1000 * _levelID) + 1));
    }
}