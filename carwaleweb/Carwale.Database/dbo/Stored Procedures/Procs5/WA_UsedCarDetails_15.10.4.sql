IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WA_UsedCarDetails_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WA_UsedCarDetails_15]
GO

	
/*
Author:Rakesh Yadav
Date: 04 oct 2013
Desc : Fetch used car details,image url and photos of cars

Modified BY : Supriya K on 11/6/2014 to add column cityname & maskingname for both dealer & individulal .Fetch the comments from livelisting table
--Modified by Aditi Dhaybar on 24/2/2015 for  warranty,rsa,service recordsApproved by: Manish Chourasiya on 01-07-2014 4:10pm , With (NoLock) is used.
--Modified by Aditi Dhaybar on 15/4/2015 for getting the warranty related details from Absure_CarDetails table instead o livelistings
--Modified by Aditi dhaybar on 17/4/2015 as the absure cetifications questions got changed.
--Modified by Supriya Bhide on 17/4/2015 for absure certification questions.
--Modified by Supriya Bhide on 29/6/2015 for getting Stockid
--Modified by Supriya Bhide on 11/8/2015 for getting new OriginalImgPath for image.
--Modified by Manish on 06-10-2015 added with (NOLOCK) Wherever not found
--Modified by Purohith Guguloth on 21stoctober,2015 to get a DealerQuickBloxId field in the output 
--Modified by Sahil Sharma on 16-05-2016 to add new param Imei,UsedCarNotificationId,SourceId
*/
CREATE PROCEDURE [dbo].[WA_UsedCarDetails_15.10.4] 
	 @IsDealer INT
	,@InquiryId VARCHAR(10)
	,@ProfileId VARCHAR(10)
	,@CustomerId INT = NULL	

	-- Added by Shikhar Maheshwari on 11 Feb 2015 for fetching AB Sure certification values
	,@Abs_DriveTrain DECIMAL = NULL 
	,@Abs_Suspension DECIMAL = NULL 
	,@Abs_CarExterior DECIMAL = NULL 
	,@Abs_Ac DECIMAL = NULL 
	,@Abs_LatestFeatures DECIMAL = NULL 
	,@Abs_CarInterior DECIMAL = NULL 
	,@Abs_Engine DECIMAL = NULL 
	,@Abs_Tyres DECIMAL = NULL 
	,@Abs_Breaks DECIMAL = NULL 
	,@StockId INT = NULL 

	----Added by Aditi Dhaybar on 28/3/2015 for finance quote details
	--,@EMI INT OUTPUT
	--,@LoanAmount INT  OUTPUT
	--,@InterestRate FLOAT  OUTPUT
	--,@Tenure SMALLINT  OUTPUT
	--,@OtherCharges INT  OUTPUT
	--,@DownPayment INT  OUTPUT
	--,@LoanToValue INT  OUTPUT
	--,@ShowEmi BIT OUTPUT  
	,@Price	DECIMAL(18, 0) = NULL
	,@Imei VARCHAR(50) = NULL --Added by Sahil Sharma on 16-05-2016 to store details page view for app
	,@UsedCarNotificationId TINYINT = 0 --Added by Sahil Sharma on 16-05-2016 to store details page view for app
	,@SourceId SMALLINT = 0 --Added by Sahil Sharma on 16-05-2016 to store details page view for app
