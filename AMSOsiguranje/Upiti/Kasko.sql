
SELECT * FROM [CascoDB].[casco].[Dokument]

SELECT * FROM [CascoDB].[casco].[ZahtevZaIzmenu]

SELECT TOP 1 [Dokument].[idDokument] FROM [CascoDB].[casco].[Dokument]
					LEFT JOIN [CascoDB].[casco].[ZahtevZaIzmenu] ON [CascoDB].[casco].[Dokument].[idDokument] = [CascoDB].[casco].[ZahtevZaIzmenu].[idDokument] 
					WHERE [Dokument].[idStatus] = 2 AND [Dokument].[idKorisnik] = 1001 AND [rokTrajanjaOsiguranja] = 2 AND [datumIsteka] > DATEADD(day, -30, CAST(GETDATE() AS DATE))
					GROUP BY [CascoDB].[casco].[Dokument].[idDokument] 
					ORDER BY COUNT([CascoDB].[casco].[ZahtevZaIzmenu].[idDokument]) ASC;

SELECT MAX(idDokument) FROM [CascoDB].[casco].[Dokument]


SELECT * FROM [CascoDB].[casco].[DokumentHistory]
SELECT MAX(idDokument) FROM [CascoDB].[casco].[DokumentHistory]

SELECT * FROM [CascoDB].[casco].[DokumentPoddokument]

SELECT * FROM [CascoDB].[casco].[DokumentPoddokumentHistory]


SELECT * FROM [CascoDB].[casco].[ZahtevZaIzmenu]

SELECT * FROM [CascoDB].[casco].[ZahtevZaIzmenu]
         ORDER BY idDokument ASC





SELECT TOP 1 [Dokument].[idDokument] FROM [CascoDB].[casco].[Dokument]
					JOIN [CascoDB].[casco].[ZahtevZaIzmenu] ON [CascoDB].[casco].[Dokument].[idDokument] = [CascoDB].[casco].[ZahtevZaIzmenu].[idDokument] 
					GROUP BY [CascoDB].[casco].[Dokument].[idDokument] 
					ORDER BY COUNT([CascoDB].[casco].[ZahtevZaIzmenu].[idDokument]) ASC;
  
  
  

  

  


  SELECT MIN (idDokument) AS IdDokument
  FROM [CascoDB].[casco].[Dokument] 
  WHERE ([Dokument].[idDokument] > 0 AND [idKorisnik] = 1001 AND [idProizvod] = 9 AND [idStatus] = 2  AND [datumIsteka] > CAST(GETDATE() AS DATE)) ;


  SELECT * FROM [CascoDB].[casco].[Dokument] 
           WHERE ([Dokument].[idDokument] > 0 AND [idKorisnik] = 1001 AND [idProizvod] = 9 AND [idStatus] = 2  AND [datumIsteka] > CAST(GETDATE() AS DATE)) 
           ORDER BY [brojUgovora] DESC, [idDokument] DESC;


SELECT [idDokument]
FROM (
    SELECT 
        [idDokument],
        [brojUgovora],
        Kolona3,
        ROW_NUMBER() OVER (PARTITION BY Kolona3 ORDER BY Kolona2 DESC, Kolona1 ASC) as rn
    FROM [CascoDB].[casco].[Dokument]
) as Podupit
WHERE rn = 1;