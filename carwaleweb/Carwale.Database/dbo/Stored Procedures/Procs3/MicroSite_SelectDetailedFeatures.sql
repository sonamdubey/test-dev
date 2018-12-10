IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[MicroSite_SelectDetailedFeatures]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[MicroSite_SelectDetailedFeatures]
GO

	-- =============================================
-- Author:		Umesh Ojha
-- Create date: 16-Feb-2011
-- Description:	Fetching data for car features
-- =============================================
CREATE PROCEDURE [dbo].[MicroSite_SelectDetailedFeatures]
	-- Add the parameters for the stored procedure here
	@VersionID BigInt
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT NC.Id AS CategoryId, NC.Name As Category FROM NewCarFeatureCategories NC WHERE NC.Id IN
	(SELECT DISTINCT CategoryId FROM NewCarFeatures NF, NewCarFeatureItems NI 
	WHERE NF.FeatureItemId=NI.Id AND CarVersionId=@VersionID)
	
	SELECT NI.ID, NI.Name Feature, CategoryId FROM NewCarFeatures NF, NewCarFeatureItems NI
	WHERE NF.FeatureItemId=NI.Id AND CarVersionId=@VersionID
END