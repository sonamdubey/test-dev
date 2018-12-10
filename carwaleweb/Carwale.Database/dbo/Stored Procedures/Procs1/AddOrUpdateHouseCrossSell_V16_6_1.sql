IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AddOrUpdateHouseCrossSell_V16_6_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AddOrUpdateHouseCrossSell_V16_6_1]
GO

	
-- =============================================
-- Author:		Shalini Nair
-- Create date: 10/05/2016
-- Description:	To add or update house cross-sell campaign
-- Modifier :	Sachin Bharti on 6th June 2016
-- Purpose	:	Added new parameters @CampaignCategoryId ,@ExternalLinkText,@ExtLinkClickTracker,@CarNameClickTracker,@CarImageClickTracker
-- =============================================
CREATE PROCEDURE [dbo].[AddOrUpdateHouseCrossSell_V16_6_1]
	-- Add the parameters for the stored procedure here
	@Id INT
	,@CampaignName VARCHAR(100)
	,@StartDate VARCHAR(50)
	,@EndDate VARCHAR(50)
	,@UpdatedBy INT
	,@CampaignCategoryId INT
	,@ExternalLinkText VARCHAR(100) = NULL
	,@ExtLinkClickTracker VARCHAR(250) = NULL
	,@CarNameClickTracker VARCHAR(250) = NULL
	,@CarImageClickTracker VARCHAR(250) = NULL
AS
BEGIN
	IF @Id = - 1
	BEGIN
		INSERT INTO FeaturedAd (
			NAME
			,StartDate
			,EndDate
			,IsActive
			,UpdatedBy
			,UpdatedOn
			,CampaignCategoryId
			,ExternalLinkText
			,LinkClickTracker
			,CarNameClickTracker
			,CarImageClickTracker
			)
		VALUES (
			@CampaignName
			,@StartDate
			,@EndDate
			,1
			,@UpdatedBy
			,GETDATE()
			,@CampaignCategoryId
			,@ExternalLinkText
			,@ExtLinkClickTracker
			,@CarNameClickTracker
			,@CarImageClickTracker
			)
	END
	ELSE
	BEGIN
		UPDATE FeaturedAd
		SET NAME = @CampaignName
			,StartDate = @StartDate
			,EndDate = @EndDate
			,UpdatedBy = @UpdatedBy
			,UpdatedOn = GETDATE()
			,CampaignCategoryId = @CampaignCategoryId
			,ExternalLinkText = @ExternalLinkText
			,LinkClickTracker = @ExtLinkClickTracker
			,CarNameClickTracker = @CarNameClickTracker
			,CarImageClickTracker = @CarImageClickTracker
		WHERE Id = @Id
	END

	INSERT INTO FeaturedAdLogs (
		FeaturedAdId
		,FeaturedAdName
		,StartDate
		,EndDate
		,IsActive
		,UpdatedOn
		,UpdatedBy
		,Remarks
		,CampaignCategoryId
		,ExternalLinkText
		,LinkClickTracker
		,CarNameClickTracker
		,CarImageClickTracker
		)
	SELECT Id
		,NAME
		,StartDate
		,EndDate
		,IsActive
		,UpdatedOn
		,UpdatedBy
		,CASE 
			WHEN @Id = - 1
				THEN 'Campaign added'
			ELSE 'Campaign updated'
			END
		,@CampaignCategoryId
		,@ExternalLinkText
		,@ExtLinkClickTracker
		,@CarNameClickTracker
		,@CarImageClickTracker
	FROM FeaturedAd WITH (NOLOCK)
	WHERE Id = @Id
END

