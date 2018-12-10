IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[vwAdv_GetDealModelsByMakeAndCity]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[vwAdv_GetDealModelsByMakeAndCity]
GO

	
-- =============================================
-- Author:		Anchal Gupta
-- Create date: 17-03-2016
-- Description:	Bring all the models for which the deals are available in that particular city and that particular model
-- exec [dbo].[vwAdv_GetDealModelsByMakeAndCity] 1, 18
-- =============================================
CREATE PROCEDURE [dbo].[vwAdv_GetDealModelsByMakeAndCity]
	-- Add the parameters for the stored procedure here
	@CityId int,
	@MakeId int
AS
BEGIN
	select Distinct ModelId, Model as ModelName, MaskingName from  vwLiveDeals WITH(NOLOCK) where CityId = @CityId and MakeId = @MakeId
	order by Model, ModelId asc
END

