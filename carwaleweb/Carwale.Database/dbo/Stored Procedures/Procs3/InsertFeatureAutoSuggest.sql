IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertFeatureAutoSuggest]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertFeatureAutoSuggest]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(7th June 2016)
-- Description	:	Insert sponsored auto suggest details 
-- =============================================
CREATE PROCEDURE [dbo].[InsertFeatureAutoSuggest]
	@FeatureAdId INT,
	@FeatureModelId INT,
	@TargetModelId	INT,
	@ImpressionTracker VARCHAR(250),
	@ClickTracker VARCHAR(250),
	@AddedBy INT	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO FeatureAutoSuggest ( FeaturedAdId,FeaturedModelId,TargetModelId,ImpressionTracker,ClickTracker,AddedBy,AddedOn )
				VALUES(@FeatureAdId,@FeatureModelId,@TargetModelId,@ImpressionTracker,@ClickTracker,@AddedBy,GETDATE())
END

