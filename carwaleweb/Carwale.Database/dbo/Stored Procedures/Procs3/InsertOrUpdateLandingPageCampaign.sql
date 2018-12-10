IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertOrUpdateLandingPageCampaign]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertOrUpdateLandingPageCampaign]
GO

	
-- =============================================
-- Author:		Shalini Nair
-- Create date: 28/07/2016
-- Description:	To save and update landing page campaign
-- =============================================
create PROCEDURE [dbo].[InsertOrUpdateLandingPageCampaign]
	-- Add the parameters for the stored procedure here
	@Id INT
	,@CampaignName VARCHAR(50)
	,@Type VARCHAR(50)
	,@PrimaryHeading VARCHAR(50)
	,@SecondaryHeading VARCHAR(50) = NULL 
	,@IsEmailRequired BIT
	,@DefaultModelId INT = NULL
	,@ButtonText VARCHAR(50)
	,@TrailingText VARCHAR(50) = NULL
	,@IsDesktop BIT 
	,@IsMobile BIT
	,@UpdatedBy INT
	,@NewId INT = NULL OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @Id = 0
	BEGIN
		INSERT INTO LandingPageCampaign (
			Name
			,Type
			,PrimaryHeading
			,SecondaryHeading
			,IsEmailRequired
			,DefaultModel
			,ButtonText
			,TrailingText
			,IsActive
			,IsDesktop
			,IsMobile
			,CreatedBy
			,CreatedOn
			,UpdatedBy
			,UpdatedOn
			)
		VALUES (
			@CampaignName
			,@Type
			,@PrimaryHeading
			,@SecondaryHeading
			,@IsEmailRequired
			,@DefaultModelId
			,@ButtonText
			,@TrailingText
			,1
			,@IsDesktop
			,@IsMobile
			,@UpdatedBy
			,GETDATE()
			,@UpdatedBy
			,GETDATE()
			)

		SET @NewId = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE LandingPageCampaign
		SET Name = @CampaignName
			,Type = @Type
			,PrimaryHeading = @PrimaryHeading
			,SecondaryHeading = @SecondaryHeading
			,IsEmailRequired = @IsEmailRequired
			,DefaultModel = @DefaultModelId
			,ButtonText = @ButtonText
			,TrailingText = @TrailingText
			,IsDesktop = @IsDesktop
			,IsMobile = @IsMobile
			,UpdatedBy = @UpdatedBy
			,UpdatedOn = GETDATE()
		WHERE Id = @Id

		SET @NewId = @Id
	END
END
