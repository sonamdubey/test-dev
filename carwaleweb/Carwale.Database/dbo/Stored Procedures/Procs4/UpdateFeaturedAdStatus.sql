IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateFeaturedAdStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateFeaturedAdStatus]
GO

	


-- =============================================
-- Author:		Shalini Nair
-- Create date: 11/05/2016
-- Description:	To pause or resume a featured campaign(House cross-sell)
-- =============================================
CREATE PROCEDURE [dbo].[UpdateFeaturedAdStatus]
	-- Add the parameters for the stored procedure here
	@Ids VARCHAR(500)
	,@IsActive BIT
	,@UpdatedBy INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	UPDATE FeaturedAd
	SET IsActive = @IsActive
		,UpdatedBy = @UpdatedBy
		,UpdatedOn = GETDATE()
	WHERE Id IN (
			SELECT ListMember
			FROM fnSplitCSV(@Ids)
			)

	INSERT INTO FeaturedAdLogs (
		FeaturedAdId
		,FeaturedAdName
		,StartDate
		,EndDate
		,IsActive
		,UpdatedBy
		,UpdatedOn
		,Remarks
		)
	SELECT Id
		,NAME
		,StartDate
		,EndDate
		,IsActive
		,UpdatedBy
		,UpdatedOn
		,CASE 
			WHEN @IsActive = 1
				THEN 'Campaign resumed'
			ELSE 'Campaign paused'
			END
	FROM FeaturedAd WITH (NOLOCK)
END