AS
BEGIN
	--Start: Modified by Sahil Sharma on 16-05-2016 to store details page view for app
	IF @Imei IS NOT NULL
	BEGIN 
		BEGIN TRY
			INSERT INTO UsedCarDetailViewLog(IMEICode, ProfileId, UsedCarNotificationId, SourceId, EntryTime)
			VALUES(@Imei, @ProfileId, @UsedCarNotificationId, @SourceId, GETDATE());
		END TRY
		BEGIN CATCH
						INSERT INTO CarWaleWebSiteExceptions (
												ModuleName,
												SPName,
												ErrorMsg,
												TableName,
												FailedId,
												CreatedOn)
										VALUES ('Used Car Details App',
												'dbo.WA_UsedCarDetails',
												 ERROR_MESSAGE(),
												 'UsedCarDetailViewLog',
												 @Imei,
												 GETDATE()
												)
		END CATCH	
	END
	--End: Modified by Sahil Sharma on 16-05-2016 to store details page view for app

	-- Get front image path
	SELECT FrontImagePath
		,HostUrl
		,OriginalImgPath
	FROM LiveListings WITH (NOLOCK)
	WHERE Inquiryid=@InquiryId--ProfileId = @ProfileId
	  AND SellerType= (CASE WHEN  @IsDealer=0 THEN 2 ELSE 1 END)

	--Get photos
	SELECT Id
		,ImageUrlFull
		,ImageUrlThumb
		,ImageUrlThumbSmall
		,[Description]
		,IsMain
		,DirectoryPath
		,HostUrl
		,OriginalImgPath
	FROM CarPhotos WITH (NOLOCK)
	WHERE IsActive = 1
		AND InquiryId = @InquiryId
		AND IsDealer = @IsDealer
	ORDER BY IsMain DESC
		,Id

	--Get used car details
	IF @IsDealer = 1
	BEGIN
		SELECT 
			SI.ID
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
			--Spc.NoOfCylinders, Spc.ValueMechanism, Spc.TransmissionType, Spc.FuelType,
			,CFT.FuelType
			,CR.Descr AS TransmissionType
			,SD.Features_SafetySecurity
			,SD.Features_Comfort
			,SD.Features_Others
			,SD.InteriorCondition
			,SD.ACCondition

			--Added by Aditi Dhaybar on 24/2/2015 for  warranty,rsa,service records
			,LL.CertifiedLogoUrl
			--Warranty
			,CA.IsCarInWarranty
			,CA.WarrantyValidTill
			,CA.WarrantyProvidedBy
			,CA.ThirdPartyWarrantyProviderName
			,CA.WarrantyDetails
			,CA.HasExtendedWarranty
			,CA.ExtendedWarrantyValidFor
			,CA.ExtendedWarrantyProviderName
			,CA.ExtendedWarrantyDetails
			--Service Records
			,CA.HasAnyServiceRecords
			,CA.ServiceRecordsAvailableFor
			--RSA
			,CA.HasRSAAvailable
			,CA.RSAValidTill
			,CA.RSAProviderName
			,CA.RSADetails
			,CA.HasFreeRSA
			,CA.FreeRSAValidFor
			,CA.FreeRSAProvidedBy
			,CA.FreeRSADetails
			 --Modified by Aditi Dhaybar on 15/4/2015 for  Absure related details
			,ABC.CarScore AS AbsureScore															
			,ABC.Id		AS AbsureId																							
			,'http://www.autobiz.in/absure/CarCertificate.aspx?carId=' + CONVERT(VARCHAR, ABC.Id) AS AbsureCertificateUrl
			,ABW.Warranty AS AbsureWarranty
			,DL.ActiveMaskingNumber AS MaskingNumber
			,SI.TC_StockId AS StockId	--Added By Supriya Bhide on 29/6/2015
			,isnull(QB.QuickBloxUniqueId,-1) As DealerQuickBloxID
			
		FROM SellInquiries                AS  SI WITH (NOLOCK)
		INNER JOIN SellInquiriesDetails   AS  SD WITH (NOLOCK) ON SI.Id = SD.SellInquiryId
		INNER JOIN LiveListings           AS  LL WITH (NOLOCK) ON LL.Inquiryid =  SI.ID AND LL.SellerType = 1  --Modified  by Aditi Dhaybar on 7-4-2015
		INNER JOIN SellInquiryAccessories AS  SA WITH (NOLOCK) ON SI.Id = SA.CarId
		INNER JOIN CarVersions            AS  CV WITH (NOLOCK) ON SI.CarVersionId = CV.ID
		INNER JOIN Dealers                AS  Ds WITH (NOLOCK) ON SI.DealerId = Ds.ID
		LEFT JOIN TC_CarAdditionalInformation CA WITH (NOLOCK) ON CA.StockId = SI.TC_StockId --Added by Aditi Dhaybar on 24/2/2015 for  warranty,rsa,service records
		LEFT JOIN AbSure_CarDetails          ABC WITH(NOLOCK)  ON ABC.StockId = SI.TC_StockId  --Added by Aditi Dhaybar on 25/2/2015 for  Absure related details 
		LEFT JOIN AbSure_WarrantyTypes       ABW WITH(NOLOCK)  ON ABC.FinalWarrantyTypeId = ABW.AbSure_WarrantyTypesId--Added by Aditi Dhaybar 0n 15/4/2015 for Absure warranty type details
		LEFT JOIN Dealers                     DL WITH (NOLOCK) ON LL.DealerId = DL.ID  --Added by Aditi Dhaybar on 27/2/2015 for MAsking Number details
		LEFT JOIN CarFuelType                CFT WITH (NOLOCK) ON CFT.FuelTypeId = CV.CarFuelType
		LEFT JOIN CarTransmission             CR WITH (NOLOCK) ON CR.Id = CV.CarTransmission
		LEFT JOIN CustomerFavouritesUsed     CFU WITH (NOLOCK) ON CFU.CarProfileId = 'D' + CONVERT(VARCHAR(15), SI.ID)	
																AND CFU.IsActive = 1
																AND (@CustomerId IS NULL OR CFU.CustomerId = @CustomerId)
		LEFT JOIN TC_QuickBloxUsers QB WITH (NOLOCK) ON QB.DealerId =  	SI.DealerId
			,CarModels CMO WITH (NOLOCK)
			,CarMakes CM   WITH (NOLOCK)
			,Cities AS Ct  WITH (NOLOCK)
			,States St     WITH (NOLOCK)
		
		WHERE CV.CarModelId = CMO.ID
			AND CMO.CarMakeId = CM.ID
			AND Ds.CityId = Ct.ID
			AND Ct.StateId = St.Id
			AND SI.ID = @InquiryId


			-- Added by Shikhar on Feb 11, 2015 Fetching the Car Conditions from the Absure Table

select @StockId = TC_StockId,@Price=Price from SellInquiries with (nolock) where Id = @InquiryId

	SELECT 
	C.AbSure_QCategoryId CategoryId
	,CASE SUM(CASE ISNULL(ID.AbSure_AnswerValue,0) 
				WHEN 3 THEN 0 
				ELSE Q.Weightage 
			END)
		WHEN 0 THEN 0
		ELSE ROUND(CAST
						(CAST
							((SUM( CASE WHEN Q.Type <> 2 AND ID.AbSure_AnswerValue=1 THEN ID.AbSure_AnswerValue*Q.Weightage
										WHEN ID.AbSure_AnswerValue=1 AND Q.Type = 2 THEN 2
										WHEN ID.AbSure_AnswerValue=2 AND Q.Type = 2 THEN 1
										ELSE Q.Weightage*0 END)) AS FLOAT) /
							(SUM( CASE WHEN ID.AbSure_AnswerValue = 3 AND Q.Type = 3 THEN 0
							WHEN Q.Type = 2 THEN 2
							ELSE Q.Weightage END)) * 100 AS DECIMAL(8, 2)
						)
					,0)
		END AS CategoryPercentage
	INTO #tempAbsurePartCategory
	FROM       AbSure_Questions      Q WITH (NOLOCK) 
	LEFT JOIN AbSure_InspectionData ID WITH (NOLOCK) ON Q.AbSure_QuestionsId = ID.AbSure_QuestionsId
    INNER JOIN AbSure_QCategory      C with (NOLOCK) ON Q.AbSure_QCategoryId = C.AbSure_QCategoryId
	INNER JOIN AbSure_CarDetails  ABCD WITH(NOLOCK)  ON ID.AbSure_CarDetailsId = ABCD.Id AND ABCD.StockId = @StockId
	INNER JOIN LiveListings         LL WITH(NOLOCK)  ON LL.InquiryId = @InquiryID		-- Modified By Supriya Bhide on 17/4/2015 
	--WHERE Q.IsActive = 1    --Modified by Aditi dhaybar on 17/4/2015 as the absure cetifications questions got changed.
		--WHERE LL.AbsureScore IS NOT NULL AND LL.AbsureScore >= 60	-- Modified By Supriya Bhide on 17/4/2015
	WHERE ABCD.CarScore >=60 
	 AND ABCD.IsSurveyDone = 1 --Modified by Aditi Dhaybar on 24/4/2015
	GROUP BY C.AbSure_QCategoryId

	SELECT @Abs_Ac = CategoryPercentage FROM #tempAbsurePartCategory WITH(NOLOCK) WHERE CategoryId = 1
	SELECT @Abs_Breaks = CategoryPercentage FROM #tempAbsurePartCategory WITH(NOLOCK) WHERE CategoryId = 2
	SELECT @Abs_CarExterior = CategoryPercentage FROM #tempAbsurePartCategory WITH(NOLOCK) WHERE CategoryId = 3
	SELECT @Abs_CarInterior = CategoryPercentage FROM #tempAbsurePartCategory WITH(NOLOCK) WHERE CategoryId = 4
	SELECT @Abs_LatestFeatures = CategoryPercentage FROM #tempAbsurePartCategory WITH(NOLOCK) WHERE CategoryId = 5
	SELECT @Abs_Engine = CategoryPercentage FROM #tempAbsurePartCategory WITH(NOLOCK) WHERE CategoryId = 6
	SELECT @Abs_Suspension = CategoryPercentage FROM #tempAbsurePartCategory WITH(NOLOCK) WHERE CategoryId = 7
	SELECT @Abs_DriveTrain = CategoryPercentage FROM #tempAbsurePartCategory WITH(NOLOCK) WHERE CategoryId = 8
	SELECT @Abs_Tyres = CategoryPercentage FROM #tempAbsurePartCategory WITH(NOLOCK) WHERE CategoryId = 9
	
	SELECT @Abs_Ac AS AbsAc ,@Abs_Breaks AS AbsBrakes,@Abs_CarExterior AS AbsCarExterior,
			@Abs_CarInterior AS AbsCarInterior,@Abs_LatestFeatures AS AbsLatestFeatures,@Abs_Engine AS AbsEngine,
			@Abs_Suspension AS AbsSuspension ,@Abs_DriveTrain AS AbsDriveTrain,@Abs_Tyres AS AbsTyres
	FROM #tempAbsurePartCategory;
	-- Finally dropping the temporary table
	
	DROP TABLE #tempAbsurePartCategory
	-- End of Code added by Shikhar
	--Added by Aditi Dhaybar on 28/3/2015 for finance quote details
	IF @StockId IS NOT NULL
		BEGIN
			SELECT 
				 TCST.EMI
				,TCST.LoanAmount
				,TCST.InterestRate
				,TCST.Tenure
				,TCST.OtherCharges 
				,TCST.LoanToValue
				,@Price - TCST.LoanAmount AS DownPayment
				,TCST.ShowEMIOnCarwale
			FROM TC_Stock TCST WITH(NOLOCK)
			WHERE TCST.Id = @StockId
		END
		--End of code added by Aditi
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
			--,DateAdd(D, -30, CSI.ClassifiedExpiryDate) AS LastUpdated, 
			,LL.LastUpdated
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
			--,Spc.NoOfCylinders, Spc.ValueMechanism, Spc.TransmissionType, Spc.FuelType,
			,CFT.FuelType
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
			,-1  As DealerQuickBloxID
		FROM CustomerSellInquiries AS CSI WITH (NOLOCK)
			INNER JOIN LiveListings LL WITH (NOLOCK)		--Modified  by Aditi Dhaybar on 7-4-2015
			ON LL.Inquiryid =  CSI.ID AND LL.SellerType = 2
		INNER JOIN CustomerSellInquiryDetails CSD WITH (NOLOCK) 
			ON CSI.ID = CSD.InquiryId
		INNER JOIN CarVersions AS CV WITH (NOLOCK) 
			ON CSI.CarVersionId = CV.ID
		INNER JOIN Customers AS CU WITH (NOLOCK) 
			ON CSI.CustomerId = CU.Id
		LEFT JOIN CarFuelType CFT WITH (NOLOCK) 
			ON CFT.FuelTypeId = CV.CarFuelType
		LEFT JOIN CarTransmission CR WITH (NOLOCK) 
			ON CR.Id = CV.CarTransmission
		LEFT JOIN CustomerFavouritesUsed CFU WITH (NOLOCK) 
			ON CFU.CarProfileId = LL.ProfileId
			AND CFU.IsActive = 1
			AND (@CustomerId IS NULL OR CFU.CustomerId = @CustomerId)
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

