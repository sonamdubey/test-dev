IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_GetCarSurveyorMappingReport]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_GetCarSurveyorMappingReport]
GO

	

-- =============================================
-- Author      : Vinay Kumar Prajapti
-- Create date : 29th June 2015
-- Description : To Fetch absure car surveyor mapping data for Approval done and Approval pending 
-- Modified By : Ashwini Dhamankar Added alias GuaranteeStatus for WarrantyStatus for warranty
-- Modified by : Ruchira Patil on 5th Aug 2015 (approval pending fetch completed inspection data(with Rc Image))
-- Modified By : Nilima More on 23rd sept 2015 
-- Description : For Finance Axa And CwAxa Approval on Hold data in report.
-- =============================================
CREATE PROCEDURE [dbo].[Absure_GetCarSurveyorMappingReport]
	@BranchId        INT = NULL,
	@CityId          INT,
	@AreaId          INT = NULL,
	@IsSurveyDone    BIT,
	@LoggedInUser	 INT,
	@PendingStatus	 INT,   -- @PendingStatus= 5 for approval done , @PendingStatus = 4 means Approval pending , @PendingStatus = 7 OnHOld
	@StartDate       DATETIME = NULL,                    
	@EndDate         DATETIME = NULL                      
AS
BEGIN
	
	SET NOCOUNT ON
		DECLARE @IsAxaAgency BIT	

		IF (SELECT DISTINCT RoleId FROM TC_UsersRole WHERE UserId = @LoggedInUser AND RoleId = 15) = 15
		BEGIN	
			SET @IsAxaAgency = 1
			DECLARE @TblTempUsers TABLE (Id INT)
			INSERT INTO @TblTempUsers(Id) VALUES (@LoggedInUser)
			INSERT INTO @TblTempUsers(Id) EXEC TC_GetImmediateChild @LoggedInUser
		END;

     IF @IsAxaAgency=1
	 BEGIN
		SELECT  C.Name AS City,A.Name AS Area, D.Organization AS DealerName ,(SELECT TCU.UserName  FROM TC_Users AS TCU WITH(NOLOCK) WHERE Id= @LoggedInUser) AS AgencyName,TU.UserName AS SurveyorName,TU.Email AS Login_Id,ACD.Make,ACD.Model,ACD.Version,
		REPLACE(REPLACE(ACD.RegNumber,CHAR(10),' '),CHAR(13),' ') AS RegNumber,
		CASE WHEN IsSurveyDone = 1 THEN CONVERT(VARCHAR,ACD.SurveyDate,106) ELSE CONVERT(VARCHAR,ACD.EntryDate,106) END AS SurveyDate		
		FROM Dealers D WITH(NOLOCK) 
		    RIGHT JOIN AbSure_CarDetails ACD WITH(NOLOCK) ON D.ID = ACD.DealerId 
			LEFT JOIN AbSure_CarSurveyorMapping CSMP WITH(NOLOCK) ON ACD.Id= CSMP.AbSure_CarDetailsId 
			LEFT JOIN Areas A WITH(NOLOCK) ON A.ID = ACD.OwnerAreaId
			LEFT JOIN Cities AS C WITH(NOLOCK) ON C.ID=ACD.OwnerCityId
			LEFT JOIN TC_Users TU WITH(NOLOCK) ON TU.Id = CSMP.TC_UserId
			WHERE
				 ((@CityId IS NULL AND @IsAxaAgency=1) OR ACD.OwnerCityId = @CityId)  AND 
					((@AreaId = 0 AND ACD.OwnerAreaId IN (0,-1)) OR @AreaId IS NULL OR ACD.OwnerAreaId = @AreaId)
				AND (
					(CSMP.TC_UserId IN(SELECT ID FROM @TblTempUsers))
					OR
					((SELECT DISTINCT RoleId FROM TC_UsersRole WHERE UserId = @LoggedInUser AND RoleId IN(9, 14))IN(9,14) 
					AND ISNULL(CSMP.TC_UserId,0) = ISNULL(CSMP.TC_UserId,0)
					)
					)
					AND(
					(@PendingStatus = 4 AND ISNULL(ACD.IsSurveyDone,0) = 1 AND
					 ACD.FinalWarrantyTypeId IS NULL AND ISNULL(ACD.Status,0) NOT IN ( 3,9) AND 
					 (ACD.IsRejected = 0 OR  ACD.IsRejected is NULL) 
					 AND CAST(ACD.SurveyDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE) 
					 AND (ISNULL(RCImagePending,0) = 0)) -- Approval Pending 

					OR (@PendingStatus = 5 AND ISNULL(ACD.IsSurveyDone,0) = 1 AND
					 (ACD.FinalWarrantyTypeId IS NOT NULL OR ACD.IsRejected = 1) AND 
					 ISNULL(ACD.Status,0) <> 3 AND CAST(ACD.SurveyDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND
					  CAST(@EndDate AS DATE)) -- Approval Done

					)
		       ORDER BY ACD.Make,ACD.Model,ACD.Version
	   END
	ELSE   -- Finance Axa And CwAxa
	   BEGIN

	   		SELECT  C.Name AS City,A.Name AS Area, D.Organization AS DealerName ,			
		    (SELECT UserName FROM TC_Users WITH(NOLOCK) WHERE HierId =  TU.HierId.GetAncestor(1) AND BranchId =(SELECT BranchId FROM TC_Users WHERE Id=@LoggedInUser)  /* 14168 11165*/ ) AS AgencyName,
			TU.UserName AS SurveyorName,
			TU.Email AS Login_Id,ACD.Make,ACD.Model,ACD.Version,
			 'http://www.autobiz.in/absure/carcertificate.aspx?carid='+CONVERT(VARCHAR,ACD.Id) AS  ReportLink , ISNULL(ACD.CarScore,'') AS CarScore, ISNULL(WS.Status, '') AS GuaranteeStatus ,
		REPLACE(REPLACE(ACD.RegNumber,CHAR(10),' '), CHAR(13),' ') AS RegNumber,
		CASE WHEN IsSurveyDone = 1 THEN CONVERT(VARCHAR,ACD.SurveyDate,106) ELSE CONVERT(VARCHAR,ACD.EntryDate,106) END AS SurveyDate		
		FROM Dealers D WITH(NOLOCK) 
		    RIGHT JOIN AbSure_CarDetails ACD WITH(NOLOCK) ON D.ID = ACD.DealerId 
			LEFT JOIN AbSure_CarSurveyorMapping CSMP WITH(NOLOCK) ON ACD.Id= CSMP.AbSure_CarDetailsId 
			LEFT JOIN Areas A WITH(NOLOCK) ON A.ID = ACD.OwnerAreaId
			LEFT JOIN Cities AS C WITH(NOLOCK) ON C.ID=ACD.OwnerCityId
			LEFT JOIN TC_Users TU WITH(NOLOCK) ON TU.Id = CSMP.TC_UserId
			LEFT JOIN Absure_WarrantyStatuses AS WS WITH(NOLOCK) ON WS.Id=ACD.Status
			WHERE 
				 ((@CityId IS NULL AND @IsAxaAgency=1) OR ACD.OwnerCityId = @CityId)  AND 
					((@AreaId = 0 AND ACD.OwnerAreaId IN (0,-1)) OR @AreaId IS NULL OR ACD.OwnerAreaId = @AreaId)
				AND (
					(CSMP.TC_UserId IN(SELECT ID FROM @TblTempUsers))
					OR
					((SELECT DISTINCT RoleId FROM TC_UsersRole WHERE UserId = @LoggedInUser AND RoleId IN(9, 14))IN(9,14) 
					AND ISNULL(CSMP.TC_UserId,0) = ISNULL(CSMP.TC_UserId,0)
					)
					)
					AND(
					(@PendingStatus = 4 AND ISNULL(ACD.IsSurveyDone,0) = 1 AND
					 ACD.FinalWarrantyTypeId IS NULL AND ISNULL(ACD.Status,0) NOT IN ( 3,9) AND 
					 (ACD.IsRejected = 0 OR  ACD.IsRejected is NULL) AND CAST(ACD.SurveyDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND 
					 CAST(@EndDate AS DATE) AND (ISNULL(RCImagePending,0) = 0)) -- Approval Pending 

					OR (@PendingStatus = 5 AND ISNULL(ACD.IsSurveyDone,0) = 1 AND 
					(ACD.FinalWarrantyTypeId IS NOT NULL OR ACD.IsRejected = 1) AND ISNULL(ACD.Status,0) NOT IN (3,9)  AND 
					CAST(ACD.SurveyDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)) -- Approval Done	
									
					 OR (@PendingStatus = 7 AND ISNULL(ACD.IsSurveyDone,0) = 1 AND
					 ACD.FinalWarrantyTypeId IS NULL AND ISNULL(ACD.Status,0) <> 3 AND 
					  ISNULL(ACD.IsRejected,0) = 0
					 AND CAST(ACD.SurveyDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE) 
					 AND (ISNULL(RCImagePending,0) = 0) AND ACD.Status = 9) --ONHOLD
					 )	
		       ORDER BY ACD.Make,ACD.Model,ACD.Version


	   END 

END



-------------------------------------------------------------------------------------------------------------------

