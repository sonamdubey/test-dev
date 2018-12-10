IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetFeaturedVersions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetFeaturedVersions]
GO

	
-- =============================================
-- Author:		Amit Verma
-- Create date: 29 AUG 2013
-- Description:	GET FEATURED VERSIONS
-- =============================================
CREATE PROCEDURE [dbo].[GetFeaturedVersions]
	-- Add the parameters for the stored procedure here
	@ModelId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT FCTC.VersionID,CV.Name,FCTC.ImpressionsTracker
    ,FCTC.ImageTracker,FCTC.KeywordTracker,FCTC.OnRoadPriceTracker
    FROM FeaturedCarsTrackingCode FCTC WITH (NOLOCK)
    LEFT JOIN CarVersions CV ON FCTC.VersionID = CV.ID
    WHERE FCTC.ModelId = @ModelId
    
END

