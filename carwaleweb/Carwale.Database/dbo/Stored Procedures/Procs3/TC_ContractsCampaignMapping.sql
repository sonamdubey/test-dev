IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ContractsCampaignMapping]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ContractsCampaignMapping]
GO

	-- Created By:    Afrose
-- Create date: 22-09-2015
-- Description:	Updates TotalDelivered against a campaign if lead is not duplicate
-- Modified By : Sunil Yadav On 08th feb 2016 
-- Description : Call UpdateTC_ContractStatus sp to update ContractStatus and ContractEndDate
--========================================================================
CREATE PROCEDURE [dbo].[TC_ContractsCampaignMapping] --4670, 6724, 4739061,1092622 
(@CampaignId INT 
, @DealerId BIGINT 
, @INQLeadIdOutput BIGINT
, @TC_NewCarInquiryId INT )

AS
BEGIN
  SET NOCOUNT ON;

    DECLARE @TempContractId INT
    DECLARE @ContractBehaviour SMALLINT
    DECLARE @NewContractStartDate DateTime = NULL
    DECLARE @NewContractId INT = NULL
	DECLARE @TotalGoal INT = NULL
    
	--Get contract data where contracts are active and start date has passed and end date has not yet crossed.
    SELECT TOP 1 @TempContractId = ContractId, @ContractBehaviour = ContractBehaviour ,@TotalGoal = TotalGoal
    FROM TC_ContractCampaignMapping TC WITH (NOLOCK)
    WHERE DealerId = @DealerId AND CampaignId = @CampaignId AND ContractStatus = 1
		AND CONVERT(DATE,TC.StartDate) <= CONVERT(DATE,GETDATE())
		AND CONVERT(DATE,ISNULL(TC.EndDate,GETDATE())) >= CONVERT(DATE,GETDATE())
	ORDER BY TC.StartDate, TC.Id ASC
	
	IF @@ROWCOUNT > 0
		BEGIN
			--Update contractId in inqurylead table
			UPDATE TC_InquiriesLead SET ContractId = @TempContractId,CampaignId=@CampaignId WHERE TC_InquiriesLeadId = @INQLeadIdOutput
			
			--Update contractId against buyerinquiry
			UPDATE TC_NewCarInquiries SET ContractId = @TempContractId WHERE TC_NewCarInquiriesId = @TC_NewCarInquiryId
			
			--Update Contract counting details
			IF @ContractBehaviour = 1 --Lead Based
				BEGIN
					--Update Counting and data
					UPDATE TC_ContractCampaignMapping
					SET TotalDelivered = ISNULL(TotalDelivered,0) + 1
					--,EndDate = CASE WHEN (ISNULL(TotalDelivered,0) + 1) = TotalGoal THEN GETDATE() ELSE EndDate END, -- Commented By Sunil Yadav for MultiOutlet and group.
					--   ContractStatus = CASE WHEN (ISNULL(TotalDelivered,0) + 1) = TotalGoal THEN 3 ELSE ContractStatus END -- 1-Active, 2-Paused, 3-Closed, 4- Aborted
					WHERE DealerId = @DealerId
						AND CampaignId = @CampaignId
						AND ContractId = @TempContractId

						EXEC UpdateTC_ContractStatus @TempContractId,@CampaignId,@TotalGoal -- to update contract status and contractEndDate
						
					--If this contract is over, Update the start date of new contract date against campaign.
					SELECT Id FROM TC_ContractCampaignMapping WITH (NOLOCK) WHERE ContractId = @TempContractId AND ContractStatus = 3
					IF @@ROWCOUNT > 0 -- This Contract is over now
						BEGIN
							--Remove flag IsActiveContract
							UPDATE TC_ContractCampaignMapping SET IsActiveContract = 0 WHERE ContractId = @TempContractId AND ContractStatus = 3
							
							--Select New contract Id and start date.
							SELECT TOP 1 @NewContractStartDate = TC.StartDate, @NewContractId = TC.Id FROM TC_ContractCampaignMapping TC WITH (NOLOCK)
							WHERE DealerId = @DealerId AND CampaignId = @CampaignId AND ContractStatus = 1
							ORDER BY TC.StartDate ASC 
							
							--Activate the next queued contract
							UPDATE TC_ContractCampaignMapping SET IsActiveContract = 1 WHERE ID = @NewContractId
						END
						
					--Log Data
					INSERT INTO TC_ContractCampaignDataLog(TC_InquiryLeadId, TC_NewCarInquiryId, CampaignId, ContractId, Status) 
					VALUES(@INQLeadIdOutput, @TC_NewCarInquiryId, @CampaignId, @TempContractId, 'Contract Updated')
			
				END
			ELSE IF @ContractBehaviour = 2 --Date Based
				BEGIN
					UPDATE TC_ContractCampaignMapping
					SET TotalDelivered = (ISNULL(TotalDelivered,0) + 1)
					WHERE DealerId = @DealerId
						AND CampaignId = @CampaignId
						AND ContractId = @TempContractId
						
					--To Update Datebased completed contracts we call an automated SP at midnight. TC_AP_UpdateFinishedContracts
					
					--Log Data
					INSERT INTO TC_ContractCampaignDataLog(TC_InquiryLeadId, TC_NewCarInquiryId, CampaignId, ContractId, Status) 
					VALUES(@INQLeadIdOutput, @TC_NewCarInquiryId, @CampaignId, @TempContractId, 'Contract Updated')
				END
				
			--Update data against campaign Id in CarWale database and also change the start date if the current campaign is over and new campaign is going to start
			UPDATE PQ_DealerSponsored SET TotalCount = (ISNULL(TotalCount, 0) + 1), DailyCount = (ISNULL(DailyCount,0) + 1),
				StartDate = CASE ISNULL(@NewContractStartDate,'') WHEN '' THEN StartDate ELSE @NewContractStartDate END
			WHERE Id = @CampaignId
		END
	ELSE
		BEGIN
			--Log Data
			INSERT INTO TC_ContractCampaignDataLog(TC_InquiryLeadId, TC_NewCarInquiryId, CampaignId, Status) 
			VALUES(@INQLeadIdOutput, @TC_NewCarInquiryId, @CampaignId, 'No Contract Exist')
		END
END


--------------------------------------------------------------------------------------------------------------------------------------------------------


-------------------------------------------------Vaibhav K get user manage details-------------------------------------------------------
/****** Object:  StoredProcedure [dbo].[DCRM_GetUserManagerDetails]    Script Date: 2/16/2016 4:37:12 PM ******/
SET ANSI_NULLS ON
