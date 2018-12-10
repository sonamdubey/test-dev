IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetInspectionDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetInspectionDetails]
GO

	-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date:	Jan 30,2015
-- Description: To get inspection response.
-- =============================================
CREATE PROCEDURE AbSure_GetInspectionDetails 
	-- Add the parameters for the stored procedure here
	@AbSure_CarDetailsId BIGINT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT DISTINCT CP.AbSure_QCarPartsId,CP.PartName,dbo.[AbSure_GetPartInspectionDetails](API.AbSure_QCarPartResponsesId) AS Response
	FROM			AbSure_QCarParts CP WITH(NOLOCK)
	LEFT JOIN		AbSure_PartsInspectionData API WITH(NOLOCK) ON API.AbSure_QCarPartsId = CP.AbSure_QCarPartsId AND API.AbSure_CarDetailsId = @AbSure_CarDetailsId
END
