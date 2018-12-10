IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[GetMakes]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[GetMakes]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/*
	THIS STORED PROCEDURE COLLECTS MAKE DETAILS FROM VIEW CD.vwMMV

	WRITTEN BY : AMIT VERMA ON 27 SEP 2012

	Changes History:
       
       Edited By               		EditedON               		Description
       ----------------       -----------------              	-----------------------
       --------                	-----------       	             ----------------------
*/
CREATE PROCEDURE [CD].[GetMakes]
AS
  BEGIN
      SELECT MakeID,
             Make
      FROM   CD.vwMMV--carwale..vwMMV--
      UNION
      SELECT 0,
             '--select make--'
             ORDER BY Make ASC
  END
