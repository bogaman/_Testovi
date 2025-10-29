SELECT *
  FROM [MtplDB].[mtpl].[Dokument] 
  WHERE idProizvod = 1
  ORDER BY idDokument DESC;

  SELECT *
  FROM [MtplDB].[mtpl].[DokumentPodaci] 
  ORDER BY idDokument DESC


  SELECT *
  FROM [MtplDB].[mtpl].[DokumentHistory] 
  WHERE idProizvod = 1
  ORDER BY idDokument DESC;

  SELECT * 
  FROM [MtplDB].[mtpl].[DokumentLice] order by idDokument DESC;


  SELECT *
  FROM [MtplDB].[mtpl].[Dokument] AS T1
  JOIN [MtplDB].[mtpl].[DokumentLice] AS T2 ON T1.idDokument = T2.idDokument
  WHERE T1.idProizvod = 1 AND idStatus = 2
  ORDER BY T1.idDokument DESC;


  SELECT *
  FROM [MtplDB].[mtpl].[DokumentFaktura]

