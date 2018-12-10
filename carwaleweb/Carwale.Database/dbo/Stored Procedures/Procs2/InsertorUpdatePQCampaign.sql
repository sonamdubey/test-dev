IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertorUpdatePQCampaign]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertorUpdatePQCampaign]
GO
	-- =============================================
-- Author:		Anchal gupta
-- Create date: 05/10/2015
-- Description:	Insert and update the PQ_DealerSponsered Table through operations 
-- Modified:    Vicky Lund, 06/10/2015, Insert and Delete in MM_SellerMaskingMobile
-- EXEC [InsertorUpdatePQCampaign] 4402,11658,'AAA Maruti','2631873215','abc',6,150,100,43,2,1,1,1,1,1,1,1234567898,2,573,NULL
-- =============================================
CREATE PROCEDURE [dbo].[InsertorUpdatePQCampaign]
	-- Add the parameters for the stored procedure here
	@CampaignId INT
	,@DealerId INT
	,@DealerName VARCHAR(200)
	,@Phone VARCHAR(50)
	,@DealerEmail VARCHAR(250)
	,@UpdatedBy INT
	,@TotalGoal INT
	,@DailyGoal INT
	,@LeadPanel TINYINT
	,@CampPriority INT
	,@EnableUserEmail BIT
	,@EnableUserSMS BIT
	,@EnableDealerEmail BIT
	,@EnableDealerSMS BIT
	,@IsActive BIT
	,@ContractId INT
	,@UserMobile VARCHAR(20)
	,@DealerTypeId INT
	,@NCDBrandId INT
	,@StartDate VARCHAR(50)
	,@EndDate VARCHAR(50)
	,@ContractBehaviour INT
	,@OldMaskingNumber VARCHAR(20) = NULL OUTPUT
	,@NewId INT = NULL OUTPUT
	,@IsDeleteOldMaskingNumber BIT = 0 OUTPUT
	,@IsInsertNewMaskingNumber BIT = 0 OUTPUT
