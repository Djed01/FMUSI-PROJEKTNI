using RegularLanguageAnalyser;

AutomatLexer automatLexer = new("automat.txt");
automatLexer.analyse();

RegexLexer regexLexer = new("regex.txt");
regexLexer.analyse();