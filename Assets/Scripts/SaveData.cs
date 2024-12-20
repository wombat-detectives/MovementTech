[System.Serializable]
public class SaveData
{
    public bool tutorialCompleted;
    public bool key0Collected;
    public bool key1Collected;
    public bool key2Collected;
    public bool key3Collected;
    public bool key4Collected;

    public int coins;

    public float key0Time;
    public float key1Time;
    public float key2Time;
    public float key3Time;
    public float key4Time;

    public SaveData()
    {
        tutorialCompleted = GameMaster.tutorialComplete;
        key0Collected = GameMaster.keyCollectedStatus[0];
        key1Collected = GameMaster.keyCollectedStatus[1];
        key2Collected = GameMaster.keyCollectedStatus[2];
        key3Collected = GameMaster.keyCollectedStatus[3];

        coins = GameMaster.coins;

        key0Time = GameMaster.times[0];
        key1Time = GameMaster.times[1];
        key2Time = GameMaster.times[2];
        key3Time = GameMaster.times[3];
        key4Time = GameMaster.times[4];
    }

}
