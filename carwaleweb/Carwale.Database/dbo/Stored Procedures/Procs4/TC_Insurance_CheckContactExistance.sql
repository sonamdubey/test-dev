IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Insurance_CheckContactExistance]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Insurance_CheckContactExistance]
GO

	-- =============================================
-- Author        :    Tejashree Patil
-- Date            :    16 Sept 2016
-- Modified By    :    Check existance of contact before add
-- DECLARE @InsuranceReminderId int exec [TC_Insurance_CheckContactExistance] NULL ,'4242424242', @InsuranceReminderId output SELECT @InsuranceReminderId
-- =============================================
CREATE PROCEDURE [dbo].[TC_Insurance_CheckContactExistance] @RegistrationNumber VARCHAR(20) = NULL
	,@MobileNumber VARCHAR(10) = NULL
	,@BranchId	INT
	,@InsuranceReminderId INT = NULL OUTPUT
AS
BEGIN
	-- save data in reminder 
	DECLARE @RegistrationNumberSearch VARCHAR(50)

	SET @RegistrationNumberSearch = LOWER(REPLACE(@RegistrationNumber, ' ', ''))
	SET @InsuranceReminderId = (
			SELECT TOP 1 TC_Insurance_ReminderId
			FROM TC_Insurance_Reminder IR WITH (NOLOCK)
			WHERE IR.BranchId = @BranchId AND (RegistrationNumberSearch = @RegistrationNumberSearch OR MobileNumber = @MobileNumber)
		)
END
