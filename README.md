# FMUSI-PROJEKTNI
### Projektni zadatak iz *Formalnih metoda u softverskom inženjerstvu*

Napisati biblioteku za rad sa regularnim jezicima koji se reprezentuju konačnim automatima (DKA i ε-NKA)
i/ili regularnim izrazima. Realizovati sljedeće funkcionalnosti za rad sa regularnim jezicima:

• Izvršavanje konačnih automata (DKA i ε-NKA) formiranih od strane korisnika biblioteke

• Konstrukcije reprezentacija unije, presjeka, razlike i spajanja jezika, komplementa jezika i rezultata
primjene Kleenove zvijezde nad jezikom, uz podršku za ulančavanje operacija

• Određivanje dužina najkraće riječi u jeziku

• Transformacija ε-NKA u DKA, kao i transformacija regularnog izraza u konačni automat


• Napisati aplikaciju koja učitava specifikaciju reprezentacije regularnog jezika (podržane reprezentacije su
DKA, ε-NKA i regularni izraz) i koja ispituje pripadnost specificiranih stringova reprezentovanom jeziku.
Izvršiti leksičku analizu specifikacije reprezentacije regularnog jezika i u slučaju leksičkih nepravilnosti
evidentirati broj relevantnih linija specifikacije koje sadrže nepravilnosti.


• Napisati aplikaciju za generisanje programskog koda za simulaciju mašine stanja na osnovu specifikacije
determinističkog konačnog automata. Omogućiti da, za svako formirano stanje, korisnici generisnog koda
mogu da specificiraju reakciju na događaje ulaska u stanje i izlaska iz stanja. Omogućiti da korisnici
generisanog koda za svaki simbol alfabeta automata mogu da specificiraju reakciju na izvršavanje prelaza
za taj simbol, a koja može da zavisi od stanja iz kojeg se vrši prelaz. Treba biti omogućeno nadovezivanje
reakcija, tako da se efektivno može formirati lanac reakcija na neki događaj.

Pridržavati se principa OOP, principa pisanja čistog koda, DRY principa, principa ponovne upotrebe koda,
SOLID principa i konvencija za korišteni programski jezik. Pravilno dokumentovati kod upotrebom
komentara. Obratiti pažnju na performanse po pitanju vremena izvršavanja i zauzeća memorije pri pisanju
i odabiru algoritama i struktura podataka, te dokumentovati asimptotsku kompleksnost istih. Pravilno
pokriti funkcionalnosti iz ove specifikacije sa jediničnim testovima. Dozvoljeno je korištenje standardne
biblioteke, ali ne i nestandardnih biblioteka, za odabrani programski jezik.

**===============================[ENG]===========================**

The idea of this application is to make a library for working with regular languages. Regular languages are represented with finite automatas (DFA and Epsilon NFA). Functionalities that are implemented are:

• Running finite automatas that are created from the user of the library
• Constructing union, intersection, difference, connection and complement between languages and applying Kleene star on language, with chaining operations enabled
• Finding shortest word in a language.
• Transforming Epsilon NFA into DFA, as well as transforming regular expressions into finite automata

• Second application required in this specification is the one that allows user to check if specified strings belong to the represented language. For each regular language, lexical analysis is required to check whether there are lexical errors, and if there are lexical errors, user should know how many of them are there in concrete specification.

• Make an application that will generate programming code which will simulate state machine based on specification of DFA. For each formed state, users of generated code can specify reaction on events. Events are a term that considers either entering or exiting a state. This application is supposed to allow users of generated code to specify reaction on event for each symbol in the alphabet. Chaining of the operations is required, so we can form chaining of reactions based on some event. Besides these requirements, code is commented, and asymptotic complexity is documented.

These applications are made in C#, and this is a console application.
