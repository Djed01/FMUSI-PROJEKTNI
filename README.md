# FMUSI-PROJEKTNI
Projektni zadatak iz Formalnih metoda u softverskom inženjerstvu.

Napisati biblioteku za rad sa regularnim jezicima koji se reprezentuju konačnim automatima (DKA i ε-NKA)
i/ili regularnim izrazima. Realizovati sljedeće funkcionalnosti za rad sa regularnim jezicima:

• Izvršavanje konačnih automata (DKA i ε-NKA) formiranih od strane korisnika biblioteke

• Konstrukcije reprezentacija unije, presjeka, razlike ispajanja jezika, komplementa jezika irezultata
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
