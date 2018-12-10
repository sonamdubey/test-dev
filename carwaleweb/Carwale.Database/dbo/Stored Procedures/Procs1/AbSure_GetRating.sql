IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetRating]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetRating]
GO

	
-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date: April 17,2015
-- Description:	To fetch rating of surveyor
-- EXEC [dbo].[AbSure_GetRating] 13240
-- Modified By 1 : Ruchira Patil (23rd April 2015) (to fetch ratings of the agencies when the admin logs in)
-- Modified By : vinay Kumar Prajapati : (31st july 2015 ) added order by userName 
-- Modified by : Vinay Kumar Prajapati : 26th Aug 2015 Added ROUND Function ..... 
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetRating]
@UserId BIGINT
AS
BEGIN
	SET NOCOUNT ON; 
DECLARE @RoleId INT
	DECLARE @IsAxaAdmin BIT=0
	DECLARE @BranchId AS INT
	SELECT @BranchId = BranchId FROM TC_Users WHERE ID = @UserId
	
	DECLARE @TempTable  TABLE
	(
		Id INT IDENTITY(1,1), 
		ChildId INT     ---  this is use  for get  all id  under given userId
	)

	IF (SELECT IsAgency FROM TC_Users AS TU WITH(NOLOCK) WHERE TU.Id = @UserId AND TU.Id <>20074 ) = 1 --Axa Agency  (Get All Surveyor Under Specific Agency)
		BEGIN
			INSERT INTO @TempTable(ChildId) EXEC TC_GetImmediateChild @UserId --To get child if user is axa super admin or agency
		END
	ELSE IF @UserId = 20074  -- SuperAdmin - 9 (Get all Agency Under Super Admin Axa On Live And staging Axa admin Id  =20074 and  on local Id= 13175 )
		BEGIN
			SET @IsAxaAdmin=1
			INSERT INTO @TempTable(ChildId)  EXEC TC_GetLevelWiseChild @UserId,2  -- to get All surveyorsId Under Specific Axa Admin 
		END
	ELSE IF (SELECT  ISNULL(IsAgency,0) FROM TC_Users AS TU WITH(NOLOCK) WHERE TU.Id = @UserId) = 0
		BEGIN
			INSERT INTO @TempTable(ChildId) VALUES (@UserId) -- surveyor
		END

 --   IF (SELECT DISTINCT RoleId FROM TC_UsersRole WITH(NOLOCK) WHERE UserId = @UserId AND RoleId = 15) = 15 --Axa Agency - 15  (Get All Surveyor Under Specific Agency)
	--	BEGIN
	--		INSERT INTO @TempTable(ChildId) EXEC TC_GetImmediateChild @UserId --To get child if user is axa super admin or agency
	--	END
	--ELSE IF (SELECT DISTINCT RoleId FROM TC_UsersRole WITH(NOLOCK) WHERE UserId = @UserId AND RoleId = 9) = 9   -- SuperAdmin - 9 (Get all Agency Under Super Admin Axa )
	--	BEGIN
	--	SET @IsAxaAdmin=1
	--		INSERT INTO @TempTable(ChildId)  EXEC TC_GetLevelWiseChild @UserId,2  -- to get All surveyorsId Under Specific Axa Admin 
	--	END



	IF @IsAxaAdmin=1
		BEGIN 
			SELECT		ISNULL(TU.IsAgency,0) IsAgency,TU.Id AS UserId ,TU.UserName, ARC.[CategoryText] AS CategoryText,
						ISNULL(ARC.Absure_RatingCategoryId,0) RatingCategoryId,
						CAST(ISNULL(SUM(AIR.RatingValue),0)*1.0/COUNT(ISNULL(ARC.Absure_RatingCategoryId,1)) AS DECIMAL(10,2)) AS RatingValue,
						(SELECT Id FROM TC_Users  WITH(NOLOCK) WHERE HierId =  TU.HierId.GetAncestor(1) AND BranchId =@BranchId) AS ParentID,
					    (SELECT UserName FROM TC_Users  WITH(NOLOCK) WHERE HierId =  TU.HierId.GetAncestor(1) AND BranchId =@BranchId) AS Parent
				        INTO #AxaTempTable
			FROM		TC_Users TU WITH(NOLOCK)
			LEFT JOIN	Absure_InspectionFeedback AIF WITH(NOLOCK) ON AIF.SurveyorId = TU.Id  AND AIF.BranchId <> -1
			LEFT JOIN	Absure_InspectionRating AIR WITH(NOLOCK) ON AIR.InspectionFeedbackId = AIF.Absure_InspectionFeedbackId
			LEFT JOIN	Absure_RatingCategory ARC  WITH(NOLOCK) ON ARC.Absure_RatingCategoryId = AIR.RatingCategoryId AND ARC.IsActive=1
			WHERE		TU.Id IN (SELECT ChildId FROM @TempTable)
			GROUP BY	AIF.SurveyorId,TU.UserName ,ARC.[CategoryText],ARC.Absure_RatingCategoryId,TU.Id,
			ISNULL(TU.IsAgency,0),TU.HierId.GetAncestor(1)


			SELECT '1' IsAgency,TD.ParentID AS UserId,TD.Parent AS UserName, TD.RatingCategoryId,TD.CategoryText ,CAST(ISNULL(AVG(TD.RatingValue),0) AS DECIMAL(10,2)) AS RatingValue				  			 						   
			FROM #AxaTempTable AS TD				 
 			WHERE TD.ParentID IS NOT NULL  --AND TD.RatingCategoryId<>0 
			GROUP BY TD.ParentID ,TD.IsAgency,TD.Parent ,TD.RatingCategoryId,TD.CategoryText 
			ORDER BY  UserName

			DROP TABLE #AxaTempTable
		END
	ELSE
		BEGIN	    

			SELECT		ISNULL(TU.IsAgency,0) IsAgency,TU.Id AS UserId,TU.UserName, ARC.[CategoryText] AS CategoryText,
						ISNULL(ARC.Absure_RatingCategoryId,0) RatingCategoryId,  CAST((Sum(AIR.RatingValue)*1.0)/ISNULL(COUNT(ARC.Absure_RatingCategoryId),1)  AS DECIMAL(10,2)) RatingValue
			FROM		TC_Users TU WITH(NOLOCK)
			LEFT JOIN	Absure_InspectionFeedback AIF WITH(NOLOCK) ON AIF.SurveyorId = TU.Id AND AIF.BranchId <> -1
			LEFT JOIN	Absure_InspectionRating AIR WITH(NOLOCK) ON AIR.InspectionFeedbackId = AIF.Absure_InspectionFeedbackId
			LEFT JOIN	Absure_RatingCategory ARC  WITH(NOLOCK) ON ARC.Absure_RatingCategoryId = AIR.RatingCategoryId AND ARC.IsActive=1
			WHERE		TU.Id IN (SELECT ChildId FROM @TempTable) AND AIF.BranchId <> -1
			GROUP BY	AIf.SurveyorId,TU.UserName ,ARC.[CategoryText],ARC.Absure_RatingCategoryId,TU.Id,
			ISNULL(TU.IsAgency,0)
			ORDER BY  TU.UserName 
		END
END


