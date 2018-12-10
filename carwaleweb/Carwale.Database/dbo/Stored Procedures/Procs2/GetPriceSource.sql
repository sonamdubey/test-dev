IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPriceSource]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPriceSource]
GO

	
-- =============================================
-- Author:		Vicky Lund
-- Create date: 07/06/2016
-- EXEC [GetPriceSource] 18,1
-- =============================================
CREATE PROCEDURE [dbo].[GetPriceSource] @MakeId INT
	,@CityId INT
AS
BEGIN
	SELECT PSM.Id
		,PSM.PriceSourceId
		,PSM.DealerId
	FROM PriceSourceMapping PSM WITH (NOLOCK)
	WHERE PSM.MakeId = @MakeId
		AND PSM.CityId = @CityId
END

