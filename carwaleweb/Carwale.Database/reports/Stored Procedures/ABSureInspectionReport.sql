IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[reports].[ABSureInspectionReport]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [reports].[ABSureInspectionReport]
GO

	
-- =======================================================================================
-- Author:		Manish Chourasiya
-- Create date: 12 June 2015
-- Description:	SP for ABsure Inspection Report
-- ========================================================================================
 CREATE PROCEDURE [reports].[ABSureInspectionReport]    
AS
	BEGIN

	DECLARE @MinimumInspection FLOAT=5.000;
	DECLARE @MaximumInspection FLOAT=10.000;

	  DECLARE @WhileLoopControl INT =1
		DECLARE @WhileLoopCount INT 
		DECLARE @AgencyId INT ,@CityId INT,@CityName VARCHAR(100)

		CREATE TABLE #SurveyorDetails (Id INT IDENTITY(1,1),CityId INT,CityName VARCHAR(100),AgencyId INT,SurveyorId INT)
		CREATE TABLE #SurveyorId (SurveyorId INT)
		

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



	  WITH CTE1 AS 
	              (
					SELECT CityId, 
						   CityName,
						   COUNT (DISTINCT AgencyId ) AgencyCount,
						   COUNT (DISTINCT SurveyorId ) SurveyerCount
					FROM #SurveyorDetails
					GROUP BY CityId,CityName
                  ),
			CTE2 AS (
						SELECT CT.ID CityId
							   ,COUNT(DISTINCT(CASE WHEN ISNULL(CD.Status,0) <>3
													 AND (CD.IsSurveyDone IS NULL 
																		OR CD.IsSurveyDone=0 
														  ) 
														   THEN CD.Id END
											  )
									  )  TotalPendingInspectionTillNow
							   ,COUNT(DISTINCT(CASE WHEN ISNULL(CD.Status,0) <>3
													AND  CD.IsSurveyDone=1 
													AND CD.SurveyDate IS NOT NULL 
													AND  CONVERT(DATE,CD.SurveyDate)=CONVERT(DATE,GETDATE()-1) 
													THEN CD.Id END 
											  )
									 ) TotalInsepectionDoneYesterday
							   ,COUNT(DISTINCT(CASE WHEN ISNULL(CD.Status,0) <>3
													AND  CD.IsSurveyDone=1 
													AND  CD.SurveyDate IS NOT NULL 
													AND  MONTH(CD.SurveyDate)=MONTH(GETDATE()-1) 
													AND  YEAR(CD.SurveyDate)=YEAR(GETDATE()-1) 
													THEN CD.Id END 
											  )
									 ) TotalInsepectionDoneMTD
						 FROM       AbSure_CarDetails         AS CD WITH (NOLOCK)
						 LEFT JOIN AbSure_CarSurveyorMapping AS CS WITH (NOLOCK) ON CS.AbSure_CarDetailsId=CD.Id
						 LEFT JOIN #SurveyorDetails          AS U  WITH (NOLOCK) ON U.SurveyorId=CS.TC_UserId
						 LEFT JOIN Cities                    AS CT WITH (NOLOCK) ON U.CityId=CT.ID
						 GROUP BY CT.ID 
		             )  SELECT  CTE1.CityId
					           ,CTE1.CityName
							   ,CTE1.AgencyCount
							   ,CTE1.SurveyerCount "Surveyor+Agency"
							   ,CTE1.SurveyerCount * @MinimumInspection  MinimumInspectionTarget
							   ,CTE1.SurveyerCount * @MaximumInspection  MaximumInspectionTarget
							   ,ISNULL(CTE2.TotalPendingInspectionTillNow,0) TotalPendingInspectionTillNow
							   ,ISNULL(CTE2.TotalInsepectionDoneYesterday,0) TotalInsepectionDoneYesterday
							   ,ISNULL(CTE2.TotalInsepectionDoneMTD,0) TotalInsepectionDoneMTD
							   ,ROUND((ISNULL(CTE2.TotalInsepectionDoneYesterday,0)/(CTE1.SurveyerCount * @MinimumInspection)) *100.000,2) '%UtilizationOfSurveyorsYesterDay'
							   ,ROUND((ISNULL(CTE2.TotalInsepectionDoneMTD,0)/(CTE1.SurveyerCount * @MinimumInspection * DAY ( getdate()-1 ) )) *100.000,2) '%UtilizationOfSurveyorsMTD'
						 FROM  CTE2
					  	LEFT JOIN CTE1 ON CTE1.CityId=CTE2.CityId
						ORDER BY  ISNULL(CTE2.cityid,1000000) ;
						

        DROP TABLE #SurveyorDetails
		DROP TABLE #SurveyorId
		DROP TABLE #AgencyDetails

	END 