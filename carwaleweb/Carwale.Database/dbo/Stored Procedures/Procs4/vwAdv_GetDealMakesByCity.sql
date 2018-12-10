IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[vwAdv_GetDealMakesByCity]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[vwAdv_GetDealMakesByCity]
GO

	
-- =============================================
-- Author:		Anchal Gupta
-- Create date: 17-03-2016
-- Description:	Bring all the makes for which the deals are available in that particular city
-- =============================================
CREATE PROCEDURE [dbo].[vwAdv_GetDealMakesByCity]
	-- Add the parameters for the stored procedure here
	@CityId int
AS
BEGIN
	select Distinct MakeId, make as MakeName from vwLiveDeals WITH(NOLOCK) where CityId = @CityId 
	order by make, MakeId asc
END
