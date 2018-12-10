IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[GetUnitTypes]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[GetUnitTypes]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/*
	THIS STORED PROCEDURE RETURNS CAR MAKE, MODEL OR VERSION LIST DEPENDING ON THE TYPE PROVIDED
	AND TYPE ID SENT AS INPUT PARAMETERS. DEFAULT IT RETURNS CAR MAKE WHEN NO PARAMETER IS PASSED
	
	WRITTEN BY : SHIKHAR MAHESHWARI ON 22 JUN 2012

	Changes History:
       
       Edited By               		EditedON               		Description
       -----------------          ---------------               	---------------------
       --------                	-----------       	             ----------
*/

CREATE PROCEDURE [CD].[GetUnitTypes]

AS

BEGIN

SELECT
	UnitTypeId,
	Name
FROM
	CD.UnitTypes

END
