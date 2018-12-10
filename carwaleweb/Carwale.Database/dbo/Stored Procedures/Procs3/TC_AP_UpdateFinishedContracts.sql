IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_AP_UpdateFinishedContracts]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_AP_UpdateFinishedContracts]
GO

	-- Created By:    Deepak
-- Create date: 22-09-2015
-- Description:	Updates TotalDelivered against a campaign if lead is not duplicate
--========================================================================
CREATE PROCEDURE [dbo].[TC_AP_UpdateFinishedContracts] 

AS
BEGIN
  SET NOCOUNT ON;
    
	DECLARE @NumberRecords AS INT
	DECLARE @RowCount AS INT
    DECLARE @TempCampData Table(RowID INT IDENTITY(1, 1), ContractId NUMERIC, CampaignId NUMERIC)
    DECLARE @NewContractId INT = NULL
    
	--Get contract data where contracts are active and start date is more than todays date.
	INSERT INTO @TempCampData
    SELECT TC.ContractId, TC.CampaignId
    FROM TC_ContractCampaignMapping TC WITH (NOLOCK)
    WHERE ContractStatus = 1 AND CONVERT(DATE,TC.EndDate) = CONVERT(DATE,GETDATE()-1)
	
	SET @NumberRecords = @@ROWCOUNT
	SET @RowCount = 1
		
	--Close the contracts where end date has arrived
	UPDATE TC_ContractCampaignMapping SET ContractStatus = 3, IsActiveContract = 0 WHERE ContractId IN(SELECT ContractId FROM @TempCampData)
	
	--Update the new contract start date
	DECLARE @NewContractStartDate AS DATETIME
	DECLARE @CampaignId AS INT
	WHILE @RowCount <= @NumberRecords
		BEGIN
			SET @NewContractStartDate = NULL
			SET @CampaignId = NULL
			
			SELECT @CampaignId = CampaignId FROM @TempCampData WHERE RowID = @RowCount
			
			SELECT TOP 1 @NewContractStartDate = TC.StartDate, @NewContractId = TC.Id FROM TC_ContractCampaignMapping TC WITH (NOLOCK)
			WHERE CampaignId = @CampaignId AND ContractStatus = 1
			ORDER BY TC.StartDate ASC
			
			IF @@ROWCOUNT > 0
				BEGIN
					UPDATE PQ_DealerSponsored SET StartDate = @NewContractStartDate WHERE Id = @CampaignId
					
					--Tag it as active contract
					UPDATE TC_ContractCampaignMapping SET IsActiveContract = 1 WHERE ID = @NewContractId
				END
			
			SET @RowCount = @RowCount + 1
		END
END