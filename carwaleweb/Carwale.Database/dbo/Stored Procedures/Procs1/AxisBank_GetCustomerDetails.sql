IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AxisBank_GetCustomerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AxisBank_GetCustomerDetails]
GO

	-- Written By : Akansha 12.12.2013
-- Summary : Proc will return all the User details which are necessory for the given userid.
CREATE PROCEDURE [dbo].[AxisBank_GetCustomerDetails]
	@UserId		NUMERIC(18,0),
	@FirstName			VARCHAR(50)	OUTPUT, 
	@LastName			VARCHAR(50)	OUTPUT, 
	@Email			VARCHAR(100)	OUTPUT,
	@PasswordSalt		VARCHAR(10)	OUTPUT,
	@PasswordHash		VARCHAR(64)	OUTPUT,
	@CreatedOn	DATETIME 	OUTPUT,  
	@IsVerified		BIT 		OUTPUT,  
	@IsAdmin		BIT 		OUTPUT,
	@IsActive		BIT 		OUTPUT,
	@IsExist		BIT 		OUTPUT,
	@PasswordExpiry		datetime output
AS	
BEGIN			
	SELECT 
		@FirstName	 		= FirstName, 
		@LastName	 		= LastName, 
		@Email			= Email, 		
		@PasswordSalt   = PasswordSalt,
		@PasswordHash   = PasswordHash,
		@CreatedOn		= CreatedOn,
		@IsVerified		= IsVerified,
		@IsAdmin		= IsAdmin,
		@IsActive		= IsActive,
		@PasswordExpiry = PasswordExpiry
	FROM 
		AxisBank_Users with(nolock)
	WHERE 
		UserId = @UserId
		
	IF @@ROWCOUNT > 0   
		SET @IsExist = 1

		
END
