IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DD_DeleteContactPerson]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DD_DeleteContactPerson]
GO

	
-- =============================================
-- Author:		<Khushaboo patil>
-- Create date: <1/12/2014>
-- Description:	<Delete Contact Person>
-- =============================================
CREATE PROCEDURE [dbo].[DD_DeleteContactPerson]
@PersonId	INT
AS
BEGIN
	DELETE FROM DD_ContactPerson WHERE ID = @PersonId
END

