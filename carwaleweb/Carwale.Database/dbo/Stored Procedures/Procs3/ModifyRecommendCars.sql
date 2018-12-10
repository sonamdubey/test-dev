IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ModifyRecommendCars]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ModifyRecommendCars]
GO

	-- =============================================
-- Author:		<Prashant Vishe>
-- Create date: <16 Oct 2013>
-- Description:	<for modifying recommend cars..>
-- =============================================
CREATE PROCEDURE [dbo].[ModifyRecommendCars] 
	-- Add the parameters for the stored procedure here
	@versionId int,
	@dimensionAndSpace float,
	@comfort float,
	@performance float,
	@convenience float,
	@safety float,
	@entertainment float,
	@aesthetics float,
	@salesAndSupport float,
	@fuelEconomy float
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	

	UPDATE RecommendCars SET DimensionAndSpace=@dimensionAndSpace,
							Comfort=@comfort,
							Performance=@performance,
							Convenience=@convenience,
							Safety=@safety,
							Entertainment=@entertainment,
							Aesthetics=@aesthetics,
							SalesAndSupport=@salesAndSupport,
							FuelEconomy=@fuelEconomy
	WHERE Versionid=@versionId
END
