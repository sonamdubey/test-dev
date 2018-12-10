IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdatePQCampaign_v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdatePQCampaign_v16]
GO

	-- =============================================
-- Author:		Chetan Thambad
-- Create date: 07-10-2015
-- Description:	Updation of platforms and link text
-- Modified By: Shalini Nair on 17/02/2016 to rename 'Type' column of PQ_DealerSponsored to 'CampaignBehaviour'
-- Modified By: Shalini Nair on 17/10/2016 to save AssignedGroupId
-- =============================================
CREATE PROCEDURE [dbo].[UpdatePQCampaign_v16.10.4] 
	
	@IsDesktop BIT,
	@IsMobile BIT,
	@IsAndroid BIT,
	@IsIPhone BIT,
	@Id INT,
	@UpdatedBy INT,
	@DealerName VARCHAR(80),
	@LinkText varchar(250) = NULL
	AS
	BEGIN

	UPDATE PQ_DealerSponsored
	SET	  IsDesktop = @IsDesktop,
		  DealerName=@DealerName,
          IsMobile = @IsMobile,
          IsAndroid = @IsAndroid,
          IsIPhone = @IsIPhone,
		  LinkText= @LinkText,
		  UpdatedBy=@UpdatedBy,
		  UpdatedOn = GETDATE()
		  WHERE Id = @Id

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
			,CampaignBehaviour
			,TotalGoal
			,DailyGoal
			,LeadPanel
			,CampaignPriority
			,LinkText
			,'Record Updated'
			,EnableUserEmail
			,EnableUserSMS
			,EnableDealerEmail
			,EnableDealerSMS
			,CostPerLead
		FROM PQ_DealerSponsored WITH(NOLOCK)
		WHERE Id = @Id
END

