using RegularLanguageAnalyser;


AutomatLexer automatLexer = new("automat.txt");
automatLexer.analyse();


RegexLexer regexLexer = new("regex.txt");
regexLexer.analyse();


/*
AutomatLexer automatLexer = new("automat1.txt");  // 8 linija simbol duzi od jedan
automatLexer.analyse();
*/

/*
AutomatLexer automatLexer = new("automat2.txt");  // 3 linija nekorektan format stanja
automatLexer.analyse();
*/

/*
AutomatLexer automatLexer = new("automat3.txt"); // 11 linija nepostojeci simbol
automatLexer.analyse();
*/

/*
RegexLexer regexLexer = new("regex1.txt"); // 3: nekorektan format
regexLexer.analyse();
*/

/*
RegexLexer regexLexer = new("regex2.txt"); // 3: neocekivani simbol * na tom mjestu
regexLexer.analyse();
*/

/*
RegexLexer regexLexer = new("regex3.txt"); // 13: nebalansirane zagrade
regexLexer.analyse();
*/