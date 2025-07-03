SELECT *
  FROM [TestLogDB].[test].[tReportSumary]
  --where IdTestiranje = 10 
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
  --where IdTestiranje = 10
  ORDER BY [IdTestiranje] DESC, [IdTest] ASC;


  SELECT [IdTestiranje],
                  [IdGreske],
                  [IdTest],
				  [DatumVremeGreske],
	              [Lokacija],
				  [Poruka],
				  [StackTrace]
             FROM [TestLogDB].[test].[tErrorLog]
			 --where IdTestiranje = 10
            ORDER BY [IdTestiranje] DESC, [IdGreske] ASC;