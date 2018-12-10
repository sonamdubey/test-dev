IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_MapDealerContractsWithCampaign]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_MapDealerContractsWithCampaign]
GO

	
-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <8th Oct 15>
-- Description:	<Update ContractCampaignMapping>
-- =============================================
CREATE PROCEDURE [dbo].[TC_MapDealerContractsWithCampaign] 
	@CampaignId		INT,
	@ContractId		INT,
	@MappingId		INT,
	@LeadCnt		INT = NULL,
	@ContractBehaviour	SMALLINT, -- 1 FOR LEAD BASED 2 FOR DATE BASED
	@ResumeOption	INT, -- 1 FOR RESUME WITH EXISTING 2 FOR RESUME FRESH
	@CampaignStatus	SMALLINT, -- 1 FOR RUNNING 2 FOR PAUSED 3 FOR COMPLETE
	@UserId			INT,
	@StartDate		DATETIME,
	@EndDate		DATETIME = NULL,
	@PendingDays	INT = 0
AS
BEGIN
	
	DECLARE @MaxEndate DATETIME
	DECLARE @MinStartDate DATETIME
	
	IF @ContractBehaviour = 1 -- LEAD BASED
		BEGIN
			IF @CampaignStatus = 1 -- RUNNING
				BEGIN
					UPDATE TC_ContractCampaignMapping SET CampaignId = @CampaignId WHERE ID = @MappingId
					SELECT @MinStartDate = MIN(StartDate) FROM TC_ContractCampaignMapping WITH(NOLOCK) WHERE ContractStatus = 1 AND CampaignId = @CampaignId
					UPDATE PQ_DealerSponsored SET TotalGoal = TotalGoal + @LeadCnt, StartDate = @MinStartDate WHERE Id = @CampaignId
				END
			ELSE IF @CampaignStatus = 3 -- COMPLETED
				BEGIN
					UPDATE PQ_DealerSponsored SET TotalGoal = @LeadCnt ,TotalCount = 0,DailyCount = 0, StartDate = @StartDate WHERE ID = @CampaignId
					UPDATE TC_ContractCampaignMapping SET CampaignId = @CampaignId WHERE ID = @MappingId
				END
			ELSE IF @CampaignStatus = 2 -- PAUSED
				BEGIN
					IF(@ResumeOption = 1) -- RESUME WITH EXISTING
						BEGIN
							UPDATE TC_ContractCampaignMapping SET ContractStatus = 1 WHERE ContractStatus = 2 AND CampaignId = @CampaignId
							UPDATE TC_ContractCampaignMapping SET CampaignId = @CampaignId WHERE ID = @MappingId
							SELECT @MinStartDate = MIN(StartDate) FROM TC_ContractCampaignMapping WITH(NOLOCK) WHERE ContractStatus = 1 AND CampaignId = @CampaignId
							UPDATE PQ_DealerSponsored SET IsActive = 1 , TotalGoal = TotalGoal + @LeadCnt, DailyCount = 0, StartDate = @MinStartDate WHERE ID = @CampaignId
						END
					ELSE IF(@ResumeOption = 2) -- RESUME FRESH
						BEGIN
							-- ABORT ALL EXISTING CONTRACTS AND START FRESH ONE
							UPDATE TC_ContractCampaignMapping SET ContractStatus = 4, EndDate = GETDATE() WHERE CampaignId = @CampaignId AND ContractStatus = 2 
							UPDATE PQ_DealerSponsored SET  TotalGoal = @LeadCnt ,TotalCount = 0,DailyCount = 0, StartDate = @StartDate,IsActive = 1 WHERE ID = @CampaignId 
							UPDATE TC_ContractCampaignMapping SET CampaignId = @CampaignId WHERE ID = @MappingId
						END
					ELSE IF @ResumeOption = 0 -- NO INCOMPLETE CONTRACTS
						BEGIN
							UPDATE TC_ContractCampaignMapping SET CampaignId = @CampaignId WHERE ID = @MappingId
							UPDATE PQ_DealerSponsored SET  TotalGoal = @LeadCnt ,TotalCount = 0,DailyCount = 0, StartDate = @StartDate,IsActive = 1 WHERE ID = @CampaignId 
						END

				END
		END
	ELSE IF @ContractBehaviour = 2 -- DATE BASED
		BEGIN
			IF @CampaignStatus = 1 -- RUNNING
				BEGIN
					UPDATE TC_ContractCampaignMapping SET CampaignId = @CampaignId WHERE ID = @MappingId
					SELECT @MinStartDate = MIN(StartDate) FROM TC_ContractCampaignMapping WITH(NOLOCK) WHERE ContractStatus = 1 AND CampaignId = @CampaignId
					UPDATE PQ_DealerSponsored SET EndDate = @EndDate, StartDate = @MinStartDate WHERE Id = @CampaignId
				END
			ELSE IF @CampaignStatus = 3 -- COMPLETED
				BEGIN
					UPDATE PQ_DealerSponsored SET EndDate = @EndDate ,StartDate = @StartDate WHERE ID = @CampaignId
					UPDATE TC_ContractCampaignMapping SET CampaignId = @CampaignId WHERE ID = @MappingId

				END 
			ELSE IF @CampaignStatus = 2 -- PAUSED
				BEGIN
					IF @ResumeOption = 1 -- RESUME WITH EXISTING AND ADD PENDING DAYS
						BEGIN
														
							UPDATE TC_ContractCampaignMapping SET ContractStatus = 4  WHERE ContractStatus = 2 AND CampaignId = @CampaignId
							UPDATE TC_ContractCampaignMapping SET CampaignId = @CampaignId, EndDate = EndDate + @PendingDays  WHERE ID = @MappingId
							
							SELECT @MaxEndate = MAX(EndDate) FROM TC_ContractCampaignMapping WITH(NOLOCK) WHERE ContractStatus = 1 AND CampaignId = @CampaignId
							UPDATE PQ_DealerSponsored SET IsActive = 1, StartDate = @StartDate, EndDate = @MaxEndate, TotalCount = 0, DailyCount = 0 WHERE ID = @CampaignId    
							
						END
					ELSE IF @ResumeOption = 2 -- TAKE ACTIVE FRESH
						BEGIN
							UPDATE PQ_DealerSponsored SET IsActive = 1, StartDate = @StartDate, EndDate = @EndDate, TotalCount = 0, DailyCount = 0  WHERE Id = @CampaignId  
							-- ABORT ALL EXISTING CONTRACTS AND START FRESH ONE
							UPDATE TC_ContractCampaignMapping SET ContractStatus = 4 WHERE CampaignId = @CampaignId AND ContractStatus = 2	
							UPDATE TC_ContractCampaignMapping SET CampaignId = @CampaignId WHERE ID = @MappingId					
							
						END
					ELSE IF @ResumeOption = 0 -- NO INCOMPLETE CONTRACTS
						BEGIN
							UPDATE TC_ContractCampaignMapping SET CampaignId = @CampaignId WHERE ID = @MappingId
							UPDATE PQ_DealerSponsored SET IsActive = 1, StartDate = @StartDate, EndDate = @EndDate, TotalCount = 0, DailyCount = 0 WHERE ID = @CampaignId
						END
				END
		END
END

