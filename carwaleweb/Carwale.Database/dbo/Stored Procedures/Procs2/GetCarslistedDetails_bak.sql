IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCarslistedDetails_bak]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCarslistedDetails_bak]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 8/25/2011
-- Description:	Cars listed details
-- =============================================
CREATE PROCEDURE [dbo].[GetCarslistedDetails_bak] 
	-- Add the parameters for the stored procedure here
	@Days smallint
AS
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @date datetime
	SET @date = GETDATE()-@Days

	SELECT CS.Id as ProfileId,CS.DealerId,CS.EntryDate,CT.City as City, CT.State as State,M.Make,M.Model,M.Version,CS.MakeYear,CS.Kilometers,CS.Price,1 as SellerType,'Dealer' as Seller
	FROM SellInquiries as CS
	  join vwMMV as M on M.VersionId=CS.CarVersionId
	  join Dealers as D ON D.ID=CS.DealerId
	  join vwCityDetails as CT on CT.CityId=D.CityId
	WHERE CS.EntryDate>=@date
	
	UNION
	
	SELECT CS.Id as ProfileId,CS.CustomerId,CS.EntryDate,CT.City, CT.State,M.Make,M.Model,M.Version,CS.MakeYear,CS.Kilometers,CS.Price,2 as SellerType,'Individual' as Seller
	FROM CustomerSellInquiries as CS
	join vwCityDetails as CT on CT.CityId=CS.CityId
	join vwMMV as M on M.VersionId=CS.CarVersionId
	WHERE CS.EntryDate>=@date
	ORDER BY CS.EntryDate

END
