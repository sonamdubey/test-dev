IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_GetStateList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_GetStateList]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[BA_GetStateList]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT S.ID AS ID, S.Name AS Name, S.StateCode AS Code FROM States  AS S  WITH(NOLOCK) WHERE S.IsDeleted = 0 

END
