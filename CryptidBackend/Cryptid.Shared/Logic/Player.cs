

[System.Serializable]
public class Player
{
    public Player(string id)
    {
        this.Id = id;
    }

    public string Id;
    public string Name;
    public int Gold = 0;
    public int Level = 1;
    public int Experience = 0;

}