IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Opr_LiveListing]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Opr_LiveListing]
GO

	CREATE PROCEDURE [dbo].[Opr_LiveListing]
	
		@Sellertype NUMERIC,
		@MakeId NUMERIC = NULL,
		@ModelId NUMERIC = NULL,
		@VersionId NUMERIC = NULL,
		@StateId NUMERIC = NULL,
		@CityId NUMERIC = NULL
AS
BEGIN

		IF(@Sellertype = 1)
			BEGIN
				-- FOR DEALERS
				SELECT LL.ProfileId, LL.SellerType, LL.Seller, LL.Inquiryid, 
				DL.Organization AS _Name, DL.EmailId AS Email, DL.MobileNo AS Mobile,LL.MakeName AS Make, LL.ModelName  AS Model, 
				LL.VersionName AS Version,YEAR(LL.MakeYear) AS MakeYear,LL.MakeYear AS MakeMonth, LL.Kilometers AS kilometers, 
				LL.Price AS Price,LL.EntryDate AS ListingDate, LL.CityName AS City, LL.ProfileId AS ProfileId,BDCV.Valuation AS CWVal 
				FROM LiveListings LL 
				INNER JOIN SellInquiries  AS SI ON LL.Inquiryid=SI.ID 
				INNER JOIN Dealers AS DL ON SI.DealerId=DL.ID 
				LEFT JOIN BestDealCarValuations AS BDCV ON BDCV.CarId=LL.Inquiryid AND BDCV.UserType = 1
	
				WHERE LL.SellerType=@Sellertype 
				AND LL.MakeId = COALESCE(@MakeId , LL.MakeId) 
				AND LL.ModelId =COALESCE(@ModelId, LL.ModelId) AND LL.VersionId =COALESCE(@VersionId, LL.VersionId)
				AND LL.StateId =COALESCE(@StateId, LL.StateId) AND LL.CityId =COALESCE(@CityId, LL.CityId) 
				
				ORDER BY DL.Organization
			END
		ELSE
			BEGIN
			
				--FOR INDIVIDUALS
				SELECT LL.ProfileId, LL.SellerType, LL.Seller, LL.Inquiryid,  BDCV.CarId,
				CL.Name AS _Name, CL.email AS Email, CL.Mobile AS Mobile,LL.MakeName AS Make, LL.ModelName  AS Model, 
				LL.VersionName AS Version,YEAR(LL.MakeYear) AS MakeYear,LL.MakeYear AS MakeMonth, LL.Kilometers AS kilometers, 
				LL.Price AS Price,LL.EntryDate AS ListingDate, LL.CityName AS City, LL.ProfileId AS ProfileId,BDCV.Valuation AS CWVal 
				FROM LiveListings LL 
				LEFT JOIN BestDealCarValuations AS BDCV ON BDCV.CarId=Convert(Int,SUBSTRING(LL.ProfileId,2,LEN(LL.ProfileId)-1)) AND BDCV.UserType=2 
				LEFT JOIN CustomerSellInquiries SI ON LL.Inquiryid=SI.ID
				INNER JOIN Customers AS CL ON SI.CustomerId=CL.ID  
				WHERE LL.SellerType=@Sellertype
				AND LL.MakeId = COALESCE(@MakeId , LL.MakeId) 
				AND LL.ModelId =COALESCE(@ModelId, LL.ModelId) AND LL.VersionId =COALESCE(@VersionId, LL.VersionId)
				AND LL.StateId =COALESCE(@StateId, LL.StateId) AND LL.CityId =COALESCE(@CityId, LL.CityId) 
				
				ORDER BY CL.Name
				
			END	
		

END

