IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RetrieveRecommendScoreForVersion]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RetrieveRecommendScoreForVersion]
GO

	-- =============================================

-- Author:		<Prashant Vishe>

-- Create date: <16 Oct 2013>

-- Description:	<For retrieving recommend score for paticular version...>

-- =============================================

CREATE PROCEDURE [dbo].[RetrieveRecommendScoreForVersion] 

	-- Add the parameters for the stored procedure here

	@VersionId int

AS

BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from

	-- interfering with SELECT statements.

	SET NOCOUNT ON;



    -- Insert statements for procedure here

	select MakeName,ModelName,VersionId,VersionName,DimensionAndSpace,Comfort,Performance,Convenience,Safety,Entertainment,Aesthetics,SalesAndSupport,FuelEconomy from RecommendCars where VersionId=@VersionId

END

