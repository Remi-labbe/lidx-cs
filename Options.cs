namespace Program;

enum OptionCase
{
    Asis,
    Upper,
    Lower
}

struct Options
{
    public bool sort;
    public OptionCase changeCase;
    public string inputPath;

    public Options()
    {
        sort = false;
        changeCase = OptionCase.Asis;
        inputPath = "";
    }

    public static void ShowHelp()
    {
        Console.WriteLine("Usage: ./lidx -uisShl \"Hello World\" - lex1.txt ... < file.txt");
        Console.WriteLine("Recherche des mots d'une reunion de lexiques dans un texte.");
        Console.WriteLine("Lexicals can be strings in double quotes or files preceded by a dash.");
        Console.WriteLine();
        Console.WriteLine("\t-h\tShow help");
        Console.WriteLine("\t-u\tAll Uppercase");
        Console.WriteLine("\t-l\tAll Lowercase");
        Console.WriteLine("\t-s\tDo not change case");
        Console.WriteLine("\t-S\tSort words on output");
        Console.WriteLine("\t-i file.txt\tUse file instead of stdin");
    }
}
