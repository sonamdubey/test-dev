IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UpdateDealerEmail]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UpdateDealerEmail]
GO

	-- Created by: Binu
-- Create date: 31 May 2012
-- Description:	Updating dealer super admin email from opr.
-- =============================================
CREATE PROCEDURE [dbo].[TC_UpdateDealerEmail] 
	@BranchId NUMERIC,
	@Email VARCHAR(100),
	@Id NUMERIC,
	@Status BIT OUTPUT
AS
BEGIN
	SET @Status=0
	IF NOT EXISTS(SELECT TOP 1 Id FROM TC_Users WHERE Email=@Email AND BranchId=@BranchId AND Id<>@Id)
		BEGIN
			UPDATE TC_Users SET Email=@Email WHERE Id=@Id AND BranchId=@BranchId
			SET @Status=1
		END
	PRINT @Status
END
