IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertPriceAvailabilityModelRules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertPriceAvailabilityModelRules]
GO
	

-- =============================================
-- Author:	Shalini Nair
-- Create date: 28/06/2016
-- Description:	To add make,model,version rules for Price Availability rule
-- =============================================
CREATE PROCEDURE [dbo].[InsertPriceAvailabilityModelRules] @PriceAvailabilityId INT
	,@MakeId INT
	,@ModelId INT
	,@VersionId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO PQ_PriceAvailabilityModelRules (
		PriceAvailabilityId
		,MakeId
		,ModelId
		,VersionId
		)
	VALUES (
		@PriceAvailabilityId
		,CASE 
			WHEN @MakeId = 0
				AND @ModelId = 0
				THEN (
						SELECT cm.ID
						FROM CarVersions CV WITH (NOLOCK)
						JOIN CarModels CMO WITH (NOLOCK) ON CMO.ID = CV.CarModelId
						JOIN CarMakes CM WITH (NOLOCK) ON CM.ID = CMO.CarMakeId
						WHERE CV.ID = @VersionId
						)
			WHEN @MakeId = 0
				THEN (
						SELECT cm.ID
						FROM CarModels CMO WITH (NOLOCK)
						JOIN CarMakes CM WITH (NOLOCK) ON CM.ID = CMO.CarMakeId
						WHERE CMO.ID = @ModelId
						)
			ELSE @MakeId
			END
		,CASE 
			WHEN @ModelId = 0
				THEN (
						SELECT CarModelId
						FROM CarVersions CV WITH (NOLOCK)
						WHERE CV.ID = @VersionId
						)
			ELSE @ModelId
			END
		,@VersionId
		)
END

