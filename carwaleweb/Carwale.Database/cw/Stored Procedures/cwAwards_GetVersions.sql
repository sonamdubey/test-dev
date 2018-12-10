IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[cwAwards_GetVersions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[cwAwards_GetVersions]
GO

	-- =============================================
-- Author:		Vikas
-- Create date: 24/01/2013
-- Description:	To get all the versions excet the deleted ones for a particular model
-- Modifier:	Vaibhav K (12-Feb-2013)	
--				Futuristic(Upcoming cars removed from the list) cars 
-- =============================================
CREATE PROCEDURE [cw].[cwAwards_GetVersions] 
	-- Add the parameters for the stored procedure here
	@ModelId NUMERIC 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT ID AS Value, Name AS Text FROM CarVersions WHERE CarModelId = @ModelId AND IsDeleted = 0 AND Futuristic = 0 ORDER BY Text
	
END