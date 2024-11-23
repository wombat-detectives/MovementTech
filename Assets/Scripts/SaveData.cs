using UnityEngine;

[System.Serializable]
public class SaveData
{
    public bool tutorialCompleted;
    public bool lvl1Key1;
    public bool lvl1key2;
    public bool lvl2key1;
    public bool lvl2key2;

    public SaveData(GameMaster game)
    {
        tutorialCompleted = game.tutorialComplete;
        lvl1Key1 = game.lvl1Key1;
        lvl1key2 = game.lvl1key2;
        lvl2key1 = game.lvl2key1;
        lvl2key2 = game.lvl2key2;
    }

}
