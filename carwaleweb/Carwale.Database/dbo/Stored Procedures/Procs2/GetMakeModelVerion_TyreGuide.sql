IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetMakeModelVerion_TyreGuide]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetMakeModelVerion_TyreGuide]
GO

	
-- =======================================================================================
-- Author:		Akansha
-- Create date: 15/03/2013
-- Modified By: Akansha
-- Modification Date: 29/05/2013
-- Description:	SP for Distinct Make, Model and Version to Fetch Matching Tyres
-- Modification made to show only those make for which tyres are available
-- // To select distinct makes EXEC GetMakeModelVerion_TyreGuide '0'
-- // To select distinct models based on make name EXEC GetMakeModelVerion_TyreGuide '1','BMW'
-- // To select distinct versions based on model name EXEC GetMakeModelVerion_TyreGuide '2','M3'
-- ========================================================================================
CREATE  PROCEDURE   [dbo].[GetMakeModelVerion_TyreGuide] 
	@ConditionID SMALLINT
	,@FilterName NVARCHAR(50) = NULL
AS
IF (@ConditionID = '0')
BEGIN
	SELECT DISTINCT Make as value
	FROM Apollo_TyreGuideData
	WHERE IsOption1Available = 1 or IsOption2Available = 1 or IsOption3Available = 1
	ORDER BY value
END
ELSE
BEGIN
	IF (@ConditionID = '1')
	BEGIN
		SELECT DISTINCT Model as value
		FROM Apollo_TyreGuideData
		WHERE Make = @FilterName
		AND (IsOption1Available = 1 or IsOption2Available = 1 or IsOption3Available = 1)
		ORDER BY value
	END
	ELSE
	BEGIN
		SELECT DISTINCT VERSION as value
		FROM Apollo_TyreGuideData
		WHERE Model = @FilterName
		AND (IsOption1Available = 1 or IsOption2Available = 1 or IsOption3Available = 1)
		ORDER BY value
	END
END

