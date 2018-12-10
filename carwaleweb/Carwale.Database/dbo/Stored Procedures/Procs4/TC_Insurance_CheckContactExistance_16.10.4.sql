IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Insurance_CheckContactExistance_16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Insurance_CheckContactExistance_16]
GO

	-- =============================================
-- Author		:	Tejashree Patil
-- Date			:	16 Sept 2016
-- Modified By	:	Check existance of contact before add
-- DECLARE @InsuranceReminderId int exec [TC_Insurance_CheckContactExistance] NULL ,'4242424242', @InsuranceReminderId output SELECT @InsuranceReminderId
-- Modified by : kartik rathod on 13 oct 2016,added @ChassisNo
-- =============================================
CREATE PROCEDURE [dbo].[TC_Insurance_CheckContactExistance_16.10.4]
	@RegistrationNumber VARCHAR(20) = NULL
	,@MobileNumber VARCHAR(10) = NULL
	,@BranchId	INT
	,@InsuranceReminderId INT = NULL OUTPUT
	,@ChassisNo VARCHAR(20) = NULL
AS
BEGIN
	-- save data in reminder 
	DECLARE @RegistrationNumberSearch VARCHAR(50)
	SET @RegistrationNumberSearch = LOWER(REPLACE(@RegistrationNumber,' ' ,''))

	SET  @InsuranceReminderId =(
	SELECT	TOP 1 TC_Insurance_ReminderId 
	FROM	TC_Insurance_Reminder IR WITH(NOLOCK) 
	WHERE	IR.BranchId = @BranchId AND (RegistrationNumberSearch = @RegistrationNumberSearch 
			OR MobileNumber = @MobileNumber OR ChassisNumber = @ChassisNo)
	--UNION ALL
	--SELECT	TOP 1 TC_Insurance_InquiriesId
	--FROM	TC_Insurance_Inquiries II WITH(NOLOCK) 
	--WHERE	LOWER(REPLACE(RegistrationNumber,' ' ,'')) = @RegistrationNumberSearch
	)
	
END