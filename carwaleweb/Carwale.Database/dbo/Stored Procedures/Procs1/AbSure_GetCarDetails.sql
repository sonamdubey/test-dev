IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetCarDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetCarDetails]
GO

	
-- ===============================================================================================================================
-- Author:		Ashwini Dhamankar
-- Create date: 2-01-2015
-- Description:	To fetch Car Data for AbSure Report
-- Modified By : Ashwini Dhamankar on 13-01-2015, Added join on CarTransmission and CarFuelTypes and  case on RegistrationType
-- Modified By : Ashwini Dhamankar on 16-01-2015, Added join on AbSure_CarPhotos to fetch Car Image
-- Modified By : Ashwini Dhamankar on 29-01-2015, Added Case on ImageURL
-- Modified By : Suresh Prajapati on 05th May, 2015
-- Description : Modified to get only thumb url image in Absure Car report
-- EXEC [dbo].[AbSure_GetCarDetails] 470
-- Modified By : ruchira Patil on 3rd Jun 2015 (fethed the car name from carmake table instead of view to get the cars of discontinued versions also)
-- Modified By : ruchira Patil on 11th Jun 2015 (fetch final warranty of the car and car condition and CarFittedWith)
-- Modified By : Suresh Prajapati on 16th June, 2015
-- Description : Fetch RegistrationType from Absure_RegistrationTypes table
-- Modified By : Kartik Rathod on 28th July 2015, Fetch status and IsWarrantyExpired From AbSure_CarDetails
-- Modified By : Vinay Kumar Prajapati  31th July 2015   Added MakeId , ModelId , VersionId 
-- ===============================================================================================================================
CREATE PROCEDURE [dbo].[AbSure_GetCarDetails]
	-- Add the parameters for the stored procedure here
	@AbSure_CarDetailsId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	SELECT --V.Car,
		CMK.NAME + ' ' + CML.NAME + ' ' + CV.NAME AS Car
		,CMK.ID AS MakeId
		,CML.ID AS ModelId
		,CV.ID AS VersionId

		,ABC.RegNumber
		,ABC.Kilometer
		,ABC.RegisteredAt
		,ABC.MakeYear
		,ABC.Owners
		,ABC.Colour
		,ABC.RegistrationDate
		,ABC.Insurance
		,ABC.InsuranceExpiry
		,ABC.IsBankHypothecation
		,ABC.AvailableAt
		,CT.Descr AS Transmission
		,CFT.Descr FuelType
		,ABC.IsOrigionalRC
		,RT.Category AS RegistrationType
		,ABC.Make AS CarMake
		,ABC.Model AS CarModel
		,ABC.Version AS CarVersion
		,D.FirstName + ' ' + D.LastName AS DealerName
		,D.MobileNo AS DealerContactNo
		,A.Name AS Area
		--,CASE ABC.RegistrationType
		--	WHEN 1
		--		THEN 'Dealer'
		--	WHEN 2
		--		THEN 'Individual'
		--	END AS RegistrationType
		,ISNULL('http://' + CP.HostUrl + CP.DirectoryPath + CP.ImageUrlThumb, CASE 
				WHEN ABC.ImgThumbUrl = 'http://' + CP.HostUrl + CP.DirectoryPath + CP.ImageUrlThumb
					AND CP.IsMain = 1
					AND CP.IsActive = 1
					THEN 'http://' + CP.HostUrl + CP.DirectoryPath + CP.ImageUrlThumb
				ELSE 'http://' + CP.HostUrl + CP.DirectoryPath + CP.ImageUrlThumb
				END) AS ImageURL
		,ABC.SurveyDate DateOfReport
		,ABC.CarScore
		,(
			SELECT Parameter
			FROM AbSure_ConfigurableParameters WITH (NOLOCK)
			WHERE ABC.CarScore BETWEEN MinValue
					AND MaxValue
				AND Category LIKE '%Car Conditions%'
			) CarCondition
		,ABC.DealerId
		,ABC.CarFittedWith AS CarFittedWith
		,ABC.FinalWarrantyTypeId FinalWarrantyTypeId
		,WT.Warranty Warranty
		,(
			SELECT Parameter
			FROM AbSure_ConfigurableParameters WITH (NOLOCK)
			WHERE ABC.Kilometer BETWEEN MinValue
					AND MaxValue
				AND Category LIKE '%Kilometer Factor%'
			) KMParameter
		,(
			SELECT Parameter
			FROM AbSure_ConfigurableParameters WITH (NOLOCK)
			WHERE (DATEDIFF(MONTH, ABC.MakeYear, GETDATE())) BETWEEN MinValue
					AND MaxValue
				AND Category LIKE '%Age Factor%'
			) AgeParameter
		,ISNULL(ABC.CarFittedWith, 0) CarFittedWith
					-- Modified By : Kartik Rathod on 28th July 2015, Fetch status and IsWarrantyExpired From AbSure_CarDetails
		,ABC.status
		,CASE WHEN (DATEDIFF(DAY,SurveyDate,GETDATE()) > 30) THEN 1 ELSE 0 END IsWarrantyExpired
	FROM AbSure_CarDetails ABC WITH (NOLOCK)
	INNER JOIN CarVersions CV WITH (NOLOCK) ON CV.ID = ABC.VersionId
	INNER JOIN CarModels CML WITH (NOLOCK) ON CML.ID = CV.CarModelId
	INNER JOIN CarMakes CMK WITH (NOLOCK) ON CMK.ID = CML.CarMakeId
	INNER JOIN Absure_RegistrationTypes AS RT WITH (NOLOCK) ON RT.RegistrationTypeId=ISNULL(ABC.RegistrationType,1)
	--INNER JOIN vwAllMMV V WITH (NOLOCK) ON V.VersionId = ABC.VersionId
	--	AND ApplicationId = 1
	LEFT JOIN CarTransmission CT WITH (NOLOCK) ON CT.Id = ABC.Transmission
	LEFT JOIN CarFuelTypes CFT WITH (NOLOCK) ON CFT.CarFuelTypeId = ABC.FuelType
	LEFT JOIN AbSure_CarPhotos CP WITH (NOLOCK) ON CP.AbSure_CarDetailsId = ABC.Id
		AND CP.IsMain = 1
		AND CP.IsActive = 1 
		AND CP.AbSure_CarDetailsId=@AbSure_CarDetailsId
	LEFT JOIN AbSure_WarrantyTypes WT WITH (NOLOCK) ON WT.AbSure_WarrantyTypesId = ABC.FinalWarrantyTypeId
	LEFT JOIN Dealers D WITH (NOLOCK) ON ABC.DealerId = D.ID
	LEFT JOIN Areas A WITH(NOLOCK) ON D.AreaId = A.ID
	WHERE ABC.Id = @AbSure_CarDetailsId
END