AS
BEGIN
	DECLARE @Status INT
	DECLARE @Status1 BIT
	--DECLARE @IsDeleteOldMaskingNumber BIT = 0
	--DECLARE @IsInsertNewMaskingNumber BIT = 0
	DECLARE @LogRemarks VARCHAR(100) = ''
	DECLARE @End_Date DATETIME
	DECLARE @Start_Date DATETIME

	SET @End_Date = DATEADD(SS, 59, DATEADD(MI, 59, DATEADD(HH, 23, convert(DATETIME, @EndDate, 101)))) --added 23:59:59 for campaign to last till end of day

	IF (CONVERT(DATE, @StartDate) > CONVERT(DATE, GETDATE()))
	BEGIN
		SET @Start_Date = CONVERT(DATETIME, @StartDate, 101)
	END
	ELSE
	BEGIN
		SET @Start_Date = GETDATE()
	END

	IF @CampaignId = 0
	BEGIN
		SET NOCOUNT ON;

		INSERT INTO PQ_DealerSponsored (
			DealerId
			,DealerName
			,Phone
			,IsActive
			,DealerEmailId
			,StartDate
			,EndDate
			,UpdatedBy
			,UpdatedOn
			,IsDesktop
			,IsMobile
			,IsAndroid
			,IsIPhone
			,Type
			,TotalGoal
			,DailyGoal
			,LeadPanel
			,CampaignPriority
			,LinkText
			,EnableUserEmail
			,EnableUserSMS
			,EnableDealerEmail
			,EnableDealerSMS
			,CostPerLead
			,TotalCount
			,DailyCount
			)
		VALUES (
			@DealerId
			,@DealerName
			,@Phone
			,@IsActive
			,@DealerEmail
			,@Start_Date
			,@End_Date
			,@UpdatedBy
			,GETDATE()
			,0
			,0
			,0
			,0
			,@ContractBehaviour
			,@TotalGoal
			,@DailyGoal
			,@LeadPanel
			,@CampPriority
			,NULL
			,@EnableUserEmail
			,@EnableUserSMS
			,@EnableDealerEmail
			,@EnableDealerSMS
			,NULL
			,0
			,0
			)

		SET @NewId = SCOPE_IDENTITY()
		SET @CampaignId = @NewId

		UPDATE TC_ContractCampaignMapping
		SET CampaignId = @NewId
		WHERE ContractId = @ContractId

		SET @LogRemarks = CASE 
				WHEN @IsActive = 1
					THEN 'Record Inserted'
				ELSE 'Record Removed'
				END
		--New entry is being inserted
		--Insert new Masking number
		SET @IsDeleteOldMaskingNumber = 0
		SET @IsInsertNewMaskingNumber = 1
	END
	ELSE
	BEGIN
		--DECLARE @OldMaskingNumber VARCHAR(20)

		SELECT @OldMaskingNumber = Phone
		FROM PQ_DealerSponsored WITH (NOLOCK)
		WHERE Id = @CampaignId

		IF (@OldMaskingNumber = @Phone)
		BEGIN
			--Old and new Masking numbers are same
			--No change required
			SET @IsDeleteOldMaskingNumber = 0
			SET @IsInsertNewMaskingNumber = 0
		END
		ELSE IF (
				(
					@OldMaskingNumber = ''
					OR @OldMaskingNumber = NULL
					)
				AND (
					@Phone != ''
					AND @Phone IS NOT NULL
					)
				)
		BEGIN
			--There is no old Masking number and new Number is being added
			--Add new Masking number
			SET @IsDeleteOldMaskingNumber = 0
			SET @IsInsertNewMaskingNumber = 1
		END
		ELSE IF (
				(
					@Phone = ''
					OR @Phone = NULL
					)
				AND (
					@OldMaskingNumber != ''
					AND @OldMaskingNumber IS NOT NULL
					)
				)
		BEGIN
			--There was an old Masking number which is now deleted and no new Number is being added
			--Delete old masking number
			--Ideal scenario: This case should not occur as there is a validation for Masking number on UI
			SET @IsDeleteOldMaskingNumber = 1
			SET @IsInsertNewMaskingNumber = 0
		END
		ELSE IF (
				(
					@Phone != ''
					AND @Phone IS NOT NULL
					)
				AND (
					@OldMaskingNumber != ''
					AND @OldMaskingNumber IS NOT NULL
					)
				AND @OldMaskingNumber != @Phone
				)
		BEGIN
			--There was an old Masking number which is now deleted and a new Number is being added
			--Delete old masking number
			--Save the new Masking number
			SET @IsDeleteOldMaskingNumber = 1
			SET @IsInsertNewMaskingNumber = 1
		END

		IF (@IsDeleteOldMaskingNumber = 1)
		BEGIN
			DECLARE @MaskingMappingId VARCHAR(10)

			SELECT @MaskingMappingId = MM_SellerMobileMaskingId
			FROM MM_SellerMobileMasking WITH (NOLOCK)
			WHERE ConsumerId = @DealerId
				AND MaskingNumber = @OldMaskingNumber

			EXEC DCRM_ReleaseMaskedDealers @MaskingMappingId
				,@UpdatedBy
				,@Status1
		END

		UPDATE PQ_DealerSponsored
		SET DealerName = @DealerName
			,DealerId = @DealerId
			,Phone = @Phone
			,DealerEmailId = @DealerEmail
			,IsActive = @IsActive
			,UpdatedBy = @UpdatedBy
			,UpdatedOn = GETDATE()
			,DailyGoal = @DailyGoal
			,LeadPanel = @LeadPanel
			,EnableUserEmail = @EnableUserEmail
			,EnableUserSMS = @EnableUserSMS
			,EnableDealerEmail = @EnableDealerEmail
			,EnableDealerSMS = @EnableDealerSMS
			,CampaignPriority = @CampPriority
			,Type = @ContractBehaviour
		WHERE Id = @CampaignId

		SET @NewId = @CampaignId
		SET @LogRemarks = 'Record Updated'
	END

	IF (
			@IsInsertNewMaskingNumber = 1
			AND @Phone != '18002090230' -- "18002090230" is the default number for CarWale
			)
	BEGIN
		EXEC [dbo].[DCRM_SaveDealerMasking] - 1
			,@DealerId
			,@Phone
			,@UserMobile
			,3
			,@DealerTypeId
			,@NCDBrandId
			,@UpdatedBy
			,@CampaignId
			,@Status
	END

	INSERT INTO PQ_DealerSponsoredLog (
		PQ_DealerSponsoredId
		,DealerId
		,DealerName
		,Phone
		,IsActive
		,DealerEmailId
		,StartDate
		,EndDate
		,ActionTakenBy
		,ActionTakenOn
		,IsDesktop
		,IsMobile
		,IsAndroid
		,IsIPhone
		,Type
		,TotalGoal
		,DailyGoal
		,LeadPanel
		,CampaignPriority
		,LinkText
		,Remarks
		,EnableUserEmail
		,EnableUserSMS
		,EnableDealerEmail
		,EnableDealerSMS
		,CostPerLead
		)
	SELECT Id
		,DealerId
		,DealerName
		,Phone
		,IsActive
		,DealerEmailId
		,StartDate
		,EndDate
		,UpdatedBy
		,UpdatedOn
		,IsDesktop
		,IsMobile
		,IsAndroid
		,IsIPhone
		,Type
		,TotalGoal
		,DailyGoal
		,LeadPanel
		,CampaignPriority
		,LinkText
		,@LogRemarks
		,EnableUserEmail
		,EnableUserSMS
		,EnableDealerEmail
		,EnableDealerSMS
		,CostPerLead
	FROM PQ_DealerSponsored WITH (NOLOCK)
	WHERE Id = @CampaignId
END
