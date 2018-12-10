IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UserDetailsLoad_V16_11_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UserDetailsLoad_V16_11_1]
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
-- exec TC_UserDetailsLoad_V16_11_1 20553,8,88927,1,88927
-- Modified By : Suresh Prajapati on 11th Mar, 2016
-- Description : Removed Password fetching and Added WITH (NOLOCK)
-- Modified By : Nilima More On 20th July 2016,fetch roleId based on dealerType.
-- Modified By : Ruchira Patil on 24th aug 2016,fetch roles (DP,CRE,Team Leader) for Insurance.
-- Modified by Ruchira Patil on 26th Sept 2016 (modified join to vwAllMMV to fetch DivisionList and commented Futuristic and IsDeleted condition)
-- Modified by : Kritika Choudhary on 30th Sep 2016, for feedback calling dealer,show only sales EX (N) role(dealertypeid=10)
-- Modified by : Khushaboo Patil on 4th sept 2016 ,for feedback calling dealer,show only sales EX (N) role(dealertypeid=10) remove dp role
-- Modified By Kritika Choudhary on 19th oct 2016,fetch role sales ex for Insurance.
-- Modified By Ruchira Patil on 3rd Nov 2016 (optimized the SP and removed all queries related to dealertype=4(AXA AUTOBIZ))
-- =============================================
CREATE PROCEDURE [dbo].[TC_UserDetailsLoad_V16_11_1] @BranchId INT
	,@TC_DealerTypeId TINYINT
	,@UserId INT
	,@ApplicationId TINYINT = 1
	,@LoggedInUser INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @HavingWebsiteAccess BIT
		,@DealerType TINYINT

	SELECT @HavingWebsiteAccess = HavingWebsite
	FROM Dealers WITH (NOLOCK)
	WHERE ID = @BranchId

	IF @HavingWebsiteAccess = 1
		SET @DealerType = 0

	------To fetch Roles -START--------
	SELECT TC_RolesMasterId AS Id
		,RoleName AS NAME
	FROM TC_RolesMaster WITH (NOLOCK)
	WHERE (
			(
				@TC_DealerTypeId = 2
				AND (
					TC_DealerTypeId IN (
						2
						,@DealerType
						)
					OR TC_DealerTypeId IS NULL
					)
				) -- NCD
			OR (
				@TC_DealerTypeId = 3
				AND (
					TC_DealerTypeId IN (
						1
						,2
						,@DealerType
						)
					OR TC_DealerTypeId IS NULL
					)
				) -- UCD_NCD
			OR (
				@TC_DealerTypeId = 5
				AND (
					TC_DealerTypeId IN (@TC_DealerTypeId)
					OR TC_RolesMasterId = 1
					)
				) -- Service
			OR (
				@TC_DealerTypeId = 8
				AND (
					TC_DealerTypeId IN (@TC_DealerTypeId)
					OR TC_RolesMasterId IN (
						1
						,4
						,12
						)
					)
				) -- Insurance
			OR (
				@TC_DealerTypeId = 10
				AND TC_RolesMasterId = 4
				) -- FeedbackCalling
			)
		AND IsActive = 1
		AND IsVisible = 1

	------To fetch divisions -START--------
	SELECT DISTINCT MMV.ModelId AS Id
		,MMV.Model AS ModelName
	FROM TC_DealerMakes D WITH (NOLOCK)
	INNER JOIN vwAllMMV MMV WITH (NOLOCK) ON D.MakeId = MMV.MakeId
	WHERE D.DealerId = @BranchId
		AND MMV.New = 1
		AND MMV.ApplicationId = ISNULL(@ApplicationId, 1)
		AND IsModelNew = 1
	ORDER BY ModelName

	------To fetch ReportingTo (all users) -START--------	
	SELECT DISTINCT (U.Id) AS Id
		,U.UserName AS NAME
	FROM TC_Users U WITH (NOLOCK)
	INNER JOIN TC_UsersRole R WITH (NOLOCK) ON R.UserId = U.Id
		AND (
			@UserId IS NULL
			OR U.Id NOT IN (@UserId)
			)
	WHERE U.BranchId = @BranchId
		AND U.IsActive = 1
		AND U.IsCarwaleUser = 0

	------To fetch Userdetails i.e RoleList, DivisionList, TC_ReportingTo Start------------
	IF (@UserId IS NOT NULL)
	BEGIN
		DECLARE @RoleList VARCHAR(500)
			,@DivisionList VARCHAR(500)
			,@TC_ReportingTo INT = NULL

		SELECT @RoleList = COALESCE(@RoleList + ',', '') + (convert(VARCHAR, U.RoleId) + '_' + R.RoleName)
		FROM TC_UsersRole U WITH (NOLOCK)
		INNER JOIN TC_RolesMaster R WITH (NOLOCK) ON U.RoleId = R.TC_RolesMasterId
		WHERE UserId = @UserId
			AND U.RoleId <> 9

		SELECT @DivisionList = COALESCE(@DivisionList + ',', '') + (convert(VARCHAR, P.ModelId) + '_' + M.NAME)
		FROM TC_UserModelsPermission P WITH (NOLOCK)
		INNER JOIN CarModels M WITH (NOLOCK) ON P.ModelId = M.ID
		WHERE P.TC_UsersId = @UserId

		IF EXISTS (
				SELECT U.Id
				FROM TC_Users U WITH (NOLOCK)
				JOIN TC_UsersRole R WITH (NOLOCK) ON R.UserId = U.Id
					AND U.BranchId = @BranchId
					AND R.RoleId = 10
				)
		BEGIN
			EXEC TC_GetImmediateParent @UserId = @USERID
				,@TC_ReportingTo = @TC_ReportingTo OUTPUT

			IF EXISTS (
					SELECT U.Id
					FROM TC_Users U WITH (NOLOCK)
					INNER JOIN TC_UsersRole R WITH (NOLOCK) ON R.UserId = @TC_ReportingTo
						AND U.Id = @TC_ReportingTo
						AND R.RoleId = 10
					)
				SET @TC_ReportingTo = NULL
		END
	END

	-------To fetch all prefill data of user Start-----------
	SELECT R.RoleId
		,U.Email
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
		,AreaId
		,U.ImageUrl
		,U.IsShownCarwale
	FROM TC_Users U WITH (NOLOCK)
	LEFT JOIN TC_UsersRole R WITH (NOLOCK) ON R.UserId = U.Id
		AND R.RoleId = 9
	WHERE U.Id = @UserId
END
