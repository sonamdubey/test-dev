IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[GetCarData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[GetCarData]
GO

	--[CD].[GetCarData] 'vers', 198
--[CD].[GetCarData] 'mode', 2

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/*
	THIS STORED PROCEDURE RETURNS CAR MAKE, MODEL OR VERSION LIST DEPENDING ON THE TYPE PROVIDED
	AND TYPE ID SENT AS INPUT PARAMETERS. DEFAULT IT RETURNS CAR MAKE WHEN NO PARAMETER IS PASSED
	
	WRITTEN BY : SHIKHAR MAHESHWARI ON 5 APR 2012

	Changes History:
       
       Edited By               		EditedON               		Description
       -----------------          ---------------               	---------------------
       --------                	-----------       	             ----------
*/

CREATE PROCEDURE [CD].[GetCarData]
@type CHAR(4) = '',
@typeId INT = 0

AS

BEGIN
	IF (UPPER(@type) = 'MAKE' OR @type = '')	
	BEGIN
		SELECT DISTINCT
			Make, MakeId
		FROM 
		   CD.vwMMV WITH(NOLOCK)
		ORDER BY Make
	END
	ELSE IF (UPPER(@type) = 'MODE')
	BEGIN
	SELECT Model
		, ModelId
		FROM(
		SELECT DISTINCT
			CASE VW.IsModelNew
		WHEN 1
			THEN Model
		ELSE Model + '*'
		END AS 
		Model
		, ModelId ,IsModelNew
		FROM 
			CD.vwMMV VW WITH(NOLOCK)
		WHERE 
			MakeId = @typeId
		 )AS Tab
		 ORDER BY  IsModelNew DESC,Model
	END
	ELSE IF (UPPER(@type) = 'VERS')
	BEGIN
		SELECT
		CASE IsVerionNew
		WHEN 1
			THEN [Version]
		ELSE [Version] + '*'
		END AS [Version], VersionId, CD.IsVersionDataPresent(VersionId) AS IsDataPresent--,IsVerionNew
		FROM 
			CD.vwMMV WITH(NOLOCK)
		WHERE
			ModelId = @typeId
		ORDER BY IsVerionNew DESC,[Version]
		
	END
END
