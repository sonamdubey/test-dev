IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_MultiOutlets]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_MultiOutlets]
GO

	
CREATE PROCEDURE [dbo].[TC_MultiOutlets]
	@Id	INT = NULL,
	@MultiOutletName VARCHAR(60)
AS
	
BEGIN
	IF(@Id IS NULL)
		BEGIN
		
			IF NOT EXISTS(SELECT TOP 1 * FROM TC_DealerAdmin WHERE Organization=@MultiOutletName )
				INSERT INTO TC_DealerAdmin(Organization)VALUES(@MultiOutletName)
		END
	ELSE
		BEGIN
			UPDATE TC_DealerAdmin SET Organization=@MultiOutletName WHERE Id=@Id
		END
END
