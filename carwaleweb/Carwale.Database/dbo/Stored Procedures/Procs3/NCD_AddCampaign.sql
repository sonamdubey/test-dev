IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_AddCampaign]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_AddCampaign]
GO

	
-- =============================================
-- Author:		Ruchira Patil
-- Create date: 15th April 2014
-- Description:	to insert and update campaigns for ncd dealers
-- =============================================
CREATE PROCEDURE [dbo].[NCD_AddCampaign] 
	@UpdateId			NUMERIC,--UpdateId is -1 for inserting new campaign else update given campaign
	@DealerId			NUMERIC,
	@TotalLeadCount		BIGINT,
	@CreatedBy			NUMERIC,
	@UpdatedBy			NUMERIC,
	@UpdatedOn			DATETIME,
	@CampaignName		VARCHAR(250),
	@IsActive			BIT,
	@PrevName			VARCHAR(250),
	@PrevLeadCount		BIGINT,
	@IsDailyBased		BIT,
	@DailyCount			BIGINT,
	@PrevDailyCount    BIGINT,
	@CampaignId			NUMERIC OUTPUT
	
AS
BEGIN
	
	SET @CampaignId = -1
    DECLARE @DailyDel BIGINT,
			@DelLeads	BIGINT
    IF @UpdateId = -1
		BEGIN	
			--Inserts a record for the NCD campaign
			INSERT INTO NCD_Campaigns (DealerId,CampaignName,TotalLeadCount,CreatedBy,UpdatedBy,UpdatedOn,IsActive,IsDailyBased,DailyCount)
			VALUES (@DealerId,@CampaignName,@TotalLeadCount,@CreatedBy,@UpdatedBy,@UpdatedOn,@IsActive,@IsDailyBased,@DailyCount) 
		    
			SET @CampaignId = SCOPE_IDENTITY()

			INSERT INTO NCD_CampaignLog (CampaignId,PreviousCampaignName,NewCampaignName,PreviousLeadCount,NewLeadCount,UpdatedOn,UpdatedBy,CreatedBy,IsActive,IsDailybased,PreviousDailycount)
			VALUES (@CampaignId,@PrevName,@CampaignName,@PrevLeadCount,@TotalLeadCount,@UpdatedOn,@UpdatedBy,@CreatedBy,@IsActive,@IsDailyBased,@DailyCount)
		    
			--Update NCS Dealer with new Campaign Id 
			IF (@IsActive = 1)
				BEGIN
					UPDATE NCD_Dealers
						SET	CampaignId = @CampaignId,
							TargetLeads = @TotalLeadCount,
							DelLeads = NULL,
							IsDailyBased = @IsDailyBased,
							DailyDel = NULL,
							DailyCount = @DailyCount
						WHERE DealerId = @DealerId
				END
		END
	ELSE
		BEGIN	
			UPDATE NCD_Campaigns
			SET TotalLeadCount    = @TotalLeadCount,
				CampaignName = @CampaignName,
				UpdatedBy    = @UpdatedBy,
				UpdatedOn    = @UpdatedOn,
				IsActive     = @IsActive,
				IsDailyBased = @IsDailyBased,
				DailyCount   = @DailyCount

			WHERE Id = @UpdateId
			
			SET @CampaignId = @UpdateId
			
			--Maintain log in log table for every update.
            
			IF @PrevName <> @CampaignName OR @PrevLeadCount <> @TotalLeadCount OR @IsDailyBased <> @IsDailyBased OR @PrevDailyCount <> @DailyCount
			
			BEGIN
				INSERT INTO NCD_CampaignLog(CampaignId,PreviousCampaignName,NewCampaignName,PreviousLeadCount,NewLeadCount,UpdatedOn,UpdatedBy,CreatedBy,IsActive,IsDailybased,PreviousDailycount)
				VALUES (@CampaignId,@PrevName,@CampaignName,@PrevLeadCount,@TotalLeadCount,@UpdatedOn,@UpdatedBy,@CreatedBy,@IsActive,@IsDailyBased,@DailyCount)
		    END
		    
		    SELECT @DelLeads = COUNT(Id) FROM NCD_Inquiries WHERE DealerId = @DealerId AND CampaignId = @UpdateId
		     --AND CreatedOn BETWEEN @StartDate AND @EndDate
			SELECT @DailyDel = COUNT(Id) FROM NCD_Inquiries WHERE DealerId = @DealerId AND CampaignId = @UpdateId AND CONVERT(DATE, EntryDate) = CONVERT(DATE, GETDATE())
		    
		    IF (@IsActive = 1)
				BEGIN
					UPDATE NCD_Dealers
						SET	CampaignId = @CampaignId,
							TargetLeads = @TotalLeadCount,
							DelLeads = NULL,
							IsDailyBased = @IsDailyBased,
							DailyDel = NULL,
							DailyCount = @DailyCount
						WHERE DealerId = @DealerId
				END
		END
END


