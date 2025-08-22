SELECT MAX(idDokument)
  FROM [CascoDB].[casco].[Dokument]


  SELECT *
  FROM [CascoDB].[casco].[Dokument]

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