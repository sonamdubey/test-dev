IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AddOrUpdateHouseCrossSell]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AddOrUpdateHouseCrossSell]
GO

	

-- =============================================
-- Author:		Shalini Nair
-- Create date: 10/05/2016
-- Description:	To add or update house cross-sell campaign
-- =============================================
CREATE PROCEDURE [dbo].[AddOrUpdateHouseCrossSell]
	-- Add the parameters for the stored procedure here
	@Id INT
	,@CampaignName VARCHAR(100)
	,@StartDate VARCHAR(50)
	,@EndDate VARCHAR(50)
	,@UpdatedBy INT
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
			)
		VALUES (
			@CampaignName
			,@StartDate
			,@EndDate
			,1
			,@UpdatedBy
			,GETDATE()
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
	FROM FeaturedAd WITH (NOLOCK)
END


