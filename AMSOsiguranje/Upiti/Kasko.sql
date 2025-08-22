SELECT MAX(idDokument)
  FROM [CascoDB].[casco].[Dokument]


  SELECT *
  FROM [CascoDB].[casco].[Dokument]

  SELECT *
  FROM [CascoDB].[casco].[DokumentHistory]
  
  SELECT MAX(idDokument)
  FROM [CascoDB].[casco].[DokumentHistory]

  SELECT *
  FROM [CascoDB].[casco].[DokumentHistory]

  SELECT *
  FROM [CascoDB].[casco].[ZahtevZaIzmenu]
  ORDER BY idDokument ASC


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