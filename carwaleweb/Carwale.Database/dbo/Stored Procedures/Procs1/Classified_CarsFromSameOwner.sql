IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_CarsFromSameOwner]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_CarsFromSameOwner]
GO

	-- =============================================
-- Author:		<kush kumar>
-- Create date: <01/11/2012>
-- Description:	<Procedure to return details about cars from same owner>
-- Modified By : Ashish G. Kamble on 8 Feb 2013
-- Modified By : Akansha on 1.07.2013 (Made top 3 car random)
-- Added : Make, model, and city details
-- exec [Classified_CarsFromSameOwner] 5,926000,'D8832'
-- Modified By : Akansha Srivastava on 12.2.2014
-- Description : Added MaskingName Column

-- =============================================

CREATE PROCEDURE   [dbo].[Classified_CarsFromSameOwner]

	-- Input Parameters
	@SellerId BigInt,
	@Price BigInt,
	@ProfileNo VarChar(50)
	AS
	BEGIN
		SELECT TOP 3 LL.ProfileId
	,(MakeName + ' ' + ModelName + ' ' + VersionName) CarMake
	,MakeName
	,ModelName
	,CMO.MaskingName
	,C.NAME AS CityName
	,LL.MakeYear
	,LL.Price
	,LL.Color
	,LL.Kilometers
	,LL.PhotoCount
	,isnull(LL.HostURL, '') + LL.FrontImagePath AS ImagePath
FROM LiveListings AS LL WITH (NOLOCK)
INNER JOIN  SellInquiries Si ON Si.Id = LL.Inquiryid AND LL.SellerType = 1 
INNER JOIN Cities AS C ON C.ID = LL.CityId
Inner Join CarModels CMO ON LL.ModelId=CMO.ID
WHERE LL.ProfileId <> @ProfileNo
	AND LL.SellerType = 1
	AND ll.Price BETWEEN (@price - @price * 0.3)
		AND (@price + @price * 0.3)
	AND SI.DealerId = @SellerId
ORDER BY NEWID()
	,CarMake

END
