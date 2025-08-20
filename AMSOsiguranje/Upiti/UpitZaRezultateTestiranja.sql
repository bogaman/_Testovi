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
  --where IdTestiranje = 3
  ORDER BY [IdTestiranje] DESC, [IdTest] ASC;


  SELECT *
    FROM [TestLogDB].[test].[tErrorLog] 
	--where IdTestiranje = 3
ORDER BY [IdTestiranje] DESC, [IdGreske] ASC;



--DELETE FROM [TestLogDB].[test].[tReportSumary] 
--WHERE IdTestiranje IN (2,3, 4);