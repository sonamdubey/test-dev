IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_DeactivatePQDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_DeactivatePQDealers]
GO

	
-- =============================================
-- Author:		Ruchira Patil
-- Create date: 23th Sept 2014
-- Description:	To deactivate the dealers whose end date has reached and return those dealers
-- Modifier 1: Ruchira Patil(16th Oct 2014) - To maintain the log of deactivated dealers
-- Modified By: Shalini Nair on 17/02/2016 to rename 'Type' column of PQ_DealerSponsored to 'CampaignBehaviour'
-- =============================================
CREATE PROCEDURE [dbo].[Con_DeactivatePQDealers] @DealerName VARCHAR(100) OUTPUT
AS
BEGIN
	DECLARE @TempId VARCHAR(100)
		,@TempName VARCHAR(100)

	SELECT @TempId = COALESCE(@TempId + ',', '') + CAST(Id AS VARCHAR)
		,@TempName = COALESCE(@TempName + ',', '') + CAST(Id AS VARCHAR) + ' - ' + DealerName
	FROM PQ_DealerSponsored WITH (NOLOCK)
	WHERE IsActive = 1
		AND EndDate = CAST(GETDATE() - 1 AS DATE)

	SET @DealerName = @TempName

	UPDATE PQ_DealerSponsored
	SET IsActive = 0
		,UpdatedBy = 13
		,UpdatedOn = GETDATE()
	WHERE Id IN (
			SELECT ListMember
			FROM fnSplitCSV(@TempId)
			)

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
		,'Record Deactivated-EndDate Reached'
	FROM PQ_DealerSponsored WITH (NOLOCK)
	WHERE Id IN (
			SELECT ListMember
			FROM fnSplitCSV(@TempId)
			)
END

/****** Object:  StoredProcedure [dbo].[Con_DeletePQdealers]    Script Date: 2/29/2016 11:26:51 AM ******/
SET ANSI_NULLS ON
