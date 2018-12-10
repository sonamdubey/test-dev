IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetSSRSReport]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetSSRSReport]
GO

	-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date: Aug 13,2015
-- Description: To Get SSRS report
-- Modified by: Ashwini Dhamankar on Sep 10,2015 (Removed constraint of isactive=1)
-- Modified By : Ashwini Dhamankar on Sep 11,2015 (added time to startdate and enddate)
-- Modified By : Kartik Rathod on 21 Sep 2015 , added Display condition in Case of Duplicate Car and Change in View Report for same case
-- Modified by : Nilima More on 25th sept 2015 (Added condition for Doubtfull) 
-- EXEC [AbSure_GetSSRSReport] '2015-01-25 12:16:56.730','2015-09-25 13:38:45.963'
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetSSRSReport] 
	 @StartDate DATETIME,
     @EndDate DATETIME
AS
BEGIN
		DECLARE @StartDateTime DATETIME = @StartDate + '00:00:00'
		DECLARE @EndDateTime DATETIME = @EndDate + '23:59:59'

		DECLARE @WhileLoopControl INT =1
		DECLARE @WhileLoopCount INT 
		DECLARE @AgencyId INT ,@CityId INT,@CityName VARCHAR(100),@CancelledReason VARCHAR(250)

		CREATE TABLE #SurveyorDetails (Id INT IDENTITY(1,1),CityId INT,CityName VARCHAR(100),AgencyId INT,SurveyorId INT)
		CREATE TABLE #SurveyorId (SurveyorId INT)

		SELECT @CancelledReason = Reason FROM AbSure_ReqCancellationReason WHERE Id = 7      -- Cancellation reason of duplicate car
		

		SELECT                    IDENTITY(INT,1,1) AS ID,
		                          CT.ID CityId, 
								  CT.Name CityName,
								  U.ID    AgencyId
        INTO #AgencyDetails
		FROM TC_Users U WITH (NOLOCK)
		INNER JOIN Cities AS CT WITH (NOLOCK) ON U.CityId=CT.ID
		INNER JOIN Dealers D    WITH (NOLOCK)  ON D.ID = U.BranchId
		WHERE D.TC_DealerTypeId=4
		AND U.IsAgency = 1
		AND U.IsActive = 1

		 SELECT @WhileLoopCount=COUNT(*) FROM #AgencyDetails

		 WHILE (@WhileLoopCount>=@WhileLoopControl)
		 BEGIN
			 SELECT @AgencyId=AgencyId,@CityId=CityId,@CityName=CityName FROM #AgencyDetails WHERE ID=@WhileLoopControl
           
			 INSERT INTO #SurveyorId
			 EXEC TC_GetImmediateChild @AgencyId;

			  INSERT INTO #SurveyorDetails (CityId,
										   CityName,
										   AgencyId,
										   SurveyorId)
								SELECT   @CityId,
										 @CityName,
										 @AgencyId,
										 @AgencyId
								
			 INSERT INTO #SurveyorDetails (CityId,
										   CityName,
										   AgencyId,
										   SurveyorId)
								SELECT   @CityId,
										 @CityName,
										 @AgencyId,
										 SurveyorId
								FROM #SurveyorId

			  TRUNCATE TABLE #SurveyorId

			 SET @WhileLoopControl=@WhileLoopControl+1;
		 END;
					WITH CTE AS 
					(SELECT		ACD.id as ACDId,CASE ACD.RejectionMethod 
									WHEN 1 THEN 'Auto' 
									ELSE 'Manual' 
									END RejectionMethod,
									CONVERT(VARCHAR,ISNULL(ACD.RejectedDateTime,ACD.SurveyDate),106) RejectionDate, 
									ARCR.RejectedType RejectionType,
									CASE ARCR.RejectedType 
									WHEN 1 THEN ARR.Reason 
									ELSE AQC.Category + '-' + AQS.SubCategory 
									END RejectionReason
						FROM		Absure_RejectedCarReasons ARCR	WITH(NOLOCK)
						LEFT JOIN	AbSure_CarDetails ACD			WITH(NOLOCK) ON ACD.Id = ARCR.Absure_CarDetailsId
						LEFT JOIN	AbSure_RejectionReasons ARR		WITH(NOLOCK) ON ARR.Id = ARCR.RejectedReason AND ARCR.RejectedType=1
						LEFT JOIN	AbSure_QSubCategory AQS			WITH(NOLOCK) ON AQS.AbSure_QSubCategoryId = ARCR.RejectedReason AND ARCR.RejectedType=2
						LEFT JOIN	AbSure_QCategory AQC			WITH(NOLOCK) ON AQC.AbSure_QCategoryId = AQS.AbSure_QCategoryId
					), CTE1 AS
							  (SELECT AbsureCarId,
									   count(id) NoOfReschedule, 
									   Max(ScheduledDate) LastRescheduleDate
								FROM Absure_Appointments WITH (NOLOCK)
								GROUP BY AbsureCarId)

					SELECT D.Organization,C.Name DealerCity,A.Name DealerArea,C1.Name OwnerCity,A1.Name OwnerArea,
							CM.Name AS Make,CMO.Name AS Model,vw.name AS Version,
						   AC.EntryDate AS InspectionRequested,
						   S.EntryDate AS InspectionScheduled,
						   IsRejected,RejectedDateTime,IsSurveyDone,SurveyDate,FinalWarrantyDate as ApprovedDate,IsSoldOut,Warranty,
						   CASE WHEN WS.Status = 'Cancelled' AND AC.CancelReason = @CancelledReason THEN 'Duplicate Inspection' ELSE WS.Status END Status,
						   AC.CancelReason,AC.CancelledOn,TC.UserName AS CancelledBY,TD.DealerType
						   ,LEFT(isnull(STUFF ((SELECT ', '+ RejectionReason FROM CTE WHERE CTE.ACDId=ac.id FOR XML PATH('')),1,1,''),''),500) 'RejectionReason'
						   ,(SELECT TOP 1 RejectionMethod FROM CTE WHERE CTE.ACDId=ac.id ) 'RejectionMethod'
						   ,(SELECT TOP 1 EntryDate  FROM AbSure_DoubtfulCarReasons  WHERE AC.Id = ADC.AbSure_CarDetailsId AND ADC.IsActive = 1 order by EntryDate desc) 'OnHoldDate'
						   ,CTE1.LastRescheduleDate AS LastRescheduleDate
						   ,CTE1.NoOfReschedule AS NoOfReschedule
						   ,U.UserName  AS AgencyName,U1.UserName AS SurveyorName
						   ,'http://www.autobiz.in/absure/carcertificate.aspx?carid='+ +ISNULL(cast(dbo.Absure_GetMasterCarId(AC.RegNumber,AC.Id) AS varchar(1000)),AC.Id) ViewReport 
					FROM [dbo].[AbSure_CarDetails] AS AC WITH (NOLOCK)
					LEFT JOIN TC_Users  AS TC WITH (NOLOCK) ON TC.Id=AC.CancelledBy
					JOIN Dealers AS D WITH (NOLOCK) ON AC.DealerId=D.Id
					JOIN Areas AS A WITH (NOLOCK) ON A.ID=D.AreaId
					JOIN Cities AS C WITH (NOLOCK) ON C.id=D.CityId
					JOIN Areas as A1 WITH (NOLOCK) ON A1.ID=AC.OwnerAreaId
					JOIN Cities AS C1 WITH (NOLOCK) ON C1.id=AC.OwnerCityId
					JOIN CarVersions AS vw WITH (NOLOCK)  ON vw.Id=AC.VersionId
					JOIN CarModels AS CMO WITH (NOLOCK) ON CMO.id=vw.CarModelId
					JOIN CarMakes AS CM  WITH (NOLOCK) ON CM.Id=CMO.CarMakeId
					LEFT JOIN [dbo].[AbSure_CarSurveyorMapping] AS S WITH (NOLOCK)  ON AC.Id=S.AbSure_CarDetailsId
					LEFT JOIN [dbo].[AbSure_WarrantyTypes] AS W WITH (NOLOCK) ON W.AbSure_WarrantyTypesId=AC.FinalWarrantyTypeId
					LEFT JOIN Absure_WarrantyStatuses AS WS WITH (NOLOCK) ON WS.Id=AC.Status
					LEFT JOIN TC_Dealertype AS TD WITH (NOLOCK)
							ON D.TC_DealerTypeId = TD.TC_DealerTypeId
								AND TD.TC_DealerTypeId IN (1, 3, 5)
					LEFT JOIN CTE1 ON CTE1.AbsureCarId=AC.Id
					LEFT JOIN AbSure_CarSurveyorMapping AS CS WITH (NOLOCK) ON CS.AbSure_CarDetailsId=AC.Id
					LEFT JOIN #SurveyorDetails AS Sv WITH (NOLOCK) ON Sv.SurveyorId=CS.TC_UserId
					LEFT JOIN TC_Users  AS U WITH (NOLOCK) ON U.Id=Sv.AgencyId
					LEFT JOIN TC_Users AS U1 WITH (NOLOCK) ON U1.Id = Sv.SurveyorId AND U1.IsAgency <> 1
					LEFT JOIN AbSure_DoubtfulCarReasons ADC ON AC.Id = ADC.AbSure_CarDetailsId
					WHERE D.Id NOT IN (4271,3838)
					AND AC.EntryDate BETWEEN @StartDateTime AND @EndDateTime;

        DROP TABLE #SurveyorDetails
		DROP TABLE #SurveyorId
		DROP TABLE #AgencyDetails
END
