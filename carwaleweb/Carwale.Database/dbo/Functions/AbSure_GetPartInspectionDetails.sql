IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetPartInspectionDetails]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[AbSure_GetPartInspectionDetails]
GO

	-- =============================================
-- Author:		Tejashree Patil
-- Create date: 30-01-2015
-- Description:	To get Comma seperated list of Responses of Car Parts
-- =============================================
CREATE FUNCTION [dbo].[AbSure_GetPartInspectionDetails] 
(
	@AbSure_ResponseIds VARCHAR(MAX)
)
RETURNS VARCHAR(MAX)
AS
BEGIN
	DECLARE		@Response	VARCHAR(MAX) = NULL
	/*SELECT	    @Response = COALESCE(@Response+',','') + CONVERT(VARCHAR,CPR.Response)
	FROM	    AbSure_QCarParts CP WITH(NOLOCK)
				INNER JOIN  AbSure_PartsInspectionData API	WITH(NOLOCK) ON API.AbSure_QCarPartsId = CP.AbSure_QCarPartsId
				INNER JOIN  AbSure_QCarPartResponses CPR	WITH(NOLOCK) ON CPR.AbSure_QCarPartResponsesId = API.AbSure_QCarPartResponsesId
	WHERE		API.AbSure_CarDetailsId = @AbSure_CarDetailsId 
				AND API.AbSure_QCarPartsId = @AbSure_QPartId
	RETURN		@Response*/
	SELECT	    @Response = (COALESCE(@Response + ', ', '') + CAST(CPR.Response AS VARCHAR(500)))
	FROM	    AbSure_QCarPartResponses CPR
	WHERE		CPR.AbSure_QCarPartResponsesId IN (SELECT ListMember FROM [dbo].[fnSplitCSV_WithId] (@AbSure_ResponseIds))

	RETURN		ISNULL(@Response, 'Excellent')
END
