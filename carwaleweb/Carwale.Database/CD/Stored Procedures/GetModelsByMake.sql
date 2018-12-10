IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[GetModelsByMake]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[GetModelsByMake]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/*
	THIS STORED PROCEDURE COLLECTS MODEL DETAILS BASED ON MAKEID FROM VIEW CD.vwMMV

	WRITTEN BY : AMIT VERMA ON 27 SEP 2012

	Changes History:
       
       Edited By               		EditedON               		Description
       ----------------       -----------------              	-----------------------
       --------                	-----------       	             ----------------------
*/
CREATE PROCEDURE [CD].[GetModelsByMake] @MakeID INT
AS
  BEGIN
      SELECT ModelId,
             Model
      FROM   CD.vwMMV--carwale..vwMMV--
      WHERE  MakeId = @MakeID
      UNION
      SELECT 0,
             '--select model--'
             ORDER BY Model ASC
  END
