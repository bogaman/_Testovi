SELECT *
  FROM [StrictEvidenceDB].[strictevidence].[tObrasci]
  --where IDLokacijaZaduzena = 11
  --where  IdTipObrasca = 1 and SerijskiBroj = 34661600
  where SerijskiBroj = 34661600

SELECT SerijskiBroj
FROM [StrictEvidenceDB].[strictevidence].[tObrasci]
WHERE IdTipObrasca = 1 AND [SerijskiBroj] BETWEEN 34680001 AND 34690000
GROUP BY SerijskiBroj
HAVING MAX(IDLokacijaZaduzena) = 0 AND MIN(IDLokacijaZaduzena) = 0
ORDER BY SerijskiBroj ASC;


SELECT MIN([SerijskiBroj]) AS PrviSlobodanUCentralnomMagacinu
FROM (
    SELECT [SerijskiBroj]
    FROM [StrictEvidenceDB].[strictevidence].[tObrasci]
	WHERE [IdTipObrasca] = 1 AND [SerijskiBroj] BETWEEN 34680001 AND 34690000
    GROUP BY [SerijskiBroj]
    HAVING MAX([IDLokacijaZaduzena]) = 0 AND MIN([IDLokacijaZaduzena]) = 0
) AS Podskup;