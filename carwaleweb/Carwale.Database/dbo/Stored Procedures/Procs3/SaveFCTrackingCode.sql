IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SaveFCTrackingCode]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SaveFCTrackingCode]
GO

	
-- =============================================
-- Author:		Amit Verma
-- Create date: 29 AUG 2013
-- Description:	SAVE TRACKING CODE FOR FEATURED VERSIONS
-- =============================================
CREATE PROCEDURE [dbo].[SaveFCTrackingCode]
	-- Add the parameters for the stored procedure here
	@VersionId INT,
	@ImpressionsTracker NVARCHAR(MAX) = null,
	@ImageTracker NVARCHAR(MAX) = null,
	@KeywordTracker NVARCHAR(MAX) = null,
	@OnRoadPriceTracker NVARCHAR(MAX) = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    
    UPDATE FeaturedCarsTrackingCode
    SET ImpressionsTracker = (CASE @ImpressionsTracker WHEN NULL THEN ImpressionsTracker WHEN '' THEN NULL ELSE @ImpressionsTracker END)
    ,ImageTracker = (CASE @ImageTracker WHEN NULL THEN ImageTracker WHEN '' THEN NULL ELSE @ImageTracker END)
    ,KeywordTracker = (CASE @KeywordTracker WHEN NULL THEN KeywordTracker WHEN '' THEN NULL ELSE @KeywordTracker END)
    ,OnRoadPriceTracker = (CASE @OnRoadPriceTracker WHEN NULL THEN OnRoadPriceTracker WHEN '' THEN NULL ELSE @OnRoadPriceTracker END)
    WHERE VersionId = @VersionId
    
    --SELECT FCTC.VersionID,CV.Name,FCTC.ImpressionsTracker
    --,FCTC.ImageTracker,FCTC.KeywordTracker,FCTC.OnRoadPriceTracker
    --FROM FeaturedCarsTrackingCode FCTC WITH (NOLOCK)
    --LEFT JOIN CarVersions CV ON FCTC.VersionID = CV.ID
    --WHERE FCTC.ModelId = @ModelId
    
END

