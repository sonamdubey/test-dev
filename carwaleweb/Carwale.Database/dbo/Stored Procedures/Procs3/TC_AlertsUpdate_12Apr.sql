IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_AlertsUpdate_12Apr]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_AlertsUpdate_12Apr]
GO

	

-- =============================================
-- Author:		Surendra Chouksey
-- Create date: 5th Dec,2011
-- Description:	This Procedure will set permission to access Trading car from DDCRM
-- =============================================
CREATE PROCEDURE [dbo].[TC_AlertsUpdate_12Apr]
(
@BranchId NUMERIC,
@Status BIT
)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	--SET NOCOUNT ON;
	DECLARE @UserId VARCHAR(50),@Password VARCHAR(50),@TaskSet VARCHAR(100)
	SET @UserId=CONVERT(VARCHAR,@BranchId)+'CW@CarWale.com'
	SET @Password='Default'
	SET @TaskSet='2,3,4,7,9,10,17,20'
		
	UPDATE TC_Alerts SET Status=@Status,ResponseDatetime=GETDATE()
	WHERE  BranchId=@BranchId AND Status IS NULL
	
	IF(@Status=1)
	BEGIN
		IF EXISTS(SELECT Id FROM TC_Users WHERE Email=@UserId AND Password=@Password and BranchId=@BranchId)
			BEGIN				
				UPDATE TC_Users SET IsActive=1 WHERE Email=@UserId AND
				Password=@Password and BranchId=@BranchId
			END
			ELSE
			BEGIN
				-- Creating Role with limited task list to perform	
				BEGIN TRY
					BEGIN TRAN
						INSERT INTO TC_Roles(RoleName,BranchId,TaskSet,RoleCreationDate)
						VALUES('CWAccess',@BranchId,@TaskSet,GETDATE())					
							
						INSERT INTO TC_Users(UserName,Email,Password,BranchId,RoleId)
						VALUES('CarWale',@UserId,@Password,@BranchId,SCOPE_IDENTITY())
					COMMIT TRAN 
				END TRY
				-- The previous GO breaks the script into two batches,
				-- generating syntax errors. The script runs if this GO
				-- is removed.
				BEGIN CATCH
					ROLLBACK TRAN
					SELECT ERROR_NUMBER() AS ErrorNumber;
				END CATCH;		
			END
	END
	ELSE
	BEGIN
		UPDATE TC_Users SET IsActive=0 WHERE Email=@UserId AND
		Password=@Password and BranchId=@BranchId
	END	
END

