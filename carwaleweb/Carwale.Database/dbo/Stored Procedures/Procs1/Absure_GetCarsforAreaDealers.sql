IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_GetCarsforAreaDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_GetCarsforAreaDealers]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 27th Feb 2015
-- Description:	To fetch the cars according to area and dealers
-- =============================================
CREATE PROCEDURE [dbo].[Absure_GetCarsforAreaDealers]
	@DealerId        VARCHAR(MAX),
	@AreaId          VARCHAR(MAX),
	@IsSurveyDone    BIT,
	@LoggedInUser	 INT,
	@PendingStatus	 INT,
	@CarIds			 VARCHAR(MAX) = NULL OUTPUT
AS
BEGIN
	IF (SELECT DISTINCT RoleId FROM TC_UsersRole WITH(NOLOCK) WHERE UserId = @LoggedInUser AND RoleId = 15) = 15
	BEGIN
		DECLARE @TblTempUsers TABLE (Id INT)
		INSERT INTO @TblTempUsers(Id) VALUES (@LoggedInUser)
		INSERT INTO @TblTempUsers(Id) EXEC TC_GetImmediateChild @LoggedInUser
	END


	SELECT @CarIds =  COALESCE(@CarIds+',','') + CONVERT(VARCHAR,ACD.Id )  
	FROM	AbSure_CarDetails ACD WITH(NOLOCK)
			LEFT JOIN AbSure_CarSurveyorMapping CSMP WITH(NOLOCK) ON ACD.Id= CSMP.AbSure_CarDetailsId 
			--LEFT JOIN TC_Users TU WITH(NOLOCK) ON TU.Id = CSMP.TC_UserId
	WHERE	ISNULL(ACD.IsSurveyDone,0) = @IsSurveyDone AND ISNULL(ACD.Status,0) <> 3   --Status is 3 for cancelled warranty
			AND
			--(@AreaId IS NULL OR ACD.OwnerAreaId IN (SELECT ListMember FROM fnSplitCSV(@AreaId)))
			((@AreaId = 0 AND ACD.OwnerAreaId IN (0,-1)) OR @AreaId IS NULL OR ACD.OwnerAreaId IN (SELECT ListMember FROM fnSplitCSV(@AreaId)))
			AND (
				(CSMP.TC_UserId IN(SELECT ID FROM @TblTempUsers))
				OR
				((SELECT DISTINCT RoleId FROM TC_UsersRole WITH(NOLOCK) WHERE UserId = @LoggedInUser AND RoleId IN(9, 14))IN(9,14) 
				AND ISNULL(CSMP.TC_UserId,0) = ISNULL(CSMP.TC_UserId,0)
				)
			)
			AND(
				(@PendingStatus = 1 AND CSMP.TC_UserId IS NULL
				) -- Agency assignment pending
				OR (@PendingStatus = 2 AND CSMP.TC_UserId IN (SELECT ID FROM TC_Users WITH(NOLOCK) WHERE IsAgency = 1)) -- Surveyor assignment pending
				OR (@PendingStatus = 3 AND CSMP.TC_UserId IN (SELECT ID FROM TC_Users WITH(NOLOCK) WHERE IsAgency <> 1) AND ISNULL(ACD.IsSurveyDone,0) = 0) -- Inspection pending
				OR (@PendingStatus = 4 and ACD.IsSurveyDone = 1) -- Done
			)
			AND (@DealerId IS NULL OR (ACD.DealerId IN (SELECT ListMember FROM fnSplitCSV(@DealerId)) AND ACD.OwnerAreaId IN (SELECT ListMember FROM fnSplitCSV(@AreaId))))
	
END