IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ES_GetListingDetailByProfileId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ES_GetListingDetailByProfileId]
GO

	-----------------------------------------------------------------
-- Created By : Sadhana Upadhyay on 25 March 2015
-- Summary : To get Listing Detail by profile Id
-- EXEC ES_GetListingDetailByProfileId 'S4094'
-- Modified By : Sadhana Upadhyay on 6 Apr 2015
-- Summary: Added Isnull condition
-- Modified By : Sadhana Upadhyay on 14 Apr 2015
-- Summary : Added isnull condition for LastUpdated column
-- Modified By : Sadhana Upadhyay on 16 Apr 2015
-- Summary : added Condition for sellertype in query
--Modified By : Purohith Guguloth on 6th oct, 2015
-- Summary : Added a join on the table TC_QuickBloxUsers to get dealerQuickBloxId
--Modified By : Purohith Guguloth on 23th oct, 2015
-- Summary : made dealerQuickBloxId's default value to -1
--Modified By : Purohith Guguloth on 23th oct, 2015 Corrected spelling  field name "DealerQuickBloxId"
--Modified By : Sahil Sharma on 25-01-2016, Added VersionSubSegmentID from CarVersions table
--Modified By : Supriya Bhide on 02/04/2016, Added PackageId from Packages Table
--Modified By : Sahil Sharma on 09/02/2016, Added CityIds from DealerMultiCity Table
--Modified By : Prachi Phalak on 29/03/2016, Added SVScore from livelistings table.
--Modified By : Supriya Bhide on 13/05/2016, Removed packageId from elastic index
 -----------------------------------------------------------------
CREATE PROCEDURE [dbo].[ES_GetListingDetailByProfileId] @ProfileId VARCHAR(50)
AS
BEGIN
	DECLARE @SellerTypeId AS INT

	SELECT @SellerTypeId =
		CASE LEFT(@ProfileId, 1)
			 WHEN 'D' THEN 1
			 WHEN 'S' THEN 2
		  END

    IF (@SellerTypeId in (1,2))
	BEGIN
			SELECT ProfileId
				,ISNULL(SellerType,0) AS SellerType
				,Seller
				,ISNULL(Inquiryid, 0) Inquiryid
				,ISNULL(LL.MakeId, 0) MakeId
				,LL.MakeName
				,CMO.MaskingName
				,ISNULL(LL.ModelId, 0) ModelId
				,ISNULL(LL.OfferStartDate, '') OfferStartDate
				,ISNULL(LL.OfferEndDate, '') OfferEndDate
				,LL.HostUrl
				,LL.ModelName
				,ISNULL(LL.VersionId, 0) VersionId
				,LL.VersionName
				,ISNULL(LL.StateId, 0) StateId
				,LL.StateName
				,ISNULL(LL.CityId, 0) CityId
				,LL.CityName
				,ISNULL(LL.AreaId, 0) AreaId
				,ISNULL(AreaName, '') AreaName
				,ISNULL(LL.Lattitude, 0) Lattitude
				,ISNULL(LL.Longitude, 0) Longitude
				,YEAR(MakeYear) MakeYear
				,ISNULL(Price, 0) Price
				,ISNULL(Kilometers, 0) Kilometers
				,ISNULL(Color, '') Color
				,ISNULL(EntryDate, GETDATE()) AS EntryDate
				,ISNULL(LastUpdated, GETDATE()) AS LastUpdated
				,ISNULL(LL.PackageType, 0) PackageType
				--,P.Id AS PackageId	 --Supriya Bhide on 13/05/2016, Removed packageId from elastic index
				,ISNULL(LL.ShowDetails, 0) ShowDetails
				,ISNULL(Priority, 0) Priority
				,ISNULL(LL.PhotoCount, 0) PhotoCount
				,ISNULL(LL.FrontImagePath, '') FrontImagePath
				,ISNULL(LL.CertificationId, 0) CertificationId
				,ISNULL(LL.AdditionalFuel, '') AdditionalFuel
				,ISNULL(LL.Score, 0) Score
				,ISNULL(LL.Responses, 0) Responses
				,ISNULL(LL.CertifiedLogoUrl, '') CertifiedLogoUrl
				,Owners
				,ISNULL(InsertionDate, GETDATE()) AS InsertionDate
				,ISNULL(LL.DealerId, 0) DealerId
				,ISNULL(LL.IsPremium, 0) IsPremium
				,ISNULL(LL.VideoCount, 0) VideoCount
				,ISNULL(SortScore, 0) SortScore
				,ISNULL(ImageUrlMedium, '') ImageUrlMedium
				,ISNULL(LL.RootId, 0) RootId
				,LL.RootName
				,ISNULL(OwnerTypeId, 0) OwnerTypeId
				,ISNULL(LL.Emi, 0) Emi
				,ISNULL(VS.CarFuelType, 0) FuelTypeId
				,FT.Descr FuelType
				,ISNULL(VS.CarTransmission, 0) TransmissionId
				,ISNULL(VS.BodyStyleId, 0) BodyStyleId
				,BS.NAME BodyStyle
				,TM.Descr Transmission
				,LL.Comments
				,ISNULL(LL.UsedCarMasterColorsId, 0) AS UsedCarMasterColorsId
				,ISNULL(LL.AbsureScore, 0) AS AbsureScore
				,LL.AbsureWarrantyType
				,D.ActiveMaskingNumber AS MaskingNumber
				,(
					CASE 
						WHEN GETDATE() BETWEEN LL.OfferStartDate
								AND LL.OfferEndDate
							THEN 1
						ELSE 0
						END
					) AS IsHotDeal
				,ISNULL(CMR.IsSuperLuxury, 0) IsSuperLuxury
				,ISNULL(CMO.SubSegmentID, 0) AS SubSegmentID
				,ISNULL(VS.SubSegmentID, 0) AS VersionSubSegmentID --Added by Sahil Sharma 25-01-2016
				,LL.SellerName
				,LL.SellerContact
				,LL.OriginalImgPath
				,IsNull(QB.QuickBloxUniqueId,-1) As DealerQuickBloxId
				,IsNull(DMC.Cities , '') AS CityIds --Modified By : Sahil Sharma on 09/02/2016
				,ISNULL(LL.SVScore,0) AS svScore    --Modified By : Prachi Phalak on 29/03/2016
			FROM LiveListings LL WITH (NOLOCK)
			INNER JOIN CarModels CMO WITH (NOLOCK) ON LL.ModelId = CMO.ID
			INNER JOIN CarModelRoots CMR WITH (NOLOCK) ON CMR.RootId = CMO.RootId
			INNER JOIN CarMakes C WITH (NOLOCK) ON LL.MakeId = C.Id
			INNER JOIN CarVersions VS WITH (NOLOCK) ON VS.ID = LL.VersionId
			INNER JOIN CarBodyStyles BS WITH (NOLOCK) ON BS.ID = VS.BodyStyleId
			INNER JOIN CarTransmission TM WITH (NOLOCK) ON TM.Id = VS.CarTransmission
			INNER JOIN CarFuelTypes FT WITH (NOLOCK) ON FT.CarFuelTypeId = VS.CarFuelType
			LEFT JOIN Dealers D WITH (NOLOCK) ON D.ID = LL.DealerId
			LEFT JOIN TC_QuickBloxUsers QB WITH (NOLOCK) ON QB.DealerId = D.ID
			--LEFT JOIN Packages P WITH(NOLOCK) ON LL.PackageType = P.InqPtCategoryId	 Supriya Bhide on 13/05/2016, Removed packageId from elastic index
			LEFT JOIN DealerMultiCity DMC WITH(NOLOCK) ON DMC.DealerId = LL.DealerId --Modified By : Sahil Sharma on 09/02/2016
			 
			WHERE ProfileId = @ProfileId AND LL.SellerType = @SellerTypeId
		END
END
