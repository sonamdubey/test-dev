IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DD_GetPersonContactPoints]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DD_GetPersonContactPoints]
GO

	


-------------------------------------------------------------------------------------------

-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <10/11/2014>
-- Description:	<Get Contact Points>
-- =============================================
CREATE PROCEDURE [dbo].[DD_GetPersonContactPoints] 
	@DD_ContactPersonId	INT
AS
BEGIN
	SELECT ContactNumber ,DD_ContactPersonId as ContactPersonId,IsPrimary,ContactType,DD_ContactPersonId FROM DD_ContactPoints 
	WHERE DD_ContactPersonId = CASE WHEN @DD_ContactPersonId = -1 THEN DD_ContactPersonId ELSE @DD_ContactPersonId END 	
END

