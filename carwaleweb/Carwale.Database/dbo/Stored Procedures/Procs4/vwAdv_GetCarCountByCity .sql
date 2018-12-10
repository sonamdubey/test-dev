IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[vwAdv_GetCarCountByCity ]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[vwAdv_GetCarCountByCity ]
GO

	

-- =============================================
-- Author:	 Harshil
-- Create date: 05-08-2016
-- Description:	Bring the count of distinct root ids for which the deals are available in that particular city 
-- exec [dbo].[vwAdv_GetCarCountByCity ] 1
-- =============================================
create PROCEDURE [dbo].[vwAdv_GetCarCountByCity ]
	@CityId int	
AS
BEGIN
	select count(distinct RootId)  from vwLiveDeals WITH(NOLOCK) where cityId = @CityId;
END
