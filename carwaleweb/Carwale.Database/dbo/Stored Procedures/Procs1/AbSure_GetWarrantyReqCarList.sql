IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetWarrantyReqCarList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetWarrantyReqCarList]
GO

	
-- =============================================
-- Author:		Tejashree Patil	
-- Create date: 12/12/2014
-- Description:	Get car list warranty requested.
-- [AbSure_GetWarrantyReqCarList] 5,13191,1,91
-- Modified By : Tejashree Patil on 6 Feb 2015 on 11 am, fetched individual(camp) cars also.
-- Modified By : Tejashree Patil on 16 March 2015 on 11 am, fetched owner details.
-- Modified By : Vinay Kumar Prajapati  on 25th March
-- Modified By : Yuga Hatolkar on 15th April, 2015 Changed AvailableAt Field
-- Modified By : Suresh Prajapati on 15th June, 2015
-- Description : Fetched RegistraionType from Absure_RegistrationTypes table
-- Modified  By : vinay Kumar Prajapati to Merge in Existing  Sp (Whole Sp Was Change... )
-- Modified By  : Ashwini Dhamankar on June 23,2015, Fetched Latest App Version
-- Modified By  : Nilima More on July 17,2015, order by appointment time and date has been done.
-- Modified By : Ashwini Dhamankar on July 20,2015 , Fetched VersionArray and ColorArray
--2015-07-28 19:55:41.293
--12:00:00.0000000
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetWarrantyReqCarList] 
@BranchId	INT , 
@UserId		INT ,
@CarStatusTypeId    INT = 1, -- 1  for pending , 2 for  Rescheduled(but inspection not done ), 3 for FutureAppointment, 4 for cancel on last 10 days
@ApplicationType SMALLINT = NULL
AS

