IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UserDetailsLoad_V16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UserDetailsLoad_V16]
GO

	-- =============================================
-- Author:		<Author, Nilesh Utture>
-- Create date: <Create Date, 13th March, 2013>
-- Description:	<Description, Loads User Details on Add/Edit user page>
-- Modified By Nilesh on 18-06-2013 for correction of the reporting to data
-- Modified By Manish on 26-06-2013 for correction of the reporting to data we are not showing child in the reporting to field.
-- exec TC_UserDetailsLoad 6703,3,null
-- Modified BY: Umesh Ojha on 29 jul 2013 for removing the is new dealer concept(no more requirement for this things)
-- Modified by Vishal on 09-04-2014 adding VTO Number for VW users
-- Modified By Ashwini dhamankar on 16/10/2014, fetched BranchLocationId\
-- Modified By : Tejashree Patil on 30-10-2014, Added @ApplicationId to identify application and joined with vwAllMMV view.
-- Modified By : Tejashree Patil on 16-12-2014, Fetched role(Surveyor) for DealerTypeId = 4.
-- Modified By : Ashwini Dhamankar on 17-12-2014, Fetched CityId and AreaId
-- Modified By : Ruchira Patil on 17Th Feb 2015 , fetch super admin users for reporting to in axa panel and added a parameter @LoggedInUser
-- Modified By Vivek Gupta on 24-02-2015 , added IsModelNew in model selects
-- Modified By : Suresh Prajapati on 26th June, 2015
-- Description : Fetched User's serving area(s) from TC_UserAreaMapping Table
-- Modified By : Afrose, added imageurl from select statement
-- exec TC_UserDetailsLoad 20554,2,88932,1,88932,0
-- Modified By : Suresh Prajapati on 11th Mar, 2016
-- Description : Removed Password fetching and Added WITH (NOLOCK)
-- Modified By : Nilima More On 20th July 2016,fetch roleId based on dealerType.
-- Modified By : Ruchira Patil on 24th aug 2016,fetch roles (DP,CRE,Team Leader) for Insurance.
-- Modified by Ruchira Patil on 26th Sept 2016 (modified join to vwAllMMV to fetch DivisionList and commented Futuristic and IsDeleted condition)
-- =============================================
CREATE PROCEDURE [dbo].[TC_UserDetailsLoad_V16.9.1]
	-- Add the parameters for the stored procedure here
	@BranchId INT
	,@TC_DealerTypeId TINYINT
	,@UserId INT
	,@ApplicationId TINYINT = 1
	,-- Modified By : Tejashree Patil on 30-10-2014, Added @ApplicationId to identify application and joined with vwAllMMV view.
	@LoggedInUser INT
	,@IsAxaAgency BIT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @HavingWebsiteAccess BIT
	DECLARE @DealerType TINYINT = 4
	DECLARE @RoleCount TINYINT

	--DECLARE @IsNewUser BIT = 0
	SELECT @HavingWebsiteAccess = HavingWebsite
	FROM Dealers WITH (NOLOCK)
	WHERE ID = @BranchId

	IF @HavingWebsiteAccess = 1
	BEGIN
		SET @DealerType = 0
	END

	IF @TC_DealerTypeId = 3
	BEGIN
		SELECT TC_RolesMasterId AS Id
			,RoleName AS NAME
		FROM TC_RolesMaster WITH (NOLOCK)
		WHERE (
				TC_DealerTypeId IN (
					1
					,2
					,@DealerType
					)
				OR TC_DealerTypeId IS NULL
				)
			AND IsActive = 1
			AND IsVisible = 1
	END
	ELSE
	BEGIN
		IF @TC_DealerTypeId = 4
			BEGIN
				SELECT TC_RolesMasterId AS Id
					,RoleName AS NAME
				FROM TC_RolesMaster WITH (NOLOCK)
				WHERE (
						TC_DealerTypeId IN (
							@TC_DealerTypeId
							,@DealerType
							)
						)
					AND IsActive = 1
			END
		ELSE
		---- SERVICE CENTER DEALER//Modified By : Nilima More On 20th July 2016,fetch roleId based on dealerType.
			IF @TC_DealerTypeId = 5
			BEGIN
					SELECT TC_RolesMasterId AS Id,RoleName AS NAME
					FROM TC_RolesMaster WITH (NOLOCK)
					WHERE (TC_DealerTypeId IN (5) 
					OR TC_RolesMasterId = 1) AND IsActive = 1
			END
			ELSE
			----Insurance Dealer // Modified By Ruchira Patil on 24th aug 2016,fetch roles (DP,CRE,Team Leader) for Insurance.
				IF @TC_DealerTypeId = 8
				BEGIN 
					SELECT TC_RolesMasterId AS Id,RoleName AS NAME
					FROM TC_RolesMaster WITH (NOLOCK)
					WHERE (TC_DealerTypeId IN (8) 
					OR TC_RolesMasterId IN(1,12)) AND IsActive = 1
				END
				ELSE
					SELECT TC_RolesMasterId AS Id
					,RoleName AS NAME
					FROM TC_RolesMaster WITH (NOLOCK)
					WHERE (
							TC_DealerTypeId IN (
								@TC_DealerTypeId
								,@DealerType
								) 
							OR TC_DealerTypeId IS NULL
							)
						AND IsActive = 1
						AND IsVisible = 1
	END


	--IF EXISTS(SELECT U.Id FROM TC_Users U JOIN TC_UsersRole R ON R.UserId = U.Id AND U.BranchId = @BranchId AND R.RoleId=10)
	--BEGIN
	--	SET @IsNewUser = 1
	--END
	-- Modified By : Tejashree Patil on 30-10-2014, Added WITH (NOLOCK), and joined with vwAllMMV view instead of CarModel.
	SELECT DISTINCT MMV.ModelId AS Id
		,MMV.Model AS ModelName
	FROM TC_DealerMakes D WITH (NOLOCK)
	--INNER JOIN CarModels M ON D.MakeId=M.CarMakeId
	INNER JOIN vwAllMMV MMV WITH (NOLOCK) ON D.MakeId = MMV.MakeId
	WHERE D.DealerId = @BranchId
		--AND M.Futuristic = 0 AND M.New = 1 AND IsDeleted = 0 
		AND MMV.New = 1
		AND MMV.ApplicationId = ISNULL(@ApplicationId, 1)
		AND IsModelNew = 1
	ORDER BY ModelName

	IF @TC_DealerTypeId = 4
	BEGIN
		IF EXISTS (
				SELECT RoleId
				FROM TC_UsersRole WITH (NOLOCK)
				WHERE UserId = @LoggedInUser
					AND RoleId = 9
				)
		BEGIN
			SELECT DISTINCT (U.Id) AS Id
				,U.UserName AS NAME
			FROM TC_Users U WITH (NOLOCK)
			INNER JOIN TC_UsersRole R WITH (NOLOCK) ON R.UserId = U.Id
				AND R.RoleId = 9 --SUPER ADMIN
			WHERE U.BranchId = @BranchId
				AND U.IsActive = 1 --AND U.IsAgency = 1
		END
		ELSE
		BEGIN
			SELECT DISTINCT (U.Id) AS Id
				,U.UserName AS NAME
			FROM TC_Users U WITH (NOLOCK)
			WHERE U.Id = @LoggedInUser
		END
	END
	ELSE
	BEGIN
		IF @UserId IS NOT NULL
		BEGIN
			DECLARE @TblAllChild TABLE (Id INT)

			INSERT INTO @TblAllChild
			EXEC TC_GetALLChild @UserId

			SELECT DISTINCT (U.Id) AS Id
				,U.UserName AS NAME
			FROM --Modified By Manish on 26-06-2013 for correction of the reporting to data
				TC_Users U WITH (NOLOCK)
			INNER JOIN TC_UsersRole R WITH (NOLOCK) ON R.UserId = U.Id
				AND R.RoleId IN (
					1
					,7
					)
				AND U.Id NOT IN (@UserId)
			WHERE U.BranchId = @BranchId
				AND U.IsActive = 1
				AND U.Id NOT IN (
					SELECT ID
					FROM @TblAllChild
					)
		END
		ELSE
		BEGIN
			SELECT DISTINCT (U.Id) AS Id
				,U.UserName AS NAME
			FROM TC_Users U WITH (NOLOCK)
			INNER JOIN TC_UsersRole R WITH (NOLOCK) ON R.UserId = U.Id
				AND R.RoleId IN (
					1
					,7
					)
			WHERE U.BranchId = @BranchId
				AND U.IsActive = 1
		END
	END

	IF (@UserId IS NOT NULL)
	BEGIN
		DECLARE @RoleList VARCHAR(500)
		DECLARE @DivisionList VARCHAR(500)

		SELECT @RoleList = COALESCE(@RoleList + ',', '') + (convert(VARCHAR, U.RoleId) + '_' + R.RoleName)
		FROM TC_UsersRole U WITH (NOLOCK)
		INNER JOIN TC_RolesMaster R WITH (NOLOCK) ON U.RoleId = R.TC_RolesMasterId
		WHERE UserId = @UserId
			AND U.RoleId <> 9

		--Modified by Ruchira Patil on 26th Sept 2016 (join on vwAllMMV and commented Futuristic and IsDeleted condition)
		SELECT @DivisionList = COALESCE(@DivisionList + ',', '') + (convert(VARCHAR, P.ModelId) + '_' + M.Model)
		FROM TC_UserModelsPermission P WITH (NOLOCK)
		--INNER JOIN CarModels M WITH (NOLOCK) ON P.ModelId = M.ID
		INNER JOIN vwAllMMV M WITH (NOLOCK) ON p.ModelId = M.ModelId
		WHERE 
		--M.Futuristic = 0 AND IsDeleted = 0
			M.New = 1
			AND IsModelNew = 1
			AND P.TC_UsersId = @UserId

		DECLARE @TC_ReportingTo INT = NULL

		IF EXISTS (
				SELECT U.Id
				FROM TC_Users U WITH (NOLOCK)
				JOIN TC_UsersRole R WITH (NOLOCK) ON R.UserId = U.Id
					AND U.BranchId = @BranchId
					AND R.RoleId = 10
				)
		BEGIN
			/*DECLARE @EMP HIERARCHYID







			DECLARE @lvl smallint







			SELECT @emp=HIERID,@lvl=lvl FROM TC_Users WHERE ID=@USERID







			SELECT @TC_ReportingTo = ID FROM TC_Users WHERE







			HIERID.IsDescendantOf(@EMP) =2







			and lvl=@lvl-1







			and BranchId=@BranchId*/
			------ Used correct sqls for reporting to data on 18-06-2013 
			EXEC TC_GetImmediateParent @UserId = @USERID
				,@TC_ReportingTo = @TC_ReportingTo OUTPUT

			IF EXISTS (
					SELECT U.Id
					FROM TC_Users U WITH (NOLOCK)
					INNER JOIN TC_UsersRole R WITH (NOLOCK) ON R.UserId = @TC_ReportingTo
						AND U.Id = @TC_ReportingTo
						AND R.RoleId = 10
					)
			BEGIN
				SET @TC_ReportingTo = NULL
			END
		END
				-- Modified By Ashwini dhamankar on 16/10/2014
				--SELECT R.RoleId
				--	,U.Email
				--	,U.Password
				--	,@TC_ReportingTo AS ReportingTo
				--	,@RoleList AS RoleList
				--	,@DivisionList AS DivisionList
				--	,U.UserName
				--	,U.Mobile
				--	,U.Sex
				--	,U.Address
				--	,U.DOB
				--	,U.DOJ
				--	,U.VTONumbers
				--	,U.BranchLocationId
				--	,U.CityId
				--	,U.AreaId
				--FROM TC_Users U WITH (NOLOCK) -- Modified by Vishal on 09-04-2014 adding VTO Number for VW users
				--LEFT JOIN TC_UsersRole R WITH (NOLOCK) ON R.UserId = U.Id
				--	AND R.RoleId = 9
				--WHERE U.Id = @UserId
				----Added By : Suresh Prajapati
				--IF (@IsAxaAgency = 1)
				--BEGIN
				--	SELECT AM.AreaId
				--	FROM TC_UserAreaMapping AS AM WITH (NOLOCK)
				--	INNER JOIN TC_Users AS U WITH (NOLOCK) ON ISNULL(U.Id,@LoggedInUser) = AM.TC_UserId
				--	WHERE U.Id = ISNULL(@UserId, @LoggedInUser)
				--		AND AM.IsActive = 1
				--END
				--ELSE
				--BEGIN
				--	SELECT AM.AreaId
				--	FROM TC_UserAreaMapping AS AM WITH (NOLOCK)
				--	INNER JOIN TC_Users AS U WITH (NOLOCK) ON ISNULL(U.Id,@LoggedInUser) = AM.TC_UserId
				--	WHERE U.Id = ISNULL(@UserId, @LoggedInUser)
				--		AND AM.IsActive = 1
				--		AND AM.IsAssigned = 0
				--END
	END

	SELECT R.RoleId
		,U.Email
		--,U.Password
		,@TC_ReportingTo AS ReportingTo
		,@RoleList AS RoleList
		,@DivisionList AS DivisionList
		,U.UserName
		,U.Mobile
		,U.Sex
		,U.Address
		,U.DOB
		,U.DOJ
		,U.VTONumbers
		,U.BranchLocationId
		,U.CityId
		,U.AreaId
		,U.ImageUrl -- Added by Afrose
		,U.IsShownCarwale
	FROM TC_Users U WITH (NOLOCK) -- Modified by Vishal on 09-04-2014 adding VTO Number for VW users
	LEFT JOIN TC_UsersRole R WITH (NOLOCK) ON R.UserId = U.Id
		AND R.RoleId = 9
	WHERE U.Id = @UserId

	--=============================================
	-- Added By : Suresh Prajapati
	-- Surveyor's area(s) loading
	--=============================================
	IF EXISTS (
			SELECT Id
			FROM TC_Users WITH (NOLOCK)
			WHERE Id = ISNULL(@UserId, @LoggedInUser)
				AND IsAgency <> 1
			)
	BEGIN
		--Surveyor
		--SELECT DISTINCT AM.AreaId
		--	,A.CityId
		--	,C.NAME AS CityName
		--FROM TC_UserAreaMapping AS AM WITH (NOLOCK)
		--INNER JOIN TC_Users AS U WITH (NOLOCK) ON U.Id = AM.TC_UserId
		--INNER JOIN Areas AS A WITH (NOLOCK) ON A.ID = AM.AreaId
		--INNER JOIN Cities AS C WITH (NOLOCK) ON C.ID = A.CityId
		--WHERE AM.TC_UserId = ISNULL(@UserId, @LoggedInUser)		
		CREATE TABLE #AreaIdTable (
			AreaId INT
			,CityId INT
			,CityName VARCHAR(50)
			,IsCheck BIT
			)

		--check Agency's unassigned area(s)
		IF EXISTS (
				SELECT 1
				FROM TC_UserAreaMapping WITH (NOLOCK)
				WHERE TC_UserId = @LoggedInUser
					AND IsAssigned = 0
					AND IsActive = 1
				)
		BEGIN
			INSERT INTO #AreaIdTable
			SELECT AM.AreaId
				,C.ID
				,C.NAME
				,0
			FROM TC_UserAreaMapping AS AM WITH (NOLOCK)
			INNER JOIN Areas AS A WITH (NOLOCK) ON A.ID = AM.AreaId
			INNER JOIN Cities AS C WITH (NOLOCK) ON C.ID = A.CityId
			WHERE AM.TC_UserId = @LoggedInUser
				AND IsAssigned = 0
				AND IsActive = 1
		END

		--Check for surveor's existing area
		IF EXISTS (
				SELECT 1
				FROM TC_UserAreaMapping WITH (NOLOCK)
				WHERE TC_UserId = @UserId
					AND IsActive = 1
					AND IsAssigned = 1
				)
		BEGIN
			INSERT INTO #AreaIdTable
			SELECT AM.AreaId
				,C.ID
				,C.NAME
				,1
			FROM TC_UserAreaMapping AS AM WITH (NOLOCK)
			INNER JOIN Areas AS A WITH (NOLOCK) ON A.ID = AM.AreaId
			INNER JOIN Cities AS C WITH (NOLOCK) ON C.ID = A.CityId
			WHERE AM.TC_UserId = @UserId
				AND IsActive = 1
				AND IsAssigned = 1
		END

		SELECT *
		FROM #AreaIdTable WITH (NOLOCK)

		SELECT C.NAME AS CityName
		FROM TC_Users TC WITH (NOLOCK)
		INNER JOIN Cities C WITH (NOLOCK) ON C.ID = TC.CityId
		WHERE TC.Id = @LoggedInUser

		DROP TABLE #AreaIdTable
	END
	ELSE
	BEGIN
		--Agency's area(s) loading
		IF (ISNULL(@UserId, 0) = 0)
		BEGIN
			-- i.e. Agency is adding a new a user(Surveyor), hence show only active and unassigned area(s) of agency
			SELECT AM.AreaId
				,A.CityId
				,A.NAME AS AreaName
				,C.NAME AS CityName
			FROM TC_UserAreaMapping AS AM WITH (NOLOCK)
			INNER JOIN Areas AS A WITH (NOLOCK) ON A.ID = AM.AreaId
			INNER JOIN Cities AS C WITH (NOLOCK) ON C.ID = A.CityId
			WHERE AM.TC_UserId = @LoggedInUser --Agency's Id
				AND AM.IsActive = 1
				AND AM.IsAssigned = 0

			IF (@@ROWCOUNT = 0)
				SELECT C.NAME AS CityName
				FROM TC_Users TC WITH (NOLOCK)
				INNER JOIN Cities C WITH (NOLOCK) ON C.ID = TC.CityId
				WHERE TC.Id = @LoggedInUser
		END
	END

	IF (ISNULL(@IsAxaAgency, 0) = 0)
	BEGIN
		IF (ISNULL(@UserId, 0) <> 0)
		BEGIN
			SELECT AM.AreaId
				,A.CityId
				,A.NAME AS AreaName
				,C.NAME AS CityName
				,AM.IsActive
				,AM.IsAssigned
			FROM TC_UserAreaMapping AS AM WITH (NOLOCK)
			INNER JOIN Areas AS A WITH (NOLOCK) ON A.ID = AM.AreaId
			INNER JOIN Cities AS C WITH (NOLOCK) ON C.ID = A.CityId
			WHERE AM.TC_UserId = @UserId --Agency's Id
				AND AM.IsActive = 1
				--AND AM.IsAssigned = 1
		END
	END
END


