IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ESM_DeleteClient]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ESM_DeleteClient]
GO

	
-- =============================================
-- Author	:	Ajay Singh(12rd August 2015)
-- Description	:to Delete client Name 
-- =============================================
CREATE Procedure [dbo].[ESM_DeleteClient]
@ClientId INT = NULL
AS
BEGIN
UPDATE  ESM_OrganizationName SET IsActive=0 WHERE id=@ClientId	
END