BEGIN
--Dealer car list Details   
	
	IF @CarStatusTypeId  =  1    -- existing car  pending
	BEGIN

		--Dealer car list Details   
	--INSERT INTO #TempCarListTbl
	SELECT CD.Id CarId
		,COUNT(AP.AbsureCarId) ScheduledCount
	INTO #TempCarListTbl
	FROM AbSure_CarDetails CD WITH (NOLOCK)
	INNER JOIN Absure_Appointments AP WITH (NOLOCK) ON AP.AbsureCarId = CD.Id
	GROUP BY CD.Id
		,AP.AbsureCarId

	SELECT CD.Id CarId
		,CD.Make MakeName
		,CD.Model ModelName
		,V.ModelId ModelId
		,CD.Version
		,CD.VersionId 
		,dbo.[AbSure_GetVersions](ModelId) AS VersionArray
		,dbo.[AbSure_GetVersionColors](CD.VersionId) AS ColorArray
		,CD.VIN
		,CD.RegNumber RegistrationNo
		,CD.Source
		,CD.StockId
		,CD.MakeYear
		,CD.Kilometer
		,CD.Owners
		,CD.Colour
		,CD.Insurance
		,CD.InsuranceExpiry
		,CD.IsOrigionalRC
		,CD.IsBankHypothecation
		,CD.RegisteredAt
		,ISNULL(CD.AvailableAt, A.NAME) AvailableAt
		,ISNULL(CD.FuelType, V.CarFuelType) FuelType
		,CD.Transmission
		,CD.RegistrationDate
		--,CD.RegistrationType
		,RT.RegistrationTypeId AS RegistrationType
		,CD.ImgLargeUrl LargeImage
		,CD.ImgThumbUrl ThumbImage
		,ST.Price
		,CD.ModifiedDate LastUpdatedDate
		,V.ModelId
		,V.MakeId
		,ISNULL(CD.OwnerAddress, 'address') OwnerAddress
		,CD.OwnerName
		,CD.OwnerPhoneNo
		,CD.OwnerEmail
		,CD.OwnerCityId
		,CD.OwnerAreaId
		,C.NAME OwnerCity
		,A.NAME OwnerArea
		,CONVERT(DATE,ISNULL(CD.AppointmentDate, CD.EntryDate)) AppointmentDate
		--CAST
		--(
		,(
		CASE WHEN CD.AppointmentTime LIKE '%First Half%' OR CD.AppointmentTime LIKE '%09:00%' THEN '09:00:00'   
		ELSE CASE WHEN CD.AppointmentTime LIKE '%Second Half%' OR CD.AppointmentTime LIKE '%03:00%' OR CD.AppointmentTime LIKE '%15:00%' THEN '15:00:00'
		ELSE CASE WHEN CD.AppointmentTime LIKE '%12:00%' THEN '12:00:00'
		ELSE '09:00:00'
		END
		END
		END
		)
		--AS TIME
		--)
		 AppointmentTime, 
		 CarList.ScheduledCount AS ScheduledCount
	FROM AbSure_CarDetails CD WITH (NOLOCK)
	INNER JOIN TC_Stock ST WITH (NOLOCK) ON ST.Id = ISNULL(CD.StockId, NULL)
	INNER JOIN AbSure_CarSurveyorMapping CS WITH (NOLOCK) ON CS.TC_StockId = CD.StockId
	INNER JOIN vwMMV V WITH (NOLOCK) ON V.VersionId = CD.VersionId
	INNER JOIN Cities C WITH (NOLOCK) ON C.ID = CD.OwnerCityId
	INNER JOIN Absure_RegistrationTypes AS RT WITH (NOLOCK) ON RT.RegistrationTypeId = ISNULL(CD.RegistrationType, 1)
	LEFT JOIN Areas A WITH (NOLOCK) ON A.ID = CD.OwnerAreaId
	LEFT JOIN Absure_Appointments AS AP WITH (NOLOCK) ON AP.AbsureCarId = CD.Id
	LEFT JOIN #TempCarListTbl AS CarList ON CarList.CarId = CD.Id
	--LEFT  JOIN AbSure_CarPhotos			 CP	WITH(NOLOCK) ON CP.AbSure_CarDetailsId = CD.Id AND CP.IsMain = 1
	WHERE CS.TC_UserId = @userId
		--AND CS.BranchId = @BranchId
		AND CS.TC_UserId = @UserId
		AND ST.BranchId = @BranchId
		AND StockId IS NOT NULL
		AND ISNULL(CD.IsRejected, 0) <> 1
		AND ISNULL(CD.IsSurveyDone, 0) = 0
		AND ISNULL(CD.STATUS, 0) <> 3
		AND ISNULL(CD.IsActive, 1) = 1 -- Modified By Tejashree Patil on 13 March 2015

	UNION
	--INDIVIDUAL car list Details
	SELECT DISTINCT CD.Id CarId
		,CD.Make MakeName
		,CD.Model ModelName
		,V.ModelId ModelId
		,CD.Version
		,CD.VersionId 
		,dbo.[AbSure_GetVersions](ModelId) AS VersionArray
		,dbo.[AbSure_GetVersionColors](CD.VersionId) AS ColorArray
		,CD.VIN
		,CD.RegNumber RegistrationNo
		,CD.Source
		,CD.StockId
		,CD.MakeYear
		,CD.Kilometer
		,CD.Owners
		,CD.Colour
		,CD.Insurance
		,CD.InsuranceExpiry
		,CD.IsOrigionalRC
		,CD.IsBankHypothecation
		,CD.RegisteredAt
		,ISNULL(CD.AvailableAt, A.NAME) AvailableAt
		,ISNULL(CD.FuelType, V.CarFuelType) FuelType
		,CD.Transmission
		,CD.RegistrationDate
		--,CD.RegistrationType
		,RT.RegistrationTypeId AS RegistrationType
		,CD.ImgLargeUrl LargeImage
		,CD.ImgThumbUrl ThumbImage
		,0 Price
		,CD.ModifiedDate LastUpdatedDate
		,V.ModelId
		,V.MakeId
		,ISNULL(CD.OwnerAddress, 'address') OwnerAddress
		,CD.OwnerName
		,CD.OwnerPhoneNo
		,CD.OwnerEmail
		,CD.OwnerCityId
		,CD.OwnerAreaId
		,C.NAME OwnerCity
		,A.NAME OwnerArea
		,CONVERT(DATE,ISNULL(CD.AppointmentDate, CD.EntryDate)) AppointmentDate
		
		--CAST
		--(
		,(
		CASE WHEN CD.AppointmentTime LIKE '%First Half%' OR CD.AppointmentTime LIKE '%09:00%' THEN '09:00:00'
		ELSE CASE WHEN CD.AppointmentTime LIKE '%Second Half%' OR CD.AppointmentTime LIKE '%03:00%' OR CD.AppointmentTime LIKE '%15:00%' THEN '15:00:00'
		ELSE CASE WHEN CD.AppointmentTime LIKE '%12:00%' THEN '12:00:00'
		ELSE '09:00:00'
		END
		END
		END
		)
		--AS TIME
		--) 
		AppointmentTime,
		 CarList.ScheduledCount AS ScheduledCount
	FROM AbSure_CarDetails CD
	INNER JOIN AbSure_CarSurveyorMapping CS WITH (NOLOCK) ON CD.Id = CS.AbSure_CarDetailsId --CS.BranchId = CD.DealerId AND CS.TC_StockId IS NULL
	INNER JOIN vwMMV V WITH (NOLOCK) ON V.VersionId = CD.VersionId
	INNER JOIN Cities C WITH (NOLOCK) ON C.ID = CD.OwnerCityId
	INNER JOIN Absure_RegistrationTypes AS RT WITH (NOLOCK) ON RT.RegistrationTypeId = ISNULL(CD.RegistrationType, 1)
	LEFT JOIN Areas A WITH (NOLOCK) ON A.ID = CD.OwnerAreaId
	LEFT JOIN Absure_Appointments AS AP WITH (NOLOCK) ON AP.AbsureCarId = CD.Id
	LEFT JOIN #TempCarListTbl AS CarList ON CarList.CarId = CD.Id
	WHERE CS.TC_UserId = @userId
		AND CS.BranchId = @BranchId
		AND CS.TC_UserId = @UserId
		--AND ST.BranchId = @BranchId
		AND StockId IS NULL
		AND ISNULL(CD.IsRejected, 0) <> 1
		AND ISNULL(CD.IsSurveyDone, 0) = 0
		AND ISNULL(CD.STATUS, 0) <> 3
		AND ISNULL(CD.IsActive, 1) = 1 -- Modified By Tejashree Patil on 13 March 2015		
		ORDER BY AppointmentDate DESC,AppointmentTime 
	  DROP TABLE #TempCarListTbl
	END
	ELSE IF @CarStatusTypeId=2   -- For Rescheduled(But inspection not Done)
	BEGIN
		SELECT	CD.Id CarId
				,CD.Make MakeName
				,CD.Model ModelName
				,V.ModelId ModelId
				,CD.Version
				,CD.VersionId 
				,dbo.[AbSure_GetVersions](ModelId) AS VersionArray
				,dbo.[AbSure_GetVersionColors](CD.VersionId) AS ColorArray
				,CD.VIN
				,CD.RegNumber RegistrationNo
				,CD.Source
				,CD.StockId
				,CD.MakeYear
				,CD.Kilometer
				,CD.Owners
				,CD.Colour
				,CD.Insurance
				,CD.InsuranceExpiry
				,CD.IsOrigionalRC
				,CD.IsBankHypothecation
				,CD.RegisteredAt
				,ISNULL(CD.AvailableAt, A.Name) AvailableAt
				,CD.FuelType
				,CD.Transmission
				,CD.RegistrationDate
				,CD.RegistrationType
				,CD.ImgLargeUrl LargeImage
				,CD.ImgThumbUrl ThumbImage
				,ST.Price
				,CD.ModifiedDate LastUpdatedDate
				,V.ModelId
				,V.MakeId
				,ISNULL(CD.OwnerAddress,'address') OwnerAddress
				,CD.OwnerName
				,CD.OwnerPhoneNo
				,CD.OwnerEmail
				,CD.OwnerCityId
				,CD.OwnerAreaId
				,C.Name OwnerCity
				,A.Name OwnerArea
				,CONVERT(DATE,ISNULL(CD.AppointmentDate, CD.EntryDate)) AppointmentDate
				--CAST
				--(
				,(
				CASE WHEN CD.AppointmentTime LIKE '%First Half%' OR CD.AppointmentTime LIKE '%09:00%' THEN '09:00:00'
				ELSE CASE WHEN CD.AppointmentTime LIKE '%Second Half%' OR CD.AppointmentTime LIKE '%03:00%' OR CD.AppointmentTime LIKE '%15:00%' THEN '15:00:00'
				ELSE CASE WHEN CD.AppointmentTime LIKE '%12:00%' THEN '12:00:00'
				ELSE '09:00:00'
				END
				END
				END
				)
				--AS TIME
				--) 
				AppointmentTime,
				'' AS ScheduledCount   --- There is no need of scheduled count in CarStatusTypeId=2 
				
		FROM	AbSure_CarDetails CD					WITH(NOLOCK)
				INNER JOIN TC_Stock ST					WITH(NOLOCK) ON ST.Id = ISNULL(CD.StockId,NULL)
				INNER JOIN AbSure_CarSurveyorMapping CS WITH(NOLOCK) ON  CS.AbSure_CarDetailsId = CD.Id   --CS.TC_StockId = CD.StockId
				INNER JOIN vwMMV V						WITH(NOLOCK) ON V.VersionId = CD.VersionId
				INNER JOIN Cities C						WITH(NOLOCK) ON C.ID = CD.OwnerCityId
				LEFT  JOIN Areas A						WITH(NOLOCK) ON A.ID = CD.OwnerAreaId		
		WHERE	 
			 CS.TC_UserId = @UserId		        
			 AND ST.BranchId = @BranchId
			 AND StockId IS NOT NULL
			 AND ISNULL(CD.Status,0) =0  			
			 AND ISNULL(CD.IsActive,1) = 1 
			 --AND ISNULL(CD.AppointmentDate,GETDATE())>=GETDATE()  -- Use for Rescheduled
			 AND CD.Id IN(SELECT AAP.AbsureCarId FROM Absure_Appointments AS AAP WITH(NOLOCK) WHERE AAP.UserId=@UserId)
			 
	UNION
	--INDIVIDUAL car list Details
		SELECT DISTINCT 
			CD.Id CarId
				,CD.Make MakeName
				,CD.Model ModelName
				,V.ModelId ModelId
				,CD.Version
				,CD.VersionId 
				,dbo.[AbSure_GetVersions](ModelId) AS VersionArray
				,dbo.[AbSure_GetVersionColors](CD.VersionId) AS ColorArray
				,CD.VIN
				,CD.RegNumber RegistrationNo
				,CD.Source
				,CD.StockId
				,CD.MakeYear
				,CD.Kilometer
				,CD.Owners
				,CD.Colour
				,CD.Insurance
				,CD.InsuranceExpiry
				,CD.IsOrigionalRC
				,CD.IsBankHypothecation
				,CD.RegisteredAt
				,ISNULL(CD.AvailableAt, A.Name) AvailableAt
				,CD.FuelType
				,CD.Transmission
				,CD.RegistrationDate
				,CD.RegistrationType
				,CD.ImgLargeUrl LargeImage
				,CD.ImgThumbUrl ThumbImage
				,0 Price
				,CD.ModifiedDate LastUpdatedDate
				,V.ModelId
				,V.MakeId
				,ISNULL(CD.OwnerAddress,'address') OwnerAddress
				,CD.OwnerName
				,CD.OwnerPhoneNo
				,CD.OwnerEmail
				,CD.OwnerCityId
				,CD.OwnerAreaId
				,C.Name OwnerCity
				,A.Name OwnerArea
				,CONVERT(DATE,ISNULL(CD.AppointmentDate, CD.EntryDate)) AppointmentDate
				--CAST
				--(
				,(
				CASE WHEN CD.AppointmentTime LIKE '%First Half%' OR CD.AppointmentTime LIKE '%09:00%' THEN '09:00:00'   
				ELSE CASE WHEN CD.AppointmentTime LIKE '%Second Half%' OR CD.AppointmentTime LIKE '%03:00%' OR CD.AppointmentTime LIKE '%15:00%' THEN '15:00:00'
				ELSE CASE WHEN CD.AppointmentTime LIKE '%12:00%' THEN '12:00:00'
				ELSE '09:00:00'
				END
				END
				END
				)
				--AS TIME
				--)
				 AppointmentTime,
				'' AS ScheduledCount   --- There is no need of scheduled count in CarStatusTypeId=2 
		FROM    AbSure_CarDetails CD    
				INNER JOIN AbSure_CarSurveyorMapping CS WITH(NOLOCK) ON CD.Id = CS.AbSure_CarDetailsId --CS.BranchId = CD.DealerId AND CS.TC_StockId IS NULL
				INNER JOIN vwMMV V						WITH(NOLOCK) ON V.VersionId = CD.VersionId
				INNER JOIN Cities C						WITH(NOLOCK) ON C.ID = CD.OwnerCityId
				LEFT  JOIN Areas A						WITH(NOLOCK) ON A.ID = CD.OwnerAreaId
		WHERE    CS.TC_UserId = @UserId
				AND CS.BranchId = @BranchId
				AND StockId IS NULL
				AND ISNULL(CD.Status,0) =0 					
				AND ISNULL(CD.IsActive,1) = 1 
				AND CD.Id IN(SELECT AAP.AbsureCarId FROM Absure_Appointments AS AAP WITH(NOLOCK) WHERE AAP.UserId=@UserId) -- Is car Resheduled 
				ORDER BY AppointmentDate DESC,AppointmentTime 

	END
	ELSE IF @CarStatusTypeId=3   -- For  FutureAppointment
	BEGIN
		SELECT	CD.Id CarId
				,CD.Make MakeName
				,CD.Model ModelName
				,V.ModelId ModelId
				,CD.Version
				,CD.VersionId 
				,dbo.[AbSure_GetVersions](ModelId) AS VersionArray
				,dbo.[AbSure_GetVersionColors](CD.VersionId) AS ColorArray
				,CD.VIN
				,CD.RegNumber RegistrationNo
				,CD.Source
				,CD.StockId
				,CD.MakeYear
				,CD.Kilometer
				,CD.Owners
				,CD.Colour
				,CD.Insurance
				,CD.InsuranceExpiry
				,CD.IsOrigionalRC
				,CD.IsBankHypothecation
				,CD.RegisteredAt
				,ISNULL(CD.AvailableAt, A.Name) AvailableAt
				,CD.FuelType
				,CD.Transmission
				,CD.RegistrationDate
				,CD.RegistrationType
				,CD.ImgLargeUrl LargeImage
				,CD.ImgThumbUrl ThumbImage
				,ST.Price
				,CD.ModifiedDate LastUpdatedDate
				,V.ModelId
				,V.MakeId
				,ISNULL(CD.OwnerAddress,'address') OwnerAddress
				,CD.OwnerName
				,CD.OwnerPhoneNo
				,CD.OwnerEmail
				,CD.OwnerCityId
				,CD.OwnerAreaId
				,C.Name OwnerCity
				,A.Name OwnerArea
				,CONVERT(DATE,ISNULL(CD.AppointmentDate, CD.EntryDate)) AppointmentDate
				--CAST
				--(
				,(
				CASE WHEN CD.AppointmentTime LIKE '%First Half%' OR CD.AppointmentTime LIKE '%09:00%' THEN '09:00:00'   
				ELSE CASE WHEN CD.AppointmentTime LIKE '%Second Half%' OR CD.AppointmentTime LIKE '%03:00%' OR CD.AppointmentTime LIKE '%15:00%' THEN '15:00:00'
				ELSE CASE WHEN CD.AppointmentTime LIKE '%12:00%' THEN '12:00:00'
				ELSE '09:00:00'
				END
				END
				END
				)
				--AS TIME
				--)
				AppointmentTime,
				'' AS ScheduledCount   --- There is no need of scheduled count in CarStatusTypeId=3 
		FROM	AbSure_CarDetails CD					WITH(NOLOCK)
				INNER JOIN TC_Stock ST					WITH(NOLOCK) ON ST.Id = ISNULL(CD.StockId,NULL)
				INNER JOIN AbSure_CarSurveyorMapping CS WITH(NOLOCK) ON  CS.AbSure_CarDetailsId = CD.Id     --  CS.TC_StockId = CD.StockId
				INNER JOIN vwMMV V						WITH(NOLOCK) ON V.VersionId = CD.VersionId
				INNER JOIN Cities C						WITH(NOLOCK) ON C.ID = CD.OwnerCityId
				LEFT  JOIN Areas A						WITH(NOLOCK) ON A.ID = CD.OwnerAreaId
				--INNER  JOIN Absure_Appointments AS AP    WITH(NOLOCK) ON AP.AbsureCarId = CD.Id
				--LEFT  JOIN AbSure_CarPhotos			 CP	WITH(NOLOCK) ON CP.AbSure_CarDetailsId = CD.Id AND CP.IsMain = 1
		WHERE	CS.TC_UserId = @UserId
				AND ST.BranchId = @BranchId
				AND StockId IS NOT NULL
				AND ISNULL(CD.Status,0) = 0  --not Rejected , survey not done  , not Cancelled  
				AND ISNULL(CD.AppointmentDate,GETDATE()) > GETDATE()  -- For Future Inspection 
				AND ISNULL(CD.IsActive,1) = 1 

	UNION
	--INDIVIDUAL car list Details
		SELECT DISTINCT 
			CD.Id CarId
				,CD.Make MakeName
				,CD.Model ModelName
				,V.ModelId ModelId
				,CD.Version
				,CD.VersionId 
				,dbo.[AbSure_GetVersions](ModelId) AS VersionArray
				,dbo.[AbSure_GetVersionColors](CD.VersionId) AS ColorArray
				,CD.VIN
				,CD.RegNumber RegistrationNo
				,CD.Source
				,CD.StockId
				,CD.MakeYear
				,CD.Kilometer
				,CD.Owners
				,CD.Colour
				,CD.Insurance
				,CD.InsuranceExpiry
				,CD.IsOrigionalRC
				,CD.IsBankHypothecation
				,CD.RegisteredAt
				,ISNULL(CD.AvailableAt, A.Name) AvailableAt
				,CD.FuelType
				,CD.Transmission
				,CD.RegistrationDate
				,CD.RegistrationType
				,CD.ImgLargeUrl LargeImage
				,CD.ImgThumbUrl ThumbImage
				,0 Price
				,CD.ModifiedDate LastUpdatedDate
				,V.ModelId
				,V.MakeId
				,ISNULL(CD.OwnerAddress,'address') OwnerAddress
				,CD.OwnerName
				,CD.OwnerPhoneNo
				,CD.OwnerEmail
				,CD.OwnerCityId
				,CD.OwnerAreaId
				,C.Name OwnerCity
				,A.Name OwnerArea
				,CONVERT(DATE,ISNULL(CD.AppointmentDate, CD.EntryDate)) AppointmentDate
				--CAST
				--(
				,(
				CASE WHEN CD.AppointmentTime LIKE '%First Half%' OR CD.AppointmentTime LIKE '%09:00%' THEN '09:00:00'   
				ELSE CASE WHEN CD.AppointmentTime LIKE '%Second Half%' OR CD.AppointmentTime LIKE '%03:00%' OR CD.AppointmentTime LIKE '%15:00%' THEN '15:00:00'
				ELSE CASE WHEN CD.AppointmentTime LIKE '%12:00%' THEN '12:00:00'
				ELSE '09:00:00'
				END
				END
				END
				)
				--AS TIME
				--)
				AppointmentTime,
				'' AS ScheduledCount   --- There is no need of scheduled count in CarStatusTypeId=3
		FROM    AbSure_CarDetails CD    
				INNER JOIN AbSure_CarSurveyorMapping CS WITH(NOLOCK) ON CD.Id = CS.AbSure_CarDetailsId --CS.BranchId = CD.DealerId AND CS.TC_StockId IS NULL
				INNER JOIN vwMMV V						WITH(NOLOCK) ON V.VersionId = CD.VersionId
				INNER JOIN Cities C						WITH(NOLOCK) ON C.ID = CD.OwnerCityId
				LEFT  JOIN Areas A						WITH(NOLOCK) ON A.ID = CD.OwnerAreaId
		WHERE    CS.TC_UserId = @UserId
				AND CS.BranchId = @BranchId
				AND StockId IS NULL
				AND ISNULL(CD.Status,0) = 0  
				AND ISNULL(CD.AppointmentDate,GETDATE())> GETDATE()  -- For Future Inspection 
				AND ISNULL(CD.IsActive,1) = 1  
				ORDER BY AppointmentDate DESC,AppointmentTime 
	END
	ELSE IF @CarStatusTypeId=4   --For  cancelled car .. last 10 days
	BEGIN
		SELECT	CD.Id CarId
				,CD.Make MakeName
				,CD.Model ModelName
				,V.ModelId ModelId
				,CD.Version
				,CD.VersionId 
				,dbo.[AbSure_GetVersions](ModelId) AS VersionArray
				,dbo.[AbSure_GetVersionColors](CD.VersionId) AS ColorArray
				,CD.VIN
				,CD.RegNumber RegistrationNo
				,CD.Source
				,CD.StockId
				,CD.MakeYear
				,CD.Kilometer
				,CD.Owners
				,CD.Colour
				,CD.Insurance
				,CD.InsuranceExpiry
				,CD.IsOrigionalRC
				,CD.IsBankHypothecation
				,CD.RegisteredAt
				,ISNULL(CD.AvailableAt, A.Name) AvailableAt
				,CD.FuelType
				,CD.Transmission
				,CD.RegistrationDate
				,CD.RegistrationType
				,CD.ImgLargeUrl LargeImage
				,CD.ImgThumbUrl ThumbImage
				,ST.Price
				,CD.ModifiedDate LastUpdatedDate
				,V.ModelId
				,V.MakeId
				,ISNULL(CD.OwnerAddress,'address') OwnerAddress
				,CD.OwnerName
				,CD.OwnerPhoneNo
				,CD.OwnerEmail
				,CD.OwnerCityId
				,CD.OwnerAreaId
				,C.Name OwnerCity
				,A.Name OwnerArea
				,CONVERT(DATE,ISNULL(CD.AppointmentDate, CD.EntryDate)) AppointmentDate
				--CAST
				--(
				,(
				CASE WHEN CD.AppointmentTime LIKE '%First Half%' OR CD.AppointmentTime LIKE '%09:00%' THEN '09:00:00'   
				ELSE CASE WHEN CD.AppointmentTime LIKE '%Second Half%' OR CD.AppointmentTime LIKE '%03:00%' OR CD.AppointmentTime LIKE '%15:00%' THEN '15:00:00'
				ELSE CASE WHEN CD.AppointmentTime LIKE '%12:00%' THEN '12:00:00'
				ELSE '09:00:00'
				END
				END
				END
				)
				--AS TIME
				--)
				AppointmentTime,
				'' AS ScheduledCount   --- There is no need of scheduled count in CarStatusTypeId=4
		FROM	AbSure_CarDetails CD					WITH(NOLOCK)
				INNER JOIN TC_Stock ST					WITH(NOLOCK) ON ST.Id = ISNULL(CD.StockId,NULL)
				INNER JOIN AbSure_CarSurveyorMapping CS WITH(NOLOCK) ON    CS.AbSure_CarDetailsId = CD.Id   --CS.TC_StockId = CD.StockId
				INNER JOIN vwMMV V						WITH(NOLOCK) ON V.VersionId = CD.VersionId
				INNER JOIN Cities C						WITH(NOLOCK) ON C.ID = CD.OwnerCityId
				LEFT  JOIN Areas A						WITH(NOLOCK) ON A.ID = CD.OwnerAreaId
				
		WHERE	CS.TC_UserId = @UserId
				AND ST.BranchId = @BranchId
				AND StockId IS NOT NULL
				AND ISNULL(CD.CancelledOn,GETDATE()-11) >= GETDATE()-10 -- last 10 days cancelled Cars 
				AND ISNULL(CD.Status,0) = 3 -- for cancelled cars
				AND ISNULL(CD.IsActive,1) = 1 
	UNION
	--INDIVIDUAL car list Details
		SELECT DISTINCT 
			CD.Id CarId
				,CD.Make MakeName
				,CD.Model ModelName
				,V.ModelId ModelId
				,CD.Version
				,CD.VersionId 
				,dbo.[AbSure_GetVersions](ModelId) AS VersionArray
				,dbo.[AbSure_GetVersionColors](CD.VersionId) AS ColorArray
				,CD.VIN
				,CD.RegNumber RegistrationNo
				,CD.Source
				,CD.StockId
				,CD.MakeYear
				,CD.Kilometer
				,CD.Owners
				,CD.Colour
				,CD.Insurance
				,CD.InsuranceExpiry
				,CD.IsOrigionalRC
				,CD.IsBankHypothecation
				,CD.RegisteredAt
				,ISNULL(CD.AvailableAt, A.Name) AvailableAt
				,CD.FuelType
				,CD.Transmission
				,CD.RegistrationDate
				,CD.RegistrationType
				,CD.ImgLargeUrl LargeImage
				,CD.ImgThumbUrl ThumbImage
				,0 Price
				,CD.ModifiedDate LastUpdatedDate
				,V.ModelId
				,V.MakeId
				,ISNULL(CD.OwnerAddress,'address') OwnerAddress
				,CD.OwnerName
				,CD.OwnerPhoneNo
				,CD.OwnerEmail
				,CD.OwnerCityId
				,CD.OwnerAreaId
				,C.Name OwnerCity
				,A.Name OwnerArea
				,CONVERT(DATE,ISNULL(CD.AppointmentDate, CD.EntryDate)) AppointmentDate
				--CAST
				--(
				,(
				CASE WHEN CD.AppointmentTime LIKE '%First Half%' OR CD.AppointmentTime LIKE '%09:00%' THEN '09:00:00'   
				ELSE CASE WHEN CD.AppointmentTime LIKE '%Second Half%' OR CD.AppointmentTime LIKE '%03:00%' OR CD.AppointmentTime LIKE '%15:00%' THEN '15:00:00'
				ELSE CASE WHEN CD.AppointmentTime LIKE '%12:00%' THEN '12:00:00'
				ELSE '09:00:00'
				END
				END
				END
				)
				--AS TIME
				--)
				AppointmentTime,
				'' AS ScheduledCount   --- There is no need of scheduled count in CarStatusTypeId=4
		FROM    AbSure_CarDetails CD    
				INNER JOIN AbSure_CarSurveyorMapping CS WITH(NOLOCK) ON CD.Id = CS.AbSure_CarDetailsId --CS.BranchId = CD.DealerId AND CS.TC_StockId IS NULL
				INNER JOIN vwMMV V						WITH(NOLOCK) ON V.VersionId = CD.VersionId
				INNER JOIN Cities C						WITH(NOLOCK) ON C.ID = CD.OwnerCityId
				LEFT  JOIN Areas A						WITH(NOLOCK) ON A.ID = CD.OwnerAreaId
				--LEFT  JOIN Absure_Appointments AS AP    WITH(NOLOCK) ON AP.AbsureCarId = CD.Id
		WHERE    CS.TC_UserId = @UserId
				AND CS.BranchId = @BranchId
				AND StockId IS NULL      --- clearify it 
				AND ISNULL(CD.CancelledOn,GETDATE()-11) >= GETDATE()-10 -- last 10 days cancelled Cars 
				AND ISNULL(CD.Status,0) = 3 -- for cancelled cars
				AND ISNULL(CD.IsActive,1) = 1 
				ORDER BY AppointmentDate DESC,AppointmentTime 
	END

	SELECT		TOP 1 VersionId 
	FROM		WA_AndroidAppVersions WITH(NOLOCK) 
	WHERE       ApplicationType = @ApplicationType
				AND IsLatest = 1
				ORDER BY Id DESC
END

