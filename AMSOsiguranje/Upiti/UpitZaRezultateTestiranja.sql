SELECT *
  FROM [TestLogDB].[test].[tReportSumary]
  ORDER BY [IdTestiranje] DESC;


  SELECT [IdTestiranje], 
                  [IdTest], 
                  [NazivTesta], 
                  [Rezultat], 
                  [Agent], 
                  [PocetakTesta], 
				  [KrajTesta], 
				  [OpisGreske], 
				  [StackTrace],
				  [BackOffice]
  FROM [TestLogDB].[test].[tReportIndividual]
  ORDER BY [IdTestiranje] DESC, [IdTest] ASC;


  SELECT [IdTestiranje],
                  [IdGreske],
                  [IdTest],
				  [DatumVremeGreske],
	              [Lokacija],
				  [Poruka],
				  [StackTrace]
             FROM [TestLogDB].[test].[tErrorLog]
            ORDER BY [IdTestiranje] DESC, [IdGreske] ASC;