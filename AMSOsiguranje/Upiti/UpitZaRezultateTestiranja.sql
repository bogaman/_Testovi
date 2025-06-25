SELECT TOP (10) *
  FROM [TestLogDB].[test].[tReportSumary]
  ORDER BY [IdTestiranje] DESC;


  SELECT TOP (10) [IdTestiranje], 
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


  SELECT TOP (10) [IdTestiranje],
                  [IdGreske],
                  [IdTest],
				  [DatumVremeGreske],
	              [Lokacija],
				  [Poruka],
				  [StackTrace]
             FROM [TestLogDB].[test].[tErrorLog]
            ORDER BY [IdTestiranje] DESC, [IdGreske] ASC;