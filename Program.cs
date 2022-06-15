using System.Collections;
using Program;
// See https://aka.ms/new-console-template for more information

namespace wrapper
{
    public static class Program
    {
        public static readonly char[] DELIMS = " ,.;:/!?(){}\n\t".ToCharArray();
        public static void Main(string[] args)
        {
            List<string> strLex = new List<string>();
            List<string> fileLex = new List<string>();
            Options options = new Options();
            for (uint i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-":
                        ++i;
                        if (!File.Exists(args[i]))
                        {
                            Console.WriteLine("{0} doesn't exists, skiping...", args[i]);
                            continue;
                        }
                        else
                        {
                            fileLex.Add(args[i]);
                        }
                        break;
                    case "-h":
                        Options.ShowHelp();
                        return;
                    case "-S":
                        options.sort = true;
                        break;
                    case "-u":
                        options.changeCase = OptionCase.Upper;
                        break;
                    case "-l":
                        options.changeCase = OptionCase.Lower;
                        break;
                    case "-s":
                        options.changeCase = OptionCase.Asis;
                        break;
                    case "-i":
                        options.inputPath = args[++i];
                        break;
                    default:
                        strLex.AddRange(args[i].Split(DELIMS, StringSplitOptions.RemoveEmptyEntries));
                        break;
                }
            }
            Hashtable ht = new Hashtable();
            uint nbLex = (uint)(fileLex.Count() + (strLex.Count() == 0 ? 0 : 1));
            // List of the lexical names for table header
            List<string> lexNames = new List<string>();
            if (strLex.Count() != 0)
            {
                lexNames.Add("Strings");
                foreach (string w in strLex)
                {
                    string word = w;
                    switch (options.changeCase)
                    {
                        case OptionCase.Lower:
                            word = w.ToLower();
                            break;
                        case OptionCase.Upper:
                            word = w.ToUpper();
                            break;
                        default:
                            break;
                    }
                    if (!ht.Contains(word))
                    {
                        Data data = new Data(nbLex);
                        data.AddLex(0);
                        ht.Add(word, data);
                    }
                }
            }
            for (int i = 0; i < fileLex.Count(); i++)
            {
                uint currLexIdx = strLex.Count() == 0 ? (uint)i : (uint)i + 1;
                string filePath = fileLex[i];
                lexNames.Add(Path.GetFileName(filePath));
                string[] words = File.ReadAllText(filePath).Split(DELIMS, StringSplitOptions.RemoveEmptyEntries);
                foreach (string w in words)
                {
                    string word = w;
                    switch (options.changeCase)
                    {
                        case OptionCase.Lower:
                            word = w.ToLower();
                            break;
                        case OptionCase.Upper:
                            word = w.ToUpper();
                            break;
                        default:
                            break;
                    }
                    Data? data = ht[word] as Data;
                    if (data == null)
                    {
                        strLex.Add(word);
                        data = new Data(nbLex);
                        data.AddLex(currLexIdx);
                        ht.Add(word, data);
                    }
                    else
                    {
                        data.AddLex(currLexIdx);
                        ht[word] = data;
                    }
                }
            }
            string? line;
            uint lineNb = 1;
            StreamReader reader;
            if (options.inputPath == "")
            {
                reader = new StreamReader(Console.OpenStandardInput());
            }
            else
            {
                if (!File.Exists(options.inputPath))
                {
                    Console.WriteLine("{0} doesn't exists, quitting...", options.inputPath);
                    return;
                }
                reader = new StreamReader(File.OpenRead(options.inputPath));
            }
            while ((line = reader.ReadLine()) != null)
            {
                foreach (string w in line.Split(DELIMS, StringSplitOptions.RemoveEmptyEntries))
                {
                    string word = w;
                    switch (options.changeCase)
                    {
                        case OptionCase.Lower:
                            word = w.ToLower();
                            break;
                        case OptionCase.Upper:
                            word = w.ToUpper();
                            break;
                        default:
                            break;
                    }
                    Data? data = ht[word] as Data;
                    if (data != null)
                    {
                        data.AddLine(lineNb);
                        ht[word] = data;
                    }
                }
                ++lineNb;
            }
            if (options.sort)
            {
                strLex.Sort();
            }
            Console.Write("|{0,20}|", "");
            foreach (string lex in lexNames)
            {
                Console.Write("{0,20}|", lex);
            }
            // console.WriteLine("{0,30}|", "");
            Console.WriteLine();
            foreach (string w in strLex)
            {
                string word = w;
                switch (options.changeCase)
                {
                    case OptionCase.Lower:
                        word = w.ToLower();
                        break;
                    case OptionCase.Upper:
                        word = w.ToUpper();
                        break;
                    default:
                        break;
                }
                Console.Write("|{0,20}|", word);
                Data? wordData = ht[word] as Data;
                if (wordData == null)
                {
                    throw new Exception();
                }
                for (uint i = 0; i < nbLex; i++)
                {
                    Console.Write("{0,20}|", (wordData.ExistsIn(i) ? "X" : ""));
                }
                bool first = true;
                foreach (uint l in wordData.GetLines())
                {
                    Console.Write("{0}{1}", (first ? "" : ","), l);
                    if (first) first = false;
                }
                Console.WriteLine();
            }
        }
    }
}
