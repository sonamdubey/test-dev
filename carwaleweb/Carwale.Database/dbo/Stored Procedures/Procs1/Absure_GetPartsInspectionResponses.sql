IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_GetPartsInspectionResponses]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_GetPartsInspectionResponses]
GO

	
-- =============================================
-- Author:		Ruchira Patil
-- Create date: 9th Mar 2015
-- Description:	To get parts inspection responses
-- =============================================
CREATE PROCEDURE [dbo].[Absure_GetPartsInspectionResponses]
AS
BEGIN
	SELECT AbSure_QCarPartResponsesId,Response FROM AbSure_QCarPartResponses
	WHERE IsActive=1
END
