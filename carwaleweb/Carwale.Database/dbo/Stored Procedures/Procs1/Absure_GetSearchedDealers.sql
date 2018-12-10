IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_GetSearchedDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_GetSearchedDealers]
GO

	-- =============================================
-- Author      : Chetan Navin
-- Create date : 13th Mar 2015
-- Description : To get areas of searched dealers
-- Modified By : Vinay Prajapati, Added condition of  'OR D.IsInspection=1' WHERE D.IsWarranty = 1
-- EXEC Absure_GetSearchedDealers 'The Automotive Pvt. Ltd',1,NULL,0,1,10,13175,4
-- =============================================
CREATE PROCEDURE [dbo].[Absure_GetSearchedDealers] 
	@DealerName      VARCHAR(200) = NULL,
	@CityId          INT,
	@AreaId          INT = NULL,
	@IsSurveyDone    BIT,
	@FromIndex       INT, 
	@ToIndex         INT,
	@LoggedInUser	 INT,
	@PendingStatus	 INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
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
				SELECT D.ID AS DealerId,D.Organization AS Dealer,A.ID AS AreaId,A.Name AS Area,ACD.Id AS AbSure_CarDetailsId
				FROM Dealers D WITH(NOLOCK) 
				RIGHT JOIN AbSure_CarDetails ACD WITH(NOLOCK) ON D.ID = ACD.DealerId --AND ISNULL(ACD.IsSurveyDone,0) = @IsSurveyDone --AND ISNULL(ACD.Status,0) <> 3    --Status is 3 for cancelled warranty
				LEFT JOIN AbSure_CarSurveyorMapping CSMP WITH(NOLOCK) ON ACD.Id= CSMP.AbSure_CarDetailsId 
				LEFT JOIN Areas A WITH(NOLOCK) ON A.ID = ACD.OwnerAreaId
				LEFT JOIN TC_Users TU WITH(NOLOCK) ON TU.Id = CSMP.TC_UserId
				WHERE (D.IsWarranty = 1 OR D.IsInspection=1)
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
					OR (@PendingStatus = 4 AND ISNULL(ACD.IsSurveyDone,0) = 1 AND ACD.FinalWarrantyTypeId IS NULL AND ISNULL(ACD.Status,0) <> 3 AND ISNULL(ACD.IsRejected,0) = 0) -- Approval Pending 
					OR (@PendingStatus = 5 AND ISNULL(ACD.IsSurveyDone,0) = 1 AND (ACD.FinalWarrantyTypeId IS NOT NULL OR ISNULL(ACD.IsRejected,0) = 1) AND ISNULL(ACD.Status,0) <> 3) -- Approval Done
					OR (@PendingStatus = 6 AND ACD.Status = 3) -- Cancelled
					)
				)

				SELECT Area,AreaId,Dealer,COUNT(AbSure_CarDetailsId) OVER(PARTITION BY AreaId) AreaCarCount
                INTO  #TblTemp --#TblPageTempArea 
				FROM Cte1 
				GROUP BY Area,AreaId,Dealer,AbSure_CarDetailsId

				SELECT Area,AreaId,Dealer, AreaCarCount,ROW_NUMBER() OVER(ORDER BY AreaCarCount DESC) AreaNumberForPaging
                INTO  #TblPageTempArea 
				FROM #TblTemp
				GROUP BY Area,AreaId,Dealer,AreaCarCount
				ORDER BY AreaCarCount DESC

				SELECT DISTINCT Area,AreaId, AreaCarCount
                FROM #TblPageTempArea
                WHERE  
                (@FromIndex IS NULL AND @ToIndex IS NULL)
                OR
                (AreaNumberForPaging  BETWEEN @FromIndex AND @ToIndex) AND (Dealer IS NULL OR Dealer LIKE @DealerName)
                ORDER BY AreaCarCount DESC

                SELECT COUNT(DISTINCT AreaId) AS RecordCount
                FROM   #TblPageTempArea
				WHERE Dealer IS NULL OR Dealer LIKE @DealerName

				DROP TABLE #TblTemp
				DROP TABLE #TblPageTempArea
END



































