IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetUserDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetUserDetails]
GO

	-- =============================================
-- Author:		Binumon George
-- Create date: 07-03-2012
-- Description:	User Getin user details on bsis of user Id
-- [dbo].[TC_UsersDelete] 1,5,@Status
-- Modified By: Tejashree Patil on 5 Sept 2012 on 1 pm
-- Description: Removed condition from SELECT clause which returns ISNULL(@Dob , GETDATE()), used @Dob instead
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetUserDetails]
	-- Add the parameters for the stored procedure here
	@Id INT,
	@BranchId INT,
	@Status INT OUTPUT,
	@UserName VARCHAR(50) OUTPUT,
	@RoleId VARCHAR(5) OUTPUT,
	@Email VARCHAR(100) OUTPUT,
	@Mobile VARCHAR(15) OUTPUT,
	@DOB  VARCHAR(20) OUTPUT,
	@DOJ VARCHAR(20) OUTPUT,
	@Sex VARCHAR(6) OUTPUT,
	@Address VARCHAR(200) OUTPUT,
	@Password VARCHAR(20) OUTPUT,
	@IsSuperAdmin BIT OUTPUT,
	@Role INT
	--@ModifiedBy INT
AS
BEGIN
		-- checking here basic super admin or not
		SET @IsSuperAdmin=0
		IF(@Role IS NOT NULL)
		BEGIN
			EXEC TC_IsSuperAdmin @Id,@BranchId, @IsSuperAdmin OUTPUT 
		END
		
		-- Modified By: Tejashree Patil on 5 Sept 2012 on 1 pm
		
		SELECT @UserName = U.UserName,@RoleId=u.RoleId,  @Email = U.Email,@Mobile = U.Mobile, 
		@DOB = U.DOB , @DOJ = U.DOJ, @Sex = U.Sex, @Address = U.Address,
		@Password = U.Password 
		FROM dbo.TC_Users U WITH(NOLOCK) WHERE IsActive=1 and Id=@Id
		SET @Status=1
		
END