IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_GetModels]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_GetModels]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- =============================================
-- Author:		Chetan Kane	
-- Create date: 19th June 2012
-- Description:	to fetch model for sell car wigets
-- =============================================
CREATE PROCEDURE [Classified_GetModels] 
	-- Add the parameters for the stored procedure here
@MakeId INT
AS
BEGIN

	SET NOCOUNT ON;
	
	SELECT Id AS Value,Name AS Text 
	FROM CarModels 
	WHERE IsDeleted = 0 AND Futuristic = 0 AND Used = 1 AND CarMakeId = @MakeId 
	ORDER BY Name
END

