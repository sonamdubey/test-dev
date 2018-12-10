IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_GetCarSurveyorMappingTest]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_GetCarSurveyorMappingTest]
GO

	-- =============================================
-- Author      : Chetan Navin
-- Create date : 26th Dec 2014
-- Description : To Fetch absure car surveyor mapping data
-- Modifier 1  : Chetan Navin - 19th Jan 2015 ( Replaced row_number() with dense_rank() for consistent paging )
-- Modifier 2  : Ruchira Patil on 19th feb 2015 (displaying only those cars which are assigned to the agency and displaying all to the admin)
-- Modifier 3  : Chetan Navin - 16th Mar 2015 (Added type 4 to fetch areas on the basis of searched dealer)
-- Exec Absure_GetCarSurveyorMappingTest 4,NULL,1,NULL,0,1,10,13175,6,'The Automotive Pvt. Ltd'
-- =============================================
CREATE PROCEDURE [dbo].[Absure_GetCarSurveyorMappingTest] 
    @Type            INT,
	@BranchId        INT = NULL,
	@CityId          INT,
	@AreaId          INT = NULL,
	@IsSurveyDone    BIT,
	@FromIndex       INT, 
	@ToIndex         INT,
	@LoggedInUser	 INT,
	@PendingStatus	 INT,
	@Organization    VARCHAR(200) = NULL
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
				SELECT D.ID AS DealerId,Organization + ' - ' + D.MobileNo AS DealerName,D.MobileNo,D.Organization,
				ISNULL(REPLACE(REPLACE(D.Address1,'"',''),'''',''),'') AS DealerAddress , ACD.Id AS AbSure_CarDetailsId,
				(ACD.Make +' '+ ACD.Model+ ' ' + ACD.Version) AS CarName,ACD.StockId,CSMP.TC_UserId AS TC_UserId ,CASE WHEN IsSurveyDone = 1 THEN CONVERT(VARCHAR,ACD.SurveyDate,106) ELSE CONVERT(VARCHAR,ACD.EntryDate,106) END AS EntryDate,
				ISNULL(ACD.CarScore,0) AS Score,ACD.IsSurveyDone,TU.UserName,TU.IsAgency,
				--CASE WHEN IsSurveyDone = 1 THEN ACD.SurveyDate ELSE ACD.EntryDate END AS EntryDateTime,
				ACD.AbSure_WarrantyTypesId,ACD.SurveyDate,AW.Warranty AS Warranty,ACD.IsRejected,YEAR(ACD.MakeYear) AS MakeYear,REPLACE(REPLACE(ACD.RegNumber,char(10),' '),char(13),' ') AS RegNumber,ISNULL(A.ID,0) AS AreaId,A.Name AS Area
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
					(@PendingStatus = 1 AND CSMP.TC_UserId IS NULL AND ISNULL(ACD.Status,0) <> 3
					) -- Agency assignment pending
					OR (@PendingStatus = 2 AND CSMP.TC_UserId IN (SELECT ID FROM TC_Users WHERE IsAgency = 1) AND ISNULL(ACD.Status,0) <> 3 AND ISNULL(ACD.IsSurveyDone,0) = 0) -- Surveyor assignment pending
					OR (@PendingStatus = 3 AND CSMP.TC_UserId IN (SELECT ID FROM TC_Users WHERE IsAgency <> 1 AND ISNULL(ACD.Status,0) <> 3) AND ISNULL(ACD.IsSurveyDone,0) = 0) -- Inspection pending
					OR (@PendingStatus = 4 AND ISNULL(ACD.IsSurveyDone,0) = 1 AND ACD.FinalWarrantyTypeId IS NULL AND ISNULL(ACD.Status,0) <> 3 AND ACD.IsRejected = 0) -- Approval Pending 
					OR (@PendingStatus = 5 AND ISNULL(ACD.IsSurveyDone,0) = 1 AND (ACD.FinalWarrantyTypeId IS NOT NULL OR ACD.IsRejected = 1) AND ISNULL(ACD.Status,0) <> 3) -- Approval Done
					OR (@PendingStatus = 6 AND ACD.Status = 3) -- Cancelled
					)
				)              
              SELECT *
              INTO   #TblTemp 
              FROM   Cte1 
             
             --For fetching Areas
			  IF (@Type = 1) 
			  BEGIN
					SELECT Area,AreaId, COUNT(AbSure_CarDetailsId)AreaCarCount,ROW_NUMBER() OVER(ORDER BY COUNT(AbSure_CarDetailsId) DESC) AreaNumberForPaging
                    INTO  #TblPageTempArea 
					FROM #TblTemp 
					GROUP BY Area,AreaId
					ORDER BY AreaCarCount DESC

					SELECT DISTINCT Area,AreaId, AreaCarCount
                    FROM #TblPageTempArea
                    WHERE  
                    (@FromIndex IS NULL AND @ToIndex IS NULL)
                    OR
                    (AreaNumberForPaging  BETWEEN @FromIndex AND @ToIndex )
                    GROUP BY Area,AreaId,AreaCarCount
                    ORDER BY AreaCarCount DESC

                    SELECT COUNT(DISTINCT AreaId) AS RecordCount
                    FROM   #TblTemp
                    
					DROP TABLE #TblPageTempArea
			  END

			  --For fetching dealers
			  IF (@Type = 2) 
			  BEGIN		
					IF(@Organization IS NOT NULL)
						BEGIN
							SELECT DISTINCT DealerId,DealerName,DealerAddress, COUNT(AbSure_CarDetailsId) DealerCarCount
							FROM #TblTemp 
							WHERE (@AreaId IS NULL OR AreaId = @AreaId) AND (DealerName IS NULL OR LOWER(DealerName) LIKE LOWER(@Organization + '%'))
							GROUP BY DealerId,DealerName,DealerAddress
							ORDER BY DealerCarCount DESC
						END 
					ELSE
						BEGIN
							SELECT DISTINCT DealerId,DealerName,DealerAddress, COUNT(AbSure_CarDetailsId) DealerCarCount
							FROM #TblTemp 
							WHERE (@AreaId IS NULL OR AreaId = @AreaId)
							GROUP BY DealerId,DealerName,DealerAddress
							ORDER BY DealerCarCount DESC
						END	
			  END

			  --For fetching absure cars under dealer for warranty
			  IF(@Type = 3)
				BEGIN
					SELECT * FROM #TblTemp WHERE DealerId = @BranchId AND AreaId = @AreaId
					ORDER BY IsAgency ASC, EntryDate DESC
				END  
			  
			  --For fetching areas of searched dealer
			  IF(@Type = 4)
				BEGIN
					SELECT Area,AreaId,Organization,COUNT(AbSure_CarDetailsId) OVER(PARTITION BY AreaId) AreaCarCount
					INTO  #TblPageTemp 
					FROM #TblTemp 
					WHERE @Organization IS NULL OR Organization LIKE @Organization + '%'
					GROUP BY Area,AreaId,Organization,AbSure_CarDetailsId
					ORDER BY AreaCarCount

					SELECT Area,AreaId,Organization, AreaCarCount,ROW_NUMBER() OVER(ORDER BY AreaCarCount DESC) AreaNumberForPaging
					INTO  #TblPageTempSearchArea 
					FROM #TblPageTemp
					GROUP BY Area,AreaId,Organization,AreaCarCount
					ORDER BY AreaCarCount DESC

					SELECT DISTINCT Area,AreaId, AreaCarCount
					FROM #TblPageTempSearchArea
					WHERE  
					(@FromIndex IS NULL AND @ToIndex IS NULL)
					OR
					(AreaNumberForPaging  BETWEEN @FromIndex AND @ToIndex) AND (@Organization IS NULL OR LOWER(Organization) LIKE LOWER(@Organization + '%'))
					ORDER BY AreaCarCount DESC

					SELECT COUNT(DISTINCT AreaId) AS RecordCount
					FROM   #TblPageTempSearchArea
					WHERE @Organization IS NULL OR Organization LIKE @Organization + '%'
                    
					DROP TABLE #TblPageTemp
					DROP TABLE #TblPageTempSearchArea	
				END

		    DROP TABLE #TblTemp 
END
