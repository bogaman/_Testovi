SELECT *
  FROM [WebshopDB].[webshop].[Dokument] where brojUgovora =''
  ORDER BY brojUgovora ASC --, brojPonude DESC;



  SELECT  [idDokument], [brojPonude], [brojUgovora] 
    FROM [WebshopDB].[webshop].[Dokument] 
    WHERE [brojUgovora] = ''
    AND [brojPonude] = (SELECT MAX([brojPonude]) FROM [WebshopDB].[webshop].[Dokument] WHERE [brojUgovora] = '' AND [idDokument] > 1564);