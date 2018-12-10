IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_GetCarSurveyorMapping]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_GetCarSurveyorMapping]
GO

	
-- =============================================
-- Author      : Chetan Navin
-- Create date : 26th Dec 2014
-- Description : To Fetch absure car surveyor mapping data
-- Modifier 1  : Chetan Navin - 19th Jan 2015 ( Replaced row_number() with dense_rank() for consistent paging )
-- Modifier 2  : Ruchira Patil on 19th feb 2015 (displaying only those cars which are assigned to the agency and displaying all to the admin)
-- Modifier 3  : Chetan Navin - 16th Mar 2015 (Added type 4 to fetch areas on the basis of searched dealer)
-- Modifier 4  : Vinay Prajapati, Added condition of  'OR D.IsInspection=1' WHERE D.IsWarranty = 1
-- Modifier 5  : Chetan Navin - 6 May 2015 (Added conditions for sorting and searching)
-- Exec Absure_GetCarSurveyorMappingTest 1,NULL,1,NULL,0,NULL,NULL,13175,6,NULL,2,2
-- EXEC [Absure_GetCarSurveyorMapping] 2,NULL,12,NULL,0,1,10,20074,1,NULL,NULL,NULL,'2015-08-01','2015-08-11'
-- EXEC [Absure_GetCarSurveyorMapping] 2,NULL,1,NULL,0,NULL,NULL,20074,3,NULL,NULL,1,'2015-08-01','2015-08-17',1
-- Modifier 6  : Ruchira Patil - 7th July 2015 (to change the date filter according to the status)
-- Modified by : Ruchira Patil on 5th Aug 2015 (approval pending fetch completed inspection data(with Rc Image))
-- Modifier By : Kartik Rathod on Aug 21,2015 (Added status in Type 3)
-- Modified by : Kartik rathod on 25 Aug,2015 (added condition isActive=1 for stockid,carId) 
-- Modified by : Nilima More on 26 th august (added condition for is rejected=null)
-- Modified By : Kartik rathod on 14 sept 2015 (remove condition of IsActive=1 for Search By CarId)
-- Modified By : Nilima More on 15th Sept 2015 (STATUS=9 Doubtfull)
-- Modified By : Nilima More on 12th Oct 2015 (Filter for Expired Certificate)
-- =======================================================================================================
CREATE PROCEDURE [dbo].[Absure_GetCarSurveyorMapping]
    @Type            INT,
	@BranchId        INT = NULL,
	@CityId          INT,
	@AreaId          INT = NULL,
	@IsSurveyDone    BIT,
	@FromIndex       INT, 
	@ToIndex         INT,
	@LoggedInUser	 INT,
	@PendingStatus	 INT,
	@SearchBy        TINYINT = NULL,
	@SearchText      VARCHAR(200) = NULL,
	@SortBy          TINYINT = NULL,
	@StartDate       DATETIME = NULL,                     --Oldest Entry Date
	@EndDate         DATETIME = NULL ,                  --Latest Entry Date  
    @IsDealerwise	 BIT = NULL
	
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

	--WITH Cte1 
 --          AS (
	--			SELECT D.ID AS DealerId,Organization + ' - ' + D.MobileNo AS DealerName,D.MobileNo,D.Organization,
	--			ISNULL(REPLACE(REPLACE(D.Address1,'"',''),'''',''),'') AS DealerAddress , ACD.Id AS AbSure_CarDetailsId,
	--			(ACD.Make +' '+ ACD.Model+ ' ' + ACD.Version) AS CarName,ACD.StockId,CSMP.TC_UserId AS TC_UserId ,CASE WHEN IsSurveyDone = 1 THEN CONVERT(VARCHAR,ACD.SurveyDate,106) ELSE CONVERT(VARCHAR,ACD.EntryDate,106) END AS EntryDate,
	--			ISNULL(ACD.CarScore,0) AS Score,ACD.IsSurveyDone,TU.UserName, TU.IsAgency,CASE WHEN IsSurveyDone = 1 THEN ACD.SurveyDate ELSE ACD.EntryDate END AS SortingDate,
	--			--CASE WHEN IsSurveyDone = 1 THEN ACD.SurveyDate ELSE ACD.EntryDate END AS EntryDateTime,
	--			ACD.AbSure_WarrantyTypesId,ACD.SurveyDate,AW.Warranty AS Warranty,ACD.IsRejected,YEAR(ACD.MakeYear) AS MakeYear,
	--			REPLACE(REPLACE(ACD.RegNumber,CHAR(10),' '),CHAR(13),' ') AS RegNumber/*ACD.RegNumber*/,ISNULL(A.ID,0) AS AreaId,ISNULL(A.Name,'(City)') AS Area, ACD.OwnerAreaId OwnerAreaId
	--			--A.ID AS AreaId,A.Name AS Area
	--			FROM Dealers D WITH(NOLOCK) 
	--			RIGHT JOIN AbSure_CarDetails ACD WITH(NOLOCK) ON D.ID = ACD.DealerId --AND ISNULL(ACD.IsSurveyDone,0) = @IsSurveyDone --AND ISNULL(ACD.Status,0) <> 3    --Status is 3 for cancelled warranty
	--			LEFT JOIN AbSure_CarSurveyorMapping CSMP WITH(NOLOCK) ON ACD.Id= CSMP.AbSure_CarDetailsId 
	--			LEFT JOIN Areas A WITH(NOLOCK) ON A.ID = ACD.OwnerAreaId
	--			LEFT JOIN TC_Users TU WITH(NOLOCK) ON TU.Id = CSMP.TC_UserId
	--			LEFT JOIN AbSure_WarrantyTypes AWT WITH(NOLOCK) ON  ACD.AbSure_WarrantyTypesId = AWT.AbSure_WarrantyTypesId AND AWT.IsActive = 1
	--			LEFT JOIN AbSure_WarrantyTypes AW WITH(NOLOCK) ON AW.AbSure_WarrantyTypesId = ACD.FinalWarrantyTypeId
	--			WHERE --(ISNULL(D.IsWarranty,0) = 1 OR ISNULL(D.IsInspection,0) = 1)
				--AND 
				--((@SearchBy IS NOT NULL AND @SearchText IS NOT NULL) AND (
				--	(CSMP.TC_UserId IN(SELECT ID FROM @TblTempUsers))
				--	OR
				--	((SELECT DISTINCT RoleId FROM TC_UsersRole WHERE UserId = @LoggedInUser AND RoleId IN(9, 14))IN(9,14) 
				--	AND ISNULL(CSMP.TC_UserId,0) = ISNULL(CSMP.TC_UserId,0)
				--	)
				--	) ) 
				--OR 
				--(
				--	((@CityId IS NULL AND @IsAxaAgency=1) OR ACD.OwnerCityId = @CityId)  AND 
			
				--	((@AreaId = 0 AND ACD.OwnerAreaId IN (0,-1)) OR @AreaId IS NULL OR ACD.OwnerAreaId = @AreaId) 
				--	AND (
				--	(CSMP.TC_UserId IN(SELECT ID FROM @TblTempUsers))
				--	OR
				--	((SELECT DISTINCT RoleId FROM TC_UsersRole WHERE UserId = @LoggedInUser AND RoleId IN(9, 14))IN(9,14) 
				--	AND ISNULL(CSMP.TC_UserId,0) = ISNULL(CSMP.TC_UserId,0)
				--	)
				--	)
				--	AND(
				--	(@PendingStatus = 1 AND CSMP.TC_UserId IS NULL AND ISNULL(ACD.Status,0) <> 3 AND CAST(ACD.EntryDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)) -- Agency assignment pending
				--	OR (@PendingStatus = 2 AND CSMP.TC_UserId IN (SELECT ID FROM TC_Users WHERE IsAgency = 1) AND ISNULL(ACD.Status,0) <> 3 AND ISNULL(ACD.IsSurveyDone,0) = 0 AND CAST(ACD.EntryDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)) -- Surveyor assignment pending
				--	OR (@PendingStatus = 3 AND CSMP.TC_UserId IN (SELECT ID FROM TC_Users WHERE IsAgency <> 1 AND ISNULL(ACD.Status,0) <> 3) AND ISNULL(ACD.IsSurveyDone,0) = 0 AND CAST(ACD.EntryDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)) -- Inspection pending
				--	OR (@PendingStatus = 4 AND ISNULL(ACD.IsSurveyDone,0) = 1 AND ACD.FinalWarrantyTypeId IS NULL AND ISNULL(ACD.Status,0) <> 3 AND ACD.IsRejected = 0 AND CAST(ACD.SurveyDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE) AND (ISNULL(RCImagePending,0) = 0)) -- Approval Pending 
				--	OR (@PendingStatus = 5 AND ISNULL(ACD.IsSurveyDone,0) = 1 AND (ACD.FinalWarrantyTypeId IS NOT NULL OR ACD.IsRejected = 1) AND ISNULL(ACD.Status,0) <> 3 AND CAST(ACD.SurveyDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)) -- Approval Done
				--	OR (@PendingStatus = 6 AND ACD.Status = 3 AND CAST(ACD.EntryDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)) -- Cancelled
				--	)
				--)
				--)  
	WITH Cte1 
           AS (
				SELECT D.ID AS DealerId,Organization + ' - ' + D.MobileNo AS DealerName,D.MobileNo,D.Organization,ISNULL(ACD.Status,0) Status,
				ISNULL(REPLACE(REPLACE(D.Address1,'"',''),'''',''),'') AS DealerAddress , ACD.Id AS AbSure_CarDetailsId,
				(ACD.Make +' '+ ACD.Model+ ' ' + ACD.Version) AS CarName,ACD.StockId,CSMP.TC_UserId AS TC_UserId ,CASE WHEN IsSurveyDone = 1 THEN CONVERT(VARCHAR,ACD.SurveyDate,106) ELSE CONVERT(VARCHAR,ACD.EntryDate,106) END AS EntryDate,
				ISNULL(ACD.CarScore,0) AS Score,ACD.IsSurveyDone IsSurveyDone,TU.UserName, TU.IsAgency,CASE WHEN IsSurveyDone = 1 THEN ACD.SurveyDate ELSE ACD.EntryDate END AS SortingDate,
				--CASE WHEN IsSurveyDone = 1 THEN ACD.SurveyDate ELSE ACD.EntryDate END AS EntryDateTime,
				ACD.AbSure_WarrantyTypesId,ACD.SurveyDate,AW.Warranty AS Warranty,ACD.IsRejected,YEAR(ACD.MakeYear) AS MakeYear,
				REPLACE(REPLACE(ACD.RegNumber,CHAR(10),' '),CHAR(13),' ') AS RegNumber/*ACD.RegNumber*/,ISNULL(A.ID,0) AS AreaId,ISNULL(A.Name,'(City)') AS Area, ACD.OwnerAreaId OwnerAreaId
				--A.ID AS AreaId,A.Name AS Area
				FROM AbSure_CarDetails ACD WITH(NOLOCK) 
				LEFT JOIN Dealers D WITH(NOLOCK) ON D.ID = ACD.DealerId --AND ISNULL(ACD.IsSurveyDone,0) = @IsSurveyDone --AND ISNULL(ACD.Status,0) <> 3    --Status is 3 for cancelled warranty
				LEFT JOIN AbSure_CarSurveyorMapping CSMP WITH(NOLOCK) ON ACD.Id= CSMP.AbSure_CarDetailsId 
				LEFT JOIN Areas A WITH(NOLOCK) ON A.ID = ACD.OwnerAreaId
				LEFT JOIN TC_Users TU WITH(NOLOCK) ON TU.Id = CSMP.TC_UserId
				LEFT JOIN AbSure_WarrantyTypes AWT WITH(NOLOCK) ON  ACD.AbSure_WarrantyTypesId = AWT.AbSure_WarrantyTypesId AND AWT.IsActive = 1
				LEFT JOIN AbSure_WarrantyTypes AW WITH(NOLOCK) ON AW.AbSure_WarrantyTypesId = ACD.FinalWarrantyTypeId
				WHERE --(ISNULL(D.IsWarranty,0) = 1 OR ISNULL(D.IsInspection,0) = 1)
				--AND 
				(
					(@SearchBy = 1 AND @SearchText IS NOT NULL) 
					AND 
					(					
						(
							((
								(CSMP.TC_UserId IN(SELECT ID FROM @TblTempUsers))
							)
							OR
							(
								(SELECT DISTINCT RoleId FROM TC_UsersRole WITH(NOLOCK) WHERE UserId = @LoggedInUser AND RoleId IN(9, 14))IN(9,14) 
									AND ISNULL(CSMP.TC_UserId,0) = ISNULL(CSMP.TC_UserId,0)
							))
							AND 
							(
								(@PendingStatus = 1 AND CSMP.TC_UserId IS NULL AND ISNULL(ACD.Status,0) <> 3 ) -- Agency assignment pending
								OR (@PendingStatus = 2 AND CSMP.TC_UserId IN (SELECT ID FROM TC_Users WITH(NOLOCK) WHERE IsAgency = 1) AND ISNULL(ACD.Status,0) <> 3 AND ISNULL(ACD.IsSurveyDone,0) = 0 ) -- Surveyor assignment pending
								OR (@PendingStatus = 3 AND CSMP.TC_UserId IN (SELECT ID FROM TC_Users WITH(NOLOCK) WHERE IsAgency <> 1 AND ISNULL(ACD.Status,0) <> 3) AND ISNULL(ACD.IsSurveyDone,0) = 0) -- Inspection pending
								OR (@PendingStatus = 4 AND ISNULL(ACD.IsSurveyDone,0) = 1 AND ACD.FinalWarrantyTypeId IS NULL AND ISNULL(ACD.Status,0) <> 3 AND ISNULL(ACD.Status,0) <> 9 AND(ACD.IsRejected = 0 OR  ACD.IsRejected is NULL ) AND (ISNULL(RCImagePending,0) = 0) AND (DATEDIFF(DD , ACD.SurveyDate , GETDATE ())) <= 30 ) -- Approval Pending 
								OR (@PendingStatus = 5 AND ISNULL(ACD.IsSurveyDone,0) = 1 AND (ACD.FinalWarrantyTypeId IS NOT NULL OR ISNULL(ACD.IsRejected,0) = 1) AND ISNULL(ACD.Status,0) <> 3 AND (DATEDIFF(DD , ACD.SurveyDate , GETDATE ())) <= 30 ) -- Approval Done
								OR (@PendingStatus = 6 AND ACD.Status = 3 ) -- Cancelled
								OR (@PendingStatus = 7 AND ACD.Status = 9 AND ISNULL(ACD.IsSurveyDone,0) = 1  AND ACD.FinalWarrantyTypeId IS NULL AND ISNULL(ACD.IsRejected,0) = 0 AND (DATEDIFF(DD , ACD.SurveyDate , GETDATE ())) <= 30 ) --OnHold
								OR (@PendingStatus = 8 AND ISNULL(ACD.IsSurveyDone,0) = 1 AND (DATEDIFF(DD , ISNULL(ACD.SurveyDate,0), GETDATE ())) > 30 ) --Expired Certificate
							) 
						)
					)
				)
				OR
				((
					@SearchBy IS NOT NULL AND @SearchText IS NOT NULL) 
					AND (@SearchBy <> 1  
					AND (
							(CSMP.TC_UserId IN(SELECT ID FROM @TblTempUsers))
							OR
							(SELECT DISTINCT RoleId FROM TC_UsersRole WITH(NOLOCK) WHERE UserId = @LoggedInUser AND RoleId IN(9, 14))IN(9,14) 
									AND ISNULL(CSMP.TC_UserId,0) = ISNULL(CSMP.TC_UserId,0)
					)
					AND ((@SearchBy = 3 AND ACD.IsActive=1) OR (@SearchBy = 2 ))
				))
				OR 
				(
					((@CityId IS NULL AND @IsAxaAgency=1) OR ACD.OwnerCityId = @CityId)  AND 
			
					((@AreaId = 0 AND ACD.OwnerAreaId IN (0,-1)) OR @AreaId IS NULL OR ACD.OwnerAreaId = @AreaId) 
					AND (
					(CSMP.TC_UserId IN(SELECT ID FROM @TblTempUsers))
					OR
					((SELECT DISTINCT RoleId FROM TC_UsersRole WITH(NOLOCK) WHERE UserId = @LoggedInUser AND RoleId IN(9, 14))IN(9,14) 
					AND ISNULL(CSMP.TC_UserId,0) = ISNULL(CSMP.TC_UserId,0)
					)
					)
					AND(
					(@PendingStatus = 1 AND CSMP.TC_UserId IS NULL AND ISNULL(ACD.Status,0) <> 3 AND CAST(ACD.EntryDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)) -- Agency assignment pending
					OR (@PendingStatus = 2 AND CSMP.TC_UserId IN (SELECT ID FROM TC_Users WITH(NOLOCK) WHERE IsAgency = 1) AND ISNULL(ACD.Status,0) <> 3 AND ISNULL(ACD.IsSurveyDone,0) = 0 AND CAST(ACD.EntryDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)) -- Surveyor assignment pending
					OR (@PendingStatus = 3 AND CSMP.TC_UserId IN (SELECT ID FROM TC_Users WITH(NOLOCK) WHERE IsAgency <> 1 AND ISNULL(ACD.Status,0) <> 3) AND ISNULL(ACD.IsSurveyDone,0) = 0 AND CAST(ACD.EntryDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)) -- Inspection pending
					OR (@PendingStatus = 4 AND ISNULL(ACD.IsSurveyDone,0) = 1 AND ACD.FinalWarrantyTypeId IS NULL AND ISNULL(ACD.Status,0) <> 3 AND  ISNULL(ACD.Status,0) <> 9 AND (ACD.IsRejected = 0 OR  ACD.IsRejected is NULL) AND CAST(ACD.SurveyDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE) AND (ISNULL(RCImagePending,0) = 0)AND (DATEDIFF(DD , ACD.SurveyDate , GETDATE ())) <= 30 ) -- Approval Pending 
					OR (@PendingStatus = 5 AND ISNULL(ACD.IsSurveyDone,0) = 1 AND (ACD.FinalWarrantyTypeId IS NOT NULL OR ISNULL(ACD.IsRejected,0) = 1) AND ISNULL(ACD.Status,0) <> 3 AND CAST(ACD.SurveyDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE) AND (DATEDIFF(DD , ACD.SurveyDate , GETDATE ())) <= 30) -- Approval Done
					OR (@PendingStatus = 6 AND ACD.Status = 3 AND CAST(ACD.EntryDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)) -- Cancelled
					OR (@PendingStatus = 7 AND ACD.Status = 9 AND (DATEDIFF(DD , ACD.SurveyDate , GETDATE ())) <= 30 AND CAST(ACD.EntryDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE) ) --Doubtfull
					OR (@PendingStatus = 8 AND ISNULL(ACD.IsSurveyDone,0) = 1 AND  (DATEDIFF(DD , ISNULL(ACD.SurveyDate,0) , GETDATE ())) > 30 AND CAST(ACD.SurveyDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)) --Expired Certificate
					)
				)
				)               
              SELECT *--, DENSE_RANK() OVER (ORDER BY AreaId) NumberForPaging
              INTO   #TblTemp 
              FROM   Cte1 

			  create index ix_TblTemp_AbSure_CarDetailsId on #TblTemp(AbSure_CarDetailsId)
			  create index ix_TblTemp_DealerId on #TblTemp(DealerId)
			  create index ix_TblTemp_AreaId on #TblTemp(AreaId)
             
             --For fetching Areas
			  IF (@Type = 1) 
			  BEGIN
					SELECT Area,AreaId, COUNT(AbSure_CarDetailsId) OVER(PARTITION BY AreaId) AreaCarCount,SortingDate,Organization
                    INTO  #TblPageTempArea 
					FROM #TblTemp 
					GROUP BY Area,AreaId,AbSure_CarDetailsId,SortingDate,Organization

					CREATE TABLE #TblDump
					( 
						Id INT IDENTITY(1,1) NOT NULL,
						Area   VARCHAR(50),
						AreaId INT NOT NULL,
						AreaCarCount INT NOT NULL,
						SortingDate DATE,
						Organization VARCHAR(200)
					)

					INSERT INTO #TblDump (Area,AreaId,AreaCarCount,SortingDate,Organization)  SELECT Area,AreaId,AreaCarCount,SortingDate,Organization FROM
					(
						SELECT ROW_NUMBER() OVER(PARTITION BY AreaId ORDER BY  AreaId) AS Row,* FROM #TblPageTempArea
					) A
					WHERE A.Row = 1  
                    GROUP BY Area,AreaId,AreaCarCount,SortingDate,Organization
					ORDER BY CASE WHEN @SortBy = 1 THEN AreaCarCount END,
							 CASE WHEN @SortBy = 2 THEN AreaCarCount END DESC,
							 CASE WHEN @SortBy = 3 THEN SortingDate END,
							 CASE WHEN @SortBy = 4 THEN SortingDate END DESC,
							 CASE WHEN @SortBy = 5 THEN Organization END,
							 CASE WHEN @SortBy = 6 THEN Organization END DESC;

					SELECT * FROM #TblDump
					WHERE 
					(@FromIndex IS NULL AND @ToIndex IS NULL)
                    OR
                    (Id BETWEEN @FromIndex AND @ToIndex)

					--SELECT Area,AreaId,AreaCarCount
     --               FROM #TblPageTempArea
     --               WHERE  
     --               (@FromIndex IS NULL AND @ToIndex IS NULL)
     --               OR
     --               (AreaNumberForPaging BETWEEN @FromIndex AND @ToIndex )
     --               GROUP BY Area,AreaId,AreaCarCount,SurveyDate,Organization
					--ORDER BY CASE WHEN @SortDirection = 1 AND @SortBy = 1 THEN AreaCarCount END,
					--		 CASE WHEN @SortDirection = 2 AND @SortBy = 1 THEN AreaCarCount END DESC,
					--		 CASE WHEN @SortDirection = 1 AND @SortBy = 2 THEN SurveyDate END,
					--		 CASE WHEN @SortDirection = 2 AND @SortBy = 2 THEN SurveyDate END DESC,
					--		 CASE WHEN @SortDirection = 1 AND @SortBy = 3 THEN Organization END,
					--		 CASE WHEN @SortDirection = 2 AND @SortBy = 3 THEN Organization END DESC;					
                    SELECT COUNT(Id) AS RecordCount
                    FROM   #TblDump
                    
					DROP TABLE #TblPageTempArea  
					DROP TABLE #TblDump 
			  END

			  --For fetching dealers
			 --For fetching dealers
			   IF (@Type = 2) 
			  BEGIN	
					IF(@SearchBy IS NOT NULL)
						BEGIN

						
							SELECT DealerId,DealerName,DealerAddress, COUNT(AbSure_CarDetailsId) OVER(PARTITION BY DealerId) DealerCarCount
							FROM #TblTemp 
							WHERE ((@AreaId = 0 AND OwnerAreaId IN (0,-1)) OR @AreaId IS NULL OR OwnerAreaId = @AreaId) 
									AND (@SearchBy = 1 AND (Organization IS NULL OR (LOWER(Organization) LIKE '%' + LOWER(@SearchText) + '%')))
									OR (@SearchBy = 2 AND AbSure_CarDetailsId = CASE WHEN @SearchBy = 1 THEN 0 ELSE @SearchText END)
									OR (@SearchBy = 3 AND StockId = CASE WHEN @SearchBy = 1 THEN 0 ELSE @SearchText END )
							GROUP BY DealerId,DealerName,DealerAddress,SortingDate,Organization,AbSure_CarDetailsId
							ORDER BY CASE WHEN @SortBy = 1 THEN COUNT(AbSure_CarDetailsId) END,
									 CASE WHEN @SortBy = 2 THEN COUNT(AbSure_CarDetailsId) END DESC,
									 CASE WHEN @SortBy = 3 THEN SortingDate END,
									 CASE WHEN @SortBy = 4 THEN SortingDate END DESC,
									 CASE WHEN @SortBy = 5 THEN Organization END,
									 CASE WHEN @SortBy = 6 THEN Organization END DESC;

							--SELECT DealerId,DealerName,DealerAddress, COUNT(AbSure_CarDetailsId) DealerCarCount,SurveyDate,Organization
							--FROM #TblTemp 
							--WHERE ((@AreaId = 0 AND OwnerAreaId IN (0,-1)) OR @AreaId IS NULL OR OwnerAreaId = @AreaId) 
							--		AND (DealerName IS NULL OR LOWER(DealerName) LIKE LOWER('%' + @SearchBy + '%'))
							--GROUP BY DealerId,DealerName,DealerAddress,SurveyDate,Organization
							--ORDER BY CASE WHEN @SortDirection = 1 AND @SortBy = 1 THEN COUNT(AbSure_CarDetailsId) END,
							--		 CASE WHEN @SortDirection = 2 AND @SortBy = 1 THEN COUNT(AbSure_CarDetailsId) END DESC,
							--		 CASE WHEN @SortDirection = 1 AND @SortBy = 2 THEN SurveyDate END,
							--		 CASE WHEN @SortDirection = 2 AND @SortBy = 2 THEN SurveyDate END DESC,
							--		 CASE WHEN @SortDirection = 1 AND @SortBy = 3 THEN Organization END,
							--		 CASE WHEN @SortDirection = 2 AND @SortBy = 3 THEN Organization END DESC;
						END 
					ELSE
						BEGIN
							IF(@IsDealerwise <> 1 OR @IsDealerwise IS NULL)
							BEGIN
							--SELECT DISTINCT DealerId,DealerName,DealerAddress, COUNT(AbSure_CarDetailsId) DealerCarCount
							--FROM #TblTemp 
							--WHERE ((@AreaId = 0 AND OwnerAreaId IN (0,-1)) OR @AreaId IS NULL OR OwnerAreaId = @AreaId)
							--GROUP BY DealerId,DealerName,DealerAddress
							--ORDER BY DealerCarCount DESC
							SELECT DealerId,DealerName,DealerAddress, COUNT(AbSure_CarDetailsId) OVER(PARTITION BY DealerId) DealerCarCount,Area,AreaId
							FROM #TblTemp 
							WHERE ((@AreaId = 0 AND OwnerAreaId IN (0,-1)) OR @AreaId IS NULL OR OwnerAreaId = @AreaId) 
							GROUP BY DealerId,DealerName,DealerAddress,SortingDate,Organization,AbSure_CarDetailsId,Area,AreaId
							ORDER BY CASE WHEN @SortBy = 1 THEN COUNT(AbSure_CarDetailsId) END,
									 CASE WHEN @SortBy = 2 THEN COUNT(AbSure_CarDetailsId) END DESC,
									 CASE WHEN @SortBy = 3 THEN SortingDate END,
									 CASE WHEN @SortBy = 4 THEN SortingDate END DESC,
									 CASE WHEN @SortBy = 5 THEN Organization END,
									 CASE WHEN @SortBy = 6 THEN Organization END DESC;
							END	
							ELSE
							BEGIN
							
								SELECT AbSure_CarDetailsId,DealerId,DealerName,DealerAddress, COUNT(AbSure_CarDetailsId) OVER(PARTITION BY AreaId,DealerId) DealerCarCount,Area,AreaId,SortingDate
								INTO #TblPageTempDealer
								FROM #TblTemp 
								WHERE ((@AreaId = 0 AND OwnerAreaId IN (0,-1)) OR @AreaId IS NULL OR OwnerAreaId = @AreaId) 
								GROUP BY DealerId,DealerName,DealerAddress,SortingDate,Organization,AbSure_CarDetailsId,Area,AreaId
								ORDER BY CASE WHEN @SortBy = 1 THEN COUNT(AbSure_CarDetailsId) END,
										 CASE WHEN @SortBy = 2 THEN COUNT(AbSure_CarDetailsId) END DESC,
										 CASE WHEN @SortBy = 3 THEN SortingDate END,
										 CASE WHEN @SortBy = 4 THEN SortingDate END DESC,
										 CASE WHEN @SortBy = 5 THEN Organization END,
										 CASE WHEN @SortBy = 6 THEN Organization END DESC;

								
								CREATE TABLE #TblDumpDealer
								( 
									Id INT IDENTITY(1,1) NOT NULL,
									Area   VARCHAR(50),
									AreaId INT NOT NULL,
									DealerCarCount INT NOT NULL,
									SortingDate DATE,
									DealerId INT,
									DealerName VARCHAR(200),
									DealerAddress VARCHAR(200)
								)

								INSERT INTO #TblDumpDealer (Area,AreaId,DealerCarCount,SortingDate,DealerId,DealerName,DealerAddress)  SELECT Area,AreaId,DealerCarCount,SortingDate,DealerId,DealerName,DealerAddress FROM
								(
									SELECT ROW_NUMBER() OVER(PARTITION BY dealerid,areaid ORDER BY  dealerid) AS Row,* FROM #TblPageTempDealer
								) A
								WHERE A.Row = 1  
								GROUP BY Area,AreaId,DealerCarCount,DealerId,DealerName,DealerAddress,SortingDate
								ORDER BY CASE WHEN @SortBy = 1 THEN DealerCarCount END,
										 CASE WHEN @SortBy = 2 THEN DealerCarCount END DESC,
										 CASE WHEN @SortBy = 3 THEN SortingDate END,
										 CASE WHEN @SortBy = 4 THEN SortingDate END DESC,
										 CASE WHEN @SortBy = 5 THEN DealerName END,
										 CASE WHEN @SortBy = 6 THEN DealerName END DESC;

								SELECT * FROM #TblDumpDealer
								WHERE 
								(@FromIndex IS NULL AND @ToIndex IS NULL)
								OR
								(Id BETWEEN @FromIndex AND @ToIndex)
				
								SELECT COUNT(Id) AS RecordCount
								FROM   #TblDumpDealer
                    
								DROP TABLE #TblPageTempDealer  
								DROP TABLE #TblDumpDealer 

							END
					END


			  END

			  --For fetching absure cars under dealer for warranty
			  IF(@Type = 3)
				BEGIN
					IF(@SearchText IS NOT NULL AND @SearchBy = 2)
						BEGIN
							SELECT AbSure_CarDetailsId,CarName,MakeYear,TC_UserId,UserName,IsAgency,StockId,RegNumber,EntryDate,UserName,IsRejected,Warranty,Score,AbSure_WarrantyTypesId,IsSurveyDone,Status
							FROM #TblTemp 
							WHERE AbSure_CarDetailsId = @SearchText  -- AreaId = @AreaId
							ORDER BY CASE WHEN @SortBy = 3 THEN SortingDate END,
									 CASE WHEN @SortBy = 4 THEN SortingDate END DESC
						END
					ELSE IF(@SearchText IS NOT NULL AND @SearchBy = 3)
						BEGIN
							SELECT AbSure_CarDetailsId,CarName,MakeYear,TC_UserId,UserName,IsAgency,StockId,RegNumber,EntryDate,UserName,IsRejected,Warranty,Score,AbSure_WarrantyTypesId,IsSurveyDone,Status
							FROM #TblTemp 
							WHERE StockId = @SearchText  -- AreaId = @AreaId
							ORDER BY CASE WHEN @SortBy = 3 THEN SortingDate END,
									 CASE WHEN @SortBy = 4 THEN SortingDate END DESC
						END
					ELSE
						BEGIN
							SELECT AbSure_CarDetailsId,CarName,MakeYear,TC_UserId,UserName,IsAgency,StockId,RegNumber,EntryDate,UserName,IsRejected,Warranty,Score,AbSure_WarrantyTypesId,IsSurveyDone,Status
							FROM #TblTemp 
							WHERE DealerId = @BranchId AND (AreaId = @AreaId OR OwnerAreaId IN (0,-1)) -- AreaId = @AreaId
							ORDER BY CASE WHEN @SortBy = 3 THEN SortingDate END,
									 CASE WHEN @SortBy = 4 THEN SortingDate END DESC
						END
				END  
			  
			  --For fetching areas of searched dealer
			  IF(@Type = 4)
				BEGIN
					SELECT Area,AreaId,Organization,COUNT(AbSure_CarDetailsId) OVER(PARTITION BY AreaId) AreaCarCount,SortingDate
					INTO  #TblPageTemp 
					FROM  #TblTemp 
					WHERE (@SearchBy = 1 AND (Organization IS NULL OR (LOWER(Organization) LIKE '%' + LOWER(@SearchText) + '%')))
					      OR (@SearchBy = 2 AND AbSure_CarDetailsId = CASE WHEN @SearchBy = 1 THEN 0 ELSE @SearchText END)
						  OR (@SearchBy = 3 AND StockId = CASE WHEN @SearchBy = 1 THEN 0 ELSE @SearchText END )
					GROUP BY Area,AreaId,Organization,AbSure_CarDetailsId,SortingDate
					ORDER BY AreaCarCount

					CREATE TABLE #TblDumpSearch
					( 
						Id INT IDENTITY(1,1) NOT NULL,
						Area   VARCHAR(50),
						AreaId INT NOT NULL,
						AreaCarCount INT NOT NULL,
					)

					INSERT INTO #TblDumpSearch (Area,AreaId,AreaCarCount)  SELECT Area,AreaId,AreaCarCount FROM
					(
						SELECT ROW_NUMBER() OVER(PARTITION BY AreaId ORDER BY  AreaId) AS Row,* FROM #TblPageTemp
					) A
					WHERE A.Row = 1  
                    GROUP BY Area,AreaId,AreaCarCount,SortingDate,Organization
					ORDER BY CASE WHEN @SortBy = 1 THEN AreaCarCount END,
							 CASE WHEN @SortBy = 2 THEN AreaCarCount END DESC,
							 CASE WHEN @SortBy = 3 THEN SortingDate END,
							 CASE WHEN @SortBy = 4 THEN SortingDate END DESC,
							 CASE WHEN @SortBy = 5 THEN Organization END,
							 CASE WHEN @SortBy = 6 THEN Organization END DESC;

					SELECT * FROM #TblDumpSearch
					WHERE 
					(@FromIndex IS NULL AND @ToIndex IS NULL)
                    OR
                    (Id BETWEEN @FromIndex AND @ToIndex)

					
                    SELECT COUNT(Id) AS RecordCount
                    FROM   #TblDumpSearch
                    
					DROP TABLE #TblPageTemp  
					DROP TABLE #TblDumpSearch 
					--SELECT Area,AreaId, AreaCarCount,ROW_NUMBER() OVER(ORDER BY AreaCarCount DESC) AreaNumberForPaging,SurveyDate,Organization
					--INTO  #TblPageTempSearchArea 
					--FROM #TblPageTemp
					--GROUP BY Area,AreaId,AreaCarCount,SurveyDate,Organization
					--ORDER BY AreaCarCount DESC

					--SELECT Area,AreaId, AreaCarCount,SurveyDate,Organization
					--FROM #TblPageTempSearchArea
					--WHERE  
					--(@FromIndex IS NULL AND @ToIndex IS NULL)
					--OR
					--(AreaNumberForPaging BETWEEN @FromIndex AND @ToIndex) 
					--ORDER BY CASE WHEN @SortDirection = 1 AND @SortBy = 1 THEN AreaCarCount END,
					--		 CASE WHEN @SortDirection = 2 AND @SortBy = 1 THEN AreaCarCount END DESC,
					--		 CASE WHEN @SortDirection = 1 AND @SortBy = 2 THEN SurveyDate END,
					--		 CASE WHEN @SortDirection = 2 AND @SortBy = 2 THEN SurveyDate END DESC,
					--		 CASE WHEN @SortDirection = 1 AND @SortBy = 3 THEN Organization END,
					--		 CASE WHEN @SortDirection = 2 AND @SortBy = 3 THEN Organization END DESC;

					--SELECT COUNT(DISTINCT AreaId) AS RecordCount
					--FROM   #TblPageTempSearchArea
                    
					--DROP TABLE #TblPageTemp
					--DROP TABLE #TblPageTempSearchArea	
				END

		    DROP TABLE #TblTemp 
END
