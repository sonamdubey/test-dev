IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_GetStateWCityList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_GetStateWCityList]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[BA_GetStateWCityList]
	@StateId INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	SELECT C.ID AS ID,C.Name AS Name FROM Cities AS C  WITH(NOLOCK) WHERE C.IsDeleted = 0 AND C.StateId = @StateId

END
