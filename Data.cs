namespace Program;

public class Data
{
    private bool[] lexs;
    private List<uint> lines;

    public Data(uint lexnb)
    {
        lexs = new bool[lexnb];
        lines = new List<uint>();
    }

    public bool ExistsIn(uint lexId)
    {
        return lexs[lexId] == true;
    }

    public List<uint> GetLines()
    {
        return lines;
    }

    public void AddLex(uint lexIdx)
    {
        lexs[lexIdx] = true;
    }

    public void AddLine(uint line)
    {
        lines.Add(line);
    }
}
