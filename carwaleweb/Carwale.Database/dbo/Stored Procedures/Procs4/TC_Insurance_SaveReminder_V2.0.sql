IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Insurance_SaveReminder_V2]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Insurance_SaveReminder_V2]
GO

	

-- =============================================
-- Author		:	Tejashree Patil
-- Date			:	25 Aug 2016
-- Modified By	:	Save Insurance inquiry reminder
-- Modified By : Ashwini Dhamankar on Aug 26,2016 (Added @ModifiedBy)
-- Modified By : Ashwini Dhamankar on Sep 9,2016(Added more parameters)
-- Modified By : Tejashree Patil on 26 Oct 2016(Added @AssigneeId parameter)
-- =============================================
CREATE PROCEDURE [dbo].[TC_Insurance_SaveReminder_V2.0]
	 @BranchId INT
	,@RegistrationNumber VARCHAR(20) = NULL
	,@VersionId	INT
	,@InsuranceProvider VARCHAR(50)= NULL
	,@PolicyNumber VARCHAR(50) = NULL
	,@ExpiryDate DATETIME = NULL
	,@IsClaimsExist BIT = 0
	,@ModifiedBy INT
	,@MfgYear VARCHAR(5) = NULL
	,@EngineNumber VARCHAR(20) = NULL
	,@ChassisNumber VARCHAR(20) = NULL
	,@HypothecationId TINYINT = NULL
	,@PolicyPeriodFrom DATETIME = NULL
	,@MappingInqId INT = NULL
	,@LastIDV INT = NULL
	,@LastNCB FLOAT = NULL
	,@LastDiscount FLOAT = NULL
	,@LastPremium INT = NULL
	,@CustomerName VARCHAR(100) = NULL
	,@MobileNumber VARCHAR(10) = NULL
	,@CarName VARCHAR(100) = NULL
	,@CustomerId INT = NULL
	,@RegisteredAddress VARCHAR(100) = NULL
	,@AlternateMobileNumber  VARCHAR(10) = NULL
	,@InsuranceReminderId INT = NULL OUTPUT
	,@AssignedTo INT = NULL
AS
BEGIN
	
	-- save data in reminder 
	DECLARE @RegistrationNumberSearch VARCHAR(50)
	SET @RegistrationNumberSearch = LOWER(REPLACE(@RegistrationNumber,' ' ,''))

	SELECT	@InsuranceReminderId = TC_Insurance_ReminderId 
	FROM	TC_Insurance_Reminder WITH(NOLOCK) 
	WHERE	RegistrationNumberSearch = @RegistrationNumberSearch

	IF (ISNULL(@InsuranceReminderId,0) = 0)
		BEGIN
			-- SAVE IN TC_Insurance_Inquiries
			INSERT INTO TC_Insurance_Reminder(RegistrationNumber,VersionId,EntryDate,InsuranceProvider,PolicyNumber,ExpiryDate,IsClaimsExist,BranchId
			,ModifiedBy,ModifiedDate,MfgYear,EngineNumber,ChassisNumber,HypothecationId,PolicyPeriodFrom,LastIDV,LastNCB,LastDiscount,LastPremium
			,CustomerName,MobileNumber,CarName,RegistrationNumberSearch,RegisteredAddress,AlternateMobileNumber,MappingInqId,CustomerId, AssignedTo)
			VALUES(	@RegistrationNumber, @VersionId, GETDATE(), @InsuranceProvider, @PolicyNumber, @ExpiryDate, @IsClaimsExist, @BranchId
			,@ModifiedBy,GETDATE(),@MfgYear,@EngineNumber,@ChassisNumber,@HypothecationId,@PolicyPeriodFrom,@LastIDV,@LastNCB,@LastDiscount,@LastPremium
			,@CustomerName,@MobileNumber,@CarName,@RegistrationNumberSearch,@RegisteredAddress,@AlternateMobileNumber,-1,@CustomerId, @AssignedTo)
			
			SET @InsuranceReminderId = SCOPE_IDENTITY();
		END
	ELSE
		BEGIN
			-- if first service is done or updating service update data in reminder table
			UPDATE	TC_Insurance_Reminder 
			SET		RegistrationNumber  = ISNULL(@RegistrationNumber,RegistrationNumber)
					,VersionId			= ISNULL(@VersionId,VersionId)
					,ModifiedDate		= GETDATE()
					,ModifiedBy			= ISNULL(@ModifiedBy,ModifiedBy)
					,InsuranceProvider	= ISNULL(@InsuranceProvider,InsuranceProvider)
					,PolicyNumber		= ISNULL(@PolicyNumber,PolicyNumber)
					,ExpiryDate			= ISNULL(@ExpiryDate,ExpiryDate)
					,IsClaimsExist		= ISNULL(@IsClaimsExist,IsClaimsExist)
					,LastIDV			= ISNULL(@LastIDV,LastIDV)
					,LastNCB			= ISNULL(@LastNCB,LastNCB)
					,LastDiscount		= ISNULL(@LastDiscount,LastDiscount)
					,LastPremium		= ISNULL(@LastPremium,LastPremium)
					,MappingInqId		= ISNULL(@MappingInqId,MappingInqId)
					,CustomerName		= ISNULL(@CustomerName,CustomerName)
					,MobileNumber		= ISNULL(@MobileNumber,MobileNumber)
					,CarName			= ISNULL(@CarName,CarName)
					,CustomerId			= ISNULL(@CustomerId,CustomerId)
					,RegisteredAddress  = ISNULL(@RegisteredAddress,RegisteredAddress)
					,AlternateMobileNumber = ISNULL (@AlternateMobileNumber,AlternateMobileNumber)
			WHERE	RegistrationNumberSearch = @RegistrationNumberSearch

			--SELECT @InsuranceReminderId = TC_Insurance_ReminderId FROM TC_Insurance_Reminder WITH(NOLOCK) WHERE RegistrationNumberSearch = @RegistrationNumberSearch
		END
END
--------------------------------------------
