IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WA_UsedCarDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WA_UsedCarDetails]
GO
	/*



Author:Rakesh Yadav



Date: 04 oct 2013



Desc : Fetch used car details,image url and photos of cars

Modified BY : Supriya K on 11/6/2014 to add column cityname & maskingname for both dealer & individulal .Fetch the comments from livelisting table
Approved by: Manish Chourasiya on 01-07-2014 4:10pm , With (NoLock) is used.
*/
CREATE PROCEDURE [dbo].[WA_UsedCarDetails] @IsDealer INT
	,@InquiryId VARCHAR(10)
	,@ProfileId VARCHAR(10)
	,@CustomerId NUMERIC(18, 0) = NULL
AS
BEGIN
	-- Get front image path
	SELECT FrontImagePath
		,HostUrl
	FROM LiveListings WITH (NOLOCK)
	WHERE ProfileId = @ProfileId

	--Get photos
	SELECT Id
		,ImageUrlFull
		,ImageUrlThumb
		,ImageUrlThumbSmall
		,[Description]
		,IsMain
		,DirectoryPath
		,HostUrl
	FROM CarPhotos WITH (NOLOCK)
	WHERE IsActive = 1
		AND InquiryId = @InquiryId
		AND IsDealer = @IsDealer
	ORDER BY IsMain DESC
		,Id

	--Get used car details
	IF @IsDealer = 1
	BEGIN
		SELECT SI.ID
			,CM.ID AS MakeId
			,CMO.ID AS ModelId
			,CV.ID AS VersionId
			,CM.NAME AS Make
			,CM.LogoUrl AS LogoUrl
			,CMO.NAME AS Model
			,CMO.MaskingName AS MaskingName
			,CV.NAME AS Version
			,CV.largePic AS CarLargePicUrl
			,SI.Price AS Price
			,SI.Kilometers AS Kilometers
			,SI.MakeYear AS MakeYear
			,SI.Color
			,SI.ColorCode
			,LL.Comments AS Comments
			,Ct.NAME AS CityName
			,Ct.ID AS CityId
			,St.NAME STATE
			,St.StateCode
			,SI.LastUpdated
			,SA.Accessories
			,SD.Owners
			,SD.Insurance
			,SD.InsuranceExpiry
			,SD.OneTimeTax Tax
			,SD.RegistrationPlace
			,SI.StatusId
			,SD.InteriorColor AS InteriorColor
			,SD.InteriorColorCode AS InteriorColorCode
			,SD.CityMileage AS CityMileage
			,SD.AdditionalFuel
			,SD.CarDriven
			,SD.Accidental
			,SD.FloodAffected
			,SD.Warranties
			,SD.Modifications
			,SD.BatteryCondition
			,SD.BrakesCondition
			,SD.ElectricalsCondition
			,SD.EngineCondition
			,SD.ExteriorCondition
			,SD.SeatsCondition
			,SD.SuspensionsCondition
			,SD.TyresCondition
			,SD.OverallCondition
			,Ds.ID AS SellerId
			,'' AS ExpiryDate
			,Ds.CityId AS CarCityId
			,Ds.STATUS AS IsFake
			,0 AS IsCarFake
			,SI.PackageExpiryDate
			,1 AS isDealer
			,Si.PackageType
			,
			--Spc.NoOfCylinders, Spc.ValueMechanism, Spc.TransmissionType, Spc.FuelType,
			CFT.FuelType
			,CR.Descr AS TransmissionType
			,SD.Features_SafetySecurity
			,SD.Features_Comfort
			,SD.Features_Others
			,SD.InteriorCondition
			,SD.ACCondition
		FROM SellInquiries AS SI WITH (NOLOCK)
		INNER JOIN SellInquiriesDetails SD WITH (NOLOCK) ON SI.Id = SD.SellInquiryId
		INNER JOIN LiveListings LL WITH (NOLOCK) ON LL.ProfileId = 'D' + CONVERT(VARCHAR(15), SI.ID)
		INNER JOIN SellInquiryAccessories SA WITH (NOLOCK) ON SI.Id = SA.CarId
		INNER JOIN CarVersions AS CV WITH (NOLOCK) ON SI.CarVersionId = CV.ID
		INNER JOIN Dealers AS Ds WITH (NOLOCK) ON SI.DealerId = Ds.ID
		LEFT JOIN CarFuelType CFT WITH (NOLOCK) ON CFT.FuelTypeId = CV.CarFuelType
		LEFT JOIN CarTransmission CR WITH (NOLOCK) ON CR.Id = CV.CarTransmission
		LEFT JOIN CustomerFavouritesUsed CFU WITH (NOLOCK) ON CFU.CarProfileId = 'D' + CONVERT(VARCHAR(15), SI.ID)
			AND CFU.IsActive = 1
			AND (
				@CustomerId IS NULL
				OR CFU.CustomerId = @CustomerId
				)
			,CarModels CMO
			,CarMakes CM
			,Cities AS Ct
			,States St
		WHERE CV.CarModelId = CMO.ID
			AND CMO.CarMakeId = CM.ID
			AND Ds.CityId = Ct.ID
			AND Ct.StateId = St.Id
			AND SI.ID = @InquiryId
	END
	ELSE
	BEGIN
		SELECT CM.ID AS MakeId
			,CMO.ID AS ModelId
			,CV.ID AS VersionId
			,CM.NAME AS Make
			,CM.LogoUrl AS LogoUrl
			,CMO.NAME AS Model
			,CMO.MaskingName AS MaskingName
			,CV.NAME AS Version
			,CV.LargePic AS CarLargePicUrl
			,CSI.Price AS Price
			,CSI.Kilometers AS Kilometers
			,CSI.MakeYear AS MakeYear
			,CSI.Color
			,CSI.ColorCode
			,CSI.ReasonForSelling
			,LL.Comments AS Comments
			,C.NAME AS CityName
			,C.ID AS CityId
			,St.NAME STATE
			,St.StateCode
			,
			--DateAdd(D, -30, CSI.ClassifiedExpiryDate) AS LastUpdated, 
			LL.LastUpdated
			,CSD.Accessories
			,CSD.Owners
			,CSD.Insurance
			,CSD.InsuranceExpiry
			,CSD.Tax AS Tax
			,CSD.RegistrationPlace
			,CSD.InteriorColor
			,CSD.InteriorColorCode
			,CSD.CityMileage
			,CSD.AdditionalFuel
			,CSD.CarDriven
			,CSD.Accidental
			,CSD.FloodAffected
			,CSD.Accessories
			,CSD.Warranties
			,CSD.Modifications
			,
			--Spc.NoOfCylinders, Spc.ValueMechanism, Spc.TransmissionType, Spc.FuelType,
			CFT.FuelType
			,CR.Descr AS TransmissionType
			,CSD.Features_SafetySecurity
			,CSD.Features_Comfort
			,CSD.Features_Others
			,CSD.ACCondition
			,CSD.BatteryCondition
			,CSD.BrakesCondition
			,CSD.ElectricalsCondition
			,CSD.EngineCondition
			,CSD.ExteriorCondition
			,CSD.InteriorCondition
			,CSD.SeatsCondition
			,CSD.SuspensionsCondition
			,CSD.TyresCondition
			,CSD.OverallCondition
			,CU.ID AS SellerId
			,CSI.ClassifiedExpiryDate AS ExpiryDate
			,CSI.CityId AS CarCityId
			,CU.IsFake
			,CSI.IsFake AS IsCarFake
			,CSI.PackageExpiryDate
			,Csi.StatusId
			,0 AS IsDealer
			,CSI.PackageType
		FROM CustomerSellInquiries AS CSI WITH (NOLOCK)
		INNER JOIN LiveListings LL WITH (NOLOCK) ON LL.ProfileId = 'S' + CONVERT(VARCHAR(15), CSI.ID)
		INNER JOIN CustomerSellInquiryDetails CSD WITH (NOLOCK) ON CSI.ID = CSD.InquiryId
		INNER JOIN CarVersions AS CV WITH (NOLOCK) ON CSI.CarVersionId = CV.ID
		INNER JOIN Customers AS CU WITH (NOLOCK) ON CSI.CustomerId = CU.Id
		LEFT JOIN CarFuelType CFT WITH (NOLOCK) ON CFT.FuelTypeId = CV.CarFuelType
		LEFT JOIN CarTransmission CR WITH (NOLOCK) ON CR.Id = CV.CarTransmission
		LEFT JOIN CustomerFavouritesUsed CFU WITH (NOLOCK) ON CFU.CarProfileId = LL.ProfileId
			AND CFU.IsActive = 1
			AND (
				@CustomerId IS NULL
				OR CFU.CustomerId = @CustomerId
				)
			,Cities AS C
			,CarModels CMO
			,CarMakes CM
			,States St
		WHERE CV.CarModelId = CMO.ID
			AND CMO.CarMakeId = CM.ID
			AND C.ID = CSI.CityId
			AND C.StateId = St.Id
			AND CSI.ID = @InquiryId
	END
END

