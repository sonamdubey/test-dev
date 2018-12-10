IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCS_AddNCDCampaign]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCS_AddNCDCampaign]
GO

	-- =============================================
-- Author      : Chetan Navin
-- Create date : 4 April 2013
-- Description : Adds a NCD campaign for the dealer & keep a log of it in NCS_LeadCampaignLog
--				 Also If UpdateId = -1 then add new campaign ,update Campaign on given Id,Update Delivered leads in NCS_Dealers
-- Modifier    : Ruchira Patil (19 Nov 2013) -  updates the delivered leads in NCS_Dealers based on the dailybased or leadbased or datebased
-- =============================================
CREATE PROCEDURE [dbo].[NCS_AddNCDCampaign]
	-- Add the parameters for the stored procedure here
	@UpdateId			NUMERIC,--UpdateId is -1 for inserting new campaign else update given campaign
	@DealerId			NUMERIC,
	@StartDate			DATETIME,
	@EndDate			DATETIME,
	@TotalLeadCount		BIGINT,
	@CreatedBy			NUMERIC,
	@UpdatedBy			NUMERIC,
	@UpdatedOn			DATETIME,
	@CampaignName		VARCHAR(250),
	@IsActive			BIT,
	@PrevName			VARCHAR(250),
	@PrevLeadCount		BIGINT,
	@PrevStartDate		DATETIME,
	@PrevEndDate		DATETIME,
	@CampaignType		TINYINT,
	@IsAreaBased		BIT, 
	@IsDailyBased		BIT,
	@DailyCount			BIGINT,
	@PrevDailyCount    BIGINT,
	@CampaignId			NUMERIC OUTPUT
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SET @CampaignId = -1
    DECLARE @DailyDel BIGINT,
			@DelLeads	BIGINT
    -- Insert statements for procedure here
    IF @UpdateId = -1
		BEGIN	
			--Inserts a record for the NCD campaign
			INSERT INTO CRM_NCDCampaign (DealerId,CampaignType,StartDate,EndDate,TotalLeadCount,CreatedBy,UpdatedBy,UpdatedOn,CampaignName,IsActive,IsAreaBased,IsDailyBased,DailyCount)
			VALUES (@DealerId,@CampaignType,@StartDate,@EndDate,@TotalLeadCount,@CreatedBy,@UpdatedBy,@UpdatedOn,@CampaignName,@IsActive,@IsAreaBased,@IsDailyBased,@DailyCount) 
		    
			SET @CampaignId = SCOPE_IDENTITY()

			INSERT INTO CRM_NCDCampaignLog (CampaignId,CampaignType,PreviousCampaignName,NewCampaignName,PreviousLeadCount,NewLeadCount,PreviousStartDate,NewStartDate,PreviousEndDate,NewEndDate,UpdatedOn,UpdatedBy,CreatedBy,IsActive,IsAreaBased,IsDailybased,PreviousDailycount)
			VALUES (@CampaignId,@CampaignType,@PrevName,@CampaignName,@PrevLeadCount,@TotalLeadCount,@PrevStartDate,@StartDate,@PrevEndDate,@EndDate,@UpdatedOn,@UpdatedBy,@CreatedBy,@IsActive,@IsAreaBased,@IsDailyBased,@DailyCount)
		    
			--Update NCS Dealer with new Campaign Id 
			IF (@IsActive = 1)
				BEGIN
				--CampaignType  = 1 for DateBased AND 2 FOR LeadBased
					IF(@CampaignType = 1)            
						BEGIN
							UPDATE NCS_Dealers
							SET	CampaignId = @CampaignId,
								TargetLeads = NULL,
								DelLeads = NULL,
								EndDate = @EndDate,
								IsDailyBased = @IsDailyBased,
								DailyDel = NULL,
								DailyCount = @DailyCount
							WHERE ID = @DealerId
						END
					ELSE IF (@CampaignType = 2)
						BEGIN
							UPDATE NCS_Dealers
							SET	CampaignId = @CampaignId,
								TargetLeads = @TotalLeadCount,
								DelLeads = NULL,
								EndDate = NULL,
								IsDailyBased = @IsDailyBased,
								DailyDel = NULL,
								DailyCount = @DailyCount
							WHERE ID = @DealerId
						END
				END
		END
	ELSE
		BEGIN	
			UPDATE CRM_NCDCampaign
			SET TotalLeadCount    = @TotalLeadCount,
				StartDate    = @StartDate,
				EndDate      = @EndDate,
				UpdatedBy    = @UpdatedBy,
				UpdatedOn    = @UpdatedOn,
				IsActive     = @IsActive,
				CampaignType = @CampaignType,
				IsAreaBased  = @IsAreaBased,
				IsDailyBased = @IsDailyBased,
				DailyCount   = @DailyCount

			WHERE Id = @UpdateId
			
			SET @CampaignId = @UpdateId
			
			--Maintain log in log table for every update.
            
			IF @PrevName <> @CampaignName OR @PrevLeadCount <> @TotalLeadCount OR @PrevStartDate <> @StartDate OR @PrevEndDate <> @EndDate OR @IsDailyBased <> @IsDailyBased OR @PrevDailyCount <> @DailyCount
			
			BEGIN
				INSERT INTO CRM_NCDCampaignLog (CampaignId,CampaignType,PreviousCampaignName,NewCampaignName,PreviousLeadCount,NewLeadCount,PreviousStartDate,NewStartDate,
				PreviousEndDate,NewEndDate,UpdatedOn,UpdatedBy,IsActive,IsAreaBased,CreatedBy,IsDailyBased,PreviousDailyCount)
				VALUES (@CampaignId,@CampaignType,@PrevName,@CampaignName,@PrevLeadCount,@TotalLeadCount,@PrevStartDate,@StartDate,@PrevEndDate,@EndDate,@UpdatedOn,@UpdatedBy,@IsActive,@IsAreaBased,@CreatedBy,@IsDailyBased,@DailyCount)
		    END
		    
		    SELECT @DelLeads = COUNT(Id) FROM CRM_CarDealerAssignment WHERE DealerId = @DealerId AND CampaignId = @UpdateId
		     --AND CreatedOn BETWEEN @StartDate AND @EndDate
			SELECT @DailyDel = COUNT(Id) FROM CRM_CarDealerAssignment WHERE DealerId = @DealerId AND CampaignId = @UpdateId AND CONVERT(DATE, CreatedOn) = CONVERT(DATE, GETDATE())
		    
		    IF (@IsActive = 1)
				BEGIN
					--CampaignType  = 1 for DateBased AND 2 FOR LeadBased
					IF(@CampaignType = 1)            
						BEGIN
							UPDATE NCS_Dealers
							SET	CampaignId = @CampaignId,
								TargetLeads = NULL,
								DelLeads = @DelLeads,
								EndDate = @EndDate,
								IsDailyBased = @IsDailyBased,
								DailyDel = @DailyDel,
								DailyCount = @DailyCount
							WHERE ID = @DealerId
						END
					ELSE IF (@CampaignType = 2)
						BEGIN
							UPDATE NCS_Dealers
							SET	CampaignId = @CampaignId,
								TargetLeads = @TotalLeadCount,
								DelLeads = @DelLeads,
								EndDate = NULL,
								IsDailyBased = @IsDailyBased,
								DailyDel = @DailyDel,
								DailyCount = @DailyCount
							WHERE ID = @DealerId
						END
				END
		END
END