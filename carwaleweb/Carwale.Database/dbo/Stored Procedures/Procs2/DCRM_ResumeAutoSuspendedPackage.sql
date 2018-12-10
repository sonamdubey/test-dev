IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_ResumeAutoSuspendedPackage]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_ResumeAutoSuspendedPackage]
GO

	-- =============================================
-- Author:		Mihir A Chheda
-- Create date: 15-02-2016
-- Description:	Resume Package delivery if package is auto paused after payment receive
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_ResumeAutoSuspendedPackage]
	@PaymentId	INT=NULL
AS	
BEGIN
        --get list of package associated with particular transaction--

		DECLARE @TemptTable Table(GroupId Int,ContractId Int,TransactionId Int,DealerId Int,IsProccessed Bit DEFAULT 0)

		INSERT INTO @TemptTable (GroupId, ContractId,TransactionId,DealerId)

		SELECT	IPC.GroupType,RDP.DealerPackageFeatureID,DPT.TransactionId,RDP.DealerId
		FROM	DCRM_PaymentDetails(NOLOCK) DPD
		JOIN    DCRM_PaymentTransaction(NOLOCK) DPT ON DPT.TransactionId=DPD.TransactionId
		JOIN    RVN_DealerPackageFeatures(NOLOCK) RDP ON DPT.TransactionId=RDP.TransactionId
		JOIN    Packages(NOLOCK) P ON P.Id=RDP.PackageId
		JOIN    InquiryPointCategory(NOLOCK) IPC ON IPC.Id=P.InqPtCategoryId
		JOIN    DCRM_GroupType(NOLOCK) DGT ON DGT.Id=IPC.GroupType
		WHERE   DPD.ID=@PaymentId


		---Process temp table--

		DECLARE @ContractId INT
		DECLARE @GroupId INT
		DECLARE @EndDate DATETIME
		DECLARE @AutoPausedDate DATETIME
		DECLARE @RemainingDays  INT
		DECLARE @ContractBehaviour INT
		DECLARE @DealerId INT
		DECLARE @ContractStatus INT

		WHILE (SELECT Count(TransactionId) From @TemptTable WHERE IsProccessed = 0) > 0
		BEGIN
			SELECT TOP 1 @ContractId = ContractId,@GroupId=GroupId,@DealerId=DealerId 
			FROM         @TemptTable 
			WHERE        IsProccessed = 0

			--New Car Package--
			IF @GroupId=2
			BEGIN

			  --log current row befor updation in to log table
				INSERT INTO TC_ContractCampaignMappingLog (ContractId,CampaignId,DealerId,StartDate,EndDate,TotalGoal,
				TotalDelivered,ContractStatus,ContractBehaviour,CostPerLead,ContractType,ReplacementContractId,CreatedOn,AutoPauseDate)
				SELECT      TCC.ContractId,TCC.CampaignId,TCC.DealerId,TCC.StartDate,TCC.EndDate,TCC.TotalGoal,
				TCC.TotalDelivered,TCC.ContractStatus,TCC.ContractBehaviour,TCC.CostPerLead,TCC.ContractType,TCC.ReplacementContractId,GETDATE(),AutoPauseDate
				FROM        TC_ContractCampaignMapping(NOLOCK) TCC
				WHERE       TCC.ContractId=@ContractId	

			  --Get contarct behaviour--
			    SELECT  @ContractBehaviour= ContractBehaviour,@EndDate=EndDate,@AutoPausedDate=AutoPauseDate,@ContractStatus=ContractStatus
			    FROM    TC_ContractCampaignMapping(NOLOCK)
			    WHERE   ContractId=@ContractId
			     
				IF @ContractStatus=2 AND @AutoPausedDate IS NOT NULL
				BEGIN											
					 --chnage only contract status if it is lead based--
					 IF @ContractBehaviour=1
					 BEGIN
						UPDATE TC_ContractCampaignMapping 
						SET    ContractStatus = 1 ,AutoPauseDate=NULL
						WHERE  ContractId = @ContractId 
					 END
					--chnage contract status and end date if duration based--
					 ELSE
					 BEGIN				   
						--DECLARE  @DATE1 DATETIME='2016-03-05 00:00:00.000'
						--extend the end date by adding remaining days 
						SET @RemainingDays=DATEDIFF(day,CONVERT(DATE,@AutoPausedDate),CONVERT(DATE,@EndDate))
						UPDATE TC_ContractCampaignMapping 
						SET    ContractStatus = 1,EndDate=(GETDATE() + @RemainingDays),AutoPauseDate=NULL--GETDATE()
						WHERE  ContractId = @ContractId 
					  END
				   END
				ELSE
				BEGIN
					UPDATE TC_ContractCampaignMapping 
					SET    AutoPauseDate=NULL
					WHERE  ContractId = @ContractId
				END
			  END

			--Used Car Package--
			--ELSE IF @GroupId=1
			--BEGIN
			     
			--	--log current row befor updation in to log table--
			--	INSERT INTO ConsumerPackageRequestsLogs(ConsumerPkgReqId,ActualValidity,ActualInquiryPoints,ActualAmount,
			--				PaymentModeId,Chk_DD_Number,Chk_DD_Date,EntryDate,EnteredBy,EnteredById,BankName,StartDate,EndDate,ContractStatus,
			--				ContractId)				
			--	SELECT      RP.Id AS ConsumerPkgReqId,RP.ActualValidity,RP.ActualInquiryPoints,RP.ActualAmount,RP.PaymentModeId,RP.Chk_DD_Number,RP.Chk_DD_Date,RP.EntryDate,
			--				RP.EnteredBy, RP.EnteredById,RP.BankName,RP.StartDate,EndDate,ContractStatus,RP.ContractId 
			--	FROM        ConsumerPackageRequests AS RP WITH(NOLOCK) 
			--	WHERE       RP.ContractId=@ContractId
			    
			--    --Get contarct autopaused date and end date--
			--	SELECT  @EndDate=EndDate,@AutoPausedDate=AutoPauseDate,@ContractStatus=ContractStatus
			--	FROM    ConsumerPackageRequests(NOLOCK)
			--	WHERE   ContractId=@ContractId

			--	IF @ContractStatus=2 AND @AutoPausedDate IS NOT NULL
			--	BEGIN								
			--		--chnage contract status and end date--
			--		--DECLARE  @DATE2 DATETIME='2016-03-05 00:00:00.000'

			--		--extend the end date by adding remaining days
			--		SET @RemainingDays=DATEDIFF(day,CONVERT(DATE,@AutoPausedDate),CONVERT(DATE,@EndDate))
			--		UPDATE ConsumerPackageRequests 
			--		SET    ContractStatus = 1,EndDate=(GETDATE() + @RemainingDays),AutoPauseDate=NULL--GETDATE()
			--		WHERE  ContractId = @ContractId 

			--		UPDATE Sellinquiries 
			--		SET    PackageExpiryDate=(GETDATE() + @RemainingDays)
			--		WHERE  DealerId = @DealerId
			--	END
			--	ELSE
			--	BEGIN
			--		UPDATE ConsumerPackageRequests 
			--		SET    AutoPauseDate=NULL
			--		WHERE  ContractId = @ContractId 
			--	END
			--END

			UPDATE @TemptTable 
			SET    IsProccessed = 1 
			WHERE  ContractId = @ContractId 
		END		
	END
