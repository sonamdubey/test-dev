IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_GetCarSurveyorMappingTest2]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_GetCarSurveyorMappingTest2]
GO

	-- =============================================
-- Author      : Chetan Navin
-- Create date : 26th Dec 2014
-- Description : To Fetch absure car surveyor mapping data
-- Modifier    : 1. Chetan Navin - 19th Jan 2015 ( Replaced row_number() with dense_rank() for consistent paging )
-- Exec Absure_GetCarSurveyorMapping 3,6,1,NULL,1,NULL,NULL,13227,4
-- Modifier 2  : Ruchira Patil on 19th feb 2015 (displaying only those cars which are assigned to the agency and displaying all to the admin)
-- =============================================
CREATE PROCEDURE [dbo].[Absure_GetCarSurveyorMappingTest2] 
    @Type            INT,
	@BranchId        INT = NULL,
	@CityId          INT,
	@AreaId          INT = NULL,
	@IsSurveyDone    BIT,
	@FromIndex       INT, 
	@ToIndex         INT,
	@LoggedInUser	 INT,
	@PendingStatus	 INT
AS
BEGIN
	DECLARE @IsAxaAgency BIT	
	IF (SELECT DISTINCT RoleId FROM TC_UsersRole WHERE UserId = @LoggedInUser AND RoleId = 15) = 15
	BEGIN	
		SET @IsAxaAgency = 1
		DECLARE @TblTempUsers TABLE (Id INT)
		INSERT INTO @TblTempUsers(Id) VALUES (@LoggedInUser)
		INSERT INTO @TblTempUsers(Id) EXEC TC_GetImmediateChild @LoggedInUser

	END;


	WITH Cte1 
           AS (
				SELECT D.ID AS DealerId,Organization + ' - ' + D.MobileNo AS DealerName,D.MobileNo,
				ISNULL(REPLACE(REPLACE(D.Address1,'"',''),'''',''),'') AS DealerAddress , ACD.Id AS AbSure_CarDetailsId,
				(ACD.Make +' '+ ACD.Model+ ' ' + ACD.Version) AS CarName,ACD.StockId,CSMP.TC_UserId AS TC_UserId ,CASE WHEN IsSurveyDone = 1 THEN CONVERT(VARCHAR,ACD.SurveyDate,106) ELSE CONVERT(VARCHAR,ACD.EntryDate,106) END AS EntryDate,
				ISNULL(ACD.CarScore,0) AS Score,ACD.IsSurveyDone,TU.UserName,TU.IsAgency,
				CASE WHEN IsSurveyDone = 1 THEN ACD.SurveyDate ELSE ACD.EntryDate END AS EntryDateTime,
				ACD.AbSure_WarrantyTypesId,ACD.SurveyDate,AW.Warranty AS Warranty,ACD.IsRejected,YEAR(ACD.MakeYear) AS MakeYear,ACD.RegNumber,A.ID AS AreaId,A.Name AS Area
				FROM Dealers D WITH(NOLOCK) 
				RIGHT JOIN AbSure_CarDetails ACD WITH(NOLOCK) ON D.ID = ACD.DealerId --AND ISNULL(ACD.IsSurveyDone,0) = @IsSurveyDone --AND ISNULL(ACD.Status,0) <> 3    --Status is 3 for cancelled warranty
				LEFT JOIN AbSure_CarSurveyorMapping CSMP WITH(NOLOCK) ON ACD.Id= CSMP.AbSure_CarDetailsId 
				LEFT JOIN Areas A WITH(NOLOCK) ON A.ID = ACD.OwnerAreaId
				LEFT JOIN TC_Users TU WITH(NOLOCK) ON TU.Id = CSMP.TC_UserId
				LEFT JOIN AbSure_WarrantyTypes AWT WITH(NOLOCK) ON  ACD.AbSure_WarrantyTypesId = AWT.AbSure_WarrantyTypesId AND AWT.IsActive = 1
				LEFT JOIN AbSure_WarrantyTypes AW WITH(NOLOCK) ON AW.AbSure_WarrantyTypesId = ACD.FinalWarrantyTypeId
				WHERE D.IsWarranty = 1 
				AND ((@CityId IS NULL AND @IsAxaAgency=1) OR ACD.OwnerCityId = @CityId) AND (@AreaId IS NULL OR ACD.OwnerAreaId = @AreaId)
				AND (
					(CSMP.TC_UserId IN(SELECT ID FROM @TblTempUsers))
					OR
					((SELECT DISTINCT RoleId FROM TC_UsersRole WHERE UserId = @LoggedInUser AND RoleId IN(9, 14))IN(9,14) 
					AND ISNULL(CSMP.TC_UserId,0) = ISNULL(CSMP.TC_UserId,0)
					)
					)
					AND(
					(@PendingStatus = 1 AND CSMP.TC_UserId IS NULL
					) -- Agency assignment pending
					OR (@PendingStatus = 2 AND CSMP.TC_UserId IN (SELECT ID FROM TC_Users WHERE IsAgency = 1) AND ISNULL(ACD.Status,0) <> 3 AND ISNULL(ACD.IsSurveyDone,0) = 0) -- Surveyor assignment pending
					OR (@PendingStatus = 3 AND CSMP.TC_UserId IN (SELECT ID FROM TC_Users WHERE IsAgency <> 1 AND ISNULL(ACD.Status,0) <> 3) AND ISNULL(ACD.IsSurveyDone,0) = 0) -- Inspection pending
					OR (@PendingStatus = 4 AND ISNULL(ACD.IsSurveyDone,0) = 1 AND ACD.FinalWarrantyTypeId IS NULL AND ISNULL(ACD.Status,0) <> 3 AND ACD.IsRejected = 0) -- Approval Pending 
					OR (@PendingStatus = 5 AND ISNULL(ACD.IsSurveyDone,0) = 1 AND (ACD.FinalWarrantyTypeId IS NOT NULL OR ACD.IsRejected = 1) AND ISNULL(ACD.Status,0) <> 3) -- Approval Done
					OR (@PendingStatus = 6 AND ACD.Status = 3) -- Cancelled
					)
				)
         
              SELECT *, DENSE_RANK() OVER (ORDER BY AreaId) NumberForPaging
              INTO   #TblTemp 
              FROM   Cte1 
              
              

              --For fetching Areas
			  IF (@Type = 1) 
				  BEGIN
				  
				  
					 CREATE TABLE #TblPageTemp
					  ( 
						 PageId INT IDENTITY(1,1),
						 AreaId INT,
						 AreaCarCount INT
					  )
		              
		             
					   INSERT INTO #TblPageTemp(AreaId,AreaCarCount)
					   SELECT AreaId, COUNT(AbSure_CarDetailsId) AreaCarCount
					   FROM #TblTemp
					   GROUP BY AreaId
					   ORDER BY COUNT(AbSure_CarDetailsId) DESC
					   
					   SELECT * FROM #TblPageTemp
               
               
					  SELECT DISTINCT Area,AreaId, COUNT(AbSure_CarDetailsId) AreaCarCount
					  FROM #TblTemp 
					  WHERE  
					  (@FromIndex IS NULL AND @ToIndex IS NULL)
					  OR
					  (NumberForPaging  BETWEEN @FromIndex AND @ToIndex )
					  GROUP BY Area,AreaId
					  ORDER BY AreaCarCount DESC

					  SELECT COUNT(DISTINCT AreaId) AS RecordCount 
					  FROM   #TblTemp 
				  END

			  --For fetching dealers
			  IF (@Type = 2) 
				  BEGIN
					  SELECT DISTINCT DealerId,DealerName,DealerAddress, COUNT(AbSure_CarDetailsId) DealerCarCount
					  FROM #TblTemp 
					  WHERE (@AreaId IS NULL OR AreaId = @AreaId)
					  GROUP BY DealerId,DealerName,DealerAddress
					  ORDER BY DealerCarCount DESC
					  --WHERE  
					  --(@FromIndex IS NULL AND @ToIndex IS NULL)
					  --OR
					  --(NumberForPaging  BETWEEN @FromIndex AND @ToIndex )
					  --ORDER BY DealerName

					  --SELECT COUNT(DISTINCT DealerId) AS RecordCount 
					  --FROM   #TblTemp 
				  END
			  --For fetching absure cars under dealer for warranty
			  IF(@Type = 3)
				BEGIN
					SELECT * FROM #TblTemp WHERE DealerId = @BranchId AND AreaId = @AreaId
					ORDER BY IsAgency ASC, EntryDateTime DESC
				END  

		      DROP TABLE #TblTemp 
END