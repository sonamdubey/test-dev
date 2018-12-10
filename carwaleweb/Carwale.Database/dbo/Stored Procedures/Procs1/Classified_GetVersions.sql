IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_GetVersions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_GetVersions]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- =============================================
-- Author:		Chetan Kane	
-- Create date: 19th June 2012
-- Description:	to fetch Versions for sell car wigets
-- Modified by: vivek gupta on 20-1-2014, removed used=1 condition. coz used cars can be new
-- =============================================
CREATE PROCEDURE [dbo].[Classified_GetVersions] 
	-- Add the parameters for the stored procedure here
@ModelId INT
AS
BEGIN

	SET NOCOUNT ON;
	
	SELECT Id AS Value,Name AS Text 
	FROM CarVersions 
	WHERE IsDeleted = 0 
	AND Futuristic = 0 
	--AND Used=1   -- commented by vivek gupta on 20-1-2014, removed used=1 condition. coz used cars can be new
	AND CarModelId = @ModelId 
	ORDER BY Name
END

