IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_DeletePQdealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_DeletePQdealers]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 16th Oct 2014
-- Description:	To deactivate the PQ Dealers/campaigns and maintain a log of it
-- Added Conditional Remarks(vinayak)
-- Modified By: Shalini Nair on 17/02/2016 to rename 'Type' column of PQ_DealerSponsored to 'CampaignBehaviour'
-- Modified: Vicky Lund, 01/04/2016, Used applicationId column of TC_ContractCampaignMapping
-- =============================================
CREATE PROCEDURE [dbo].[Con_DeletePQdealers] @Ids VARCHAR(500)
	,@DeletedBy INT
	,@IsActive BIT
AS
BEGIN
	UPDATE PQ_DealerSponsored
	SET IsActive = @IsActive
		,PausedDate = CASE 
			WHEN @IsActive = 0
				THEN GETDATE()
			ELSE PausedDate
			END
		,UpdatedBy = @DeletedBy
		,UpdatedOn = GETDATE()
	WHERE Id IN (
			SELECT ListMember
			FROM fnSplitCSV(@Ids)
			)

	IF (@IsActive = 1)
	BEGIN
		UPDATE TC_ContractCampaignMapping
		SET ContractStatus = 1
		WHERE CampaignId IN (
				SELECT ListMember
				FROM fnSplitCSV(@Ids)
				)
			AND ContractStatus = 2
			AND ApplicationId = 1
	END
	ELSE IF (@IsActive = 0)
	BEGIN
		UPDATE TC_ContractCampaignMapping
		SET ContractStatus = 2
		WHERE CampaignId IN (
				SELECT ListMember
				FROM fnSplitCSV(@Ids)
				)
			AND ContractStatus = 1
			AND ApplicationId = 1
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
		,CampaignBehaviour
		,TotalGoal
		,DailyGoal
		,LeadPanel
		,EnableUserEmail
		,EnableUserSMS
		,EnableDealerEmail
		,EnableDealerSMS
		,CampaignPriority
		,Remarks
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
		,CampaignBehaviour
		,TotalGoal
		,DailyGoal
		,LeadPanel
		,EnableUserEmail
		,EnableUserSMS
		,EnableDealerEmail
		,EnableDealerSMS
		,CampaignPriority
		,CASE 
			WHEN @IsActive = 1
				THEN 'Record Activated'
			ELSE 'Record Deactivated'
			END
	FROM PQ_DealerSponsored WITH (NOLOCK)
	WHERE Id IN (
			SELECT ListMember
			FROM fnSplitCSV(@Ids)
			)
END

