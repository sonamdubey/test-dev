IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AxisBank_ViewCurrentUserDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AxisBank_ViewCurrentUserDetails]
GO

	

-- =============================================
-- Author:		Kumar Vikram
-- Create date: 18.12.2013
-- Description:	Gets the existing current User details
-- exec AxisBank_ViewCurrentUserDetails  '%ty%' , 11,20
-- =============================================
CREATE PROCEDURE [dbo].[AxisBank_ViewCurrentUserDetails] 
@SearchUser VARCHAR(50) = NULL,
@StartIndex int = 1,
@EndIndex int = 10 
AS
BEGIN
	SET NOCOUNT ON

	IF (
			@SearchUser IS NULL
			OR @SearchUser = ''
			)
	BEGIN
		WITH CTE
		AS (

		select *,ROW_NUMBER() OVER (
					ORDER BY UserId DESC
					) AS RowIndex from (
			SELECT U.UserId
				,LoginId
				,FirstName
				,LastName
				,CreatedOn
				,IsActive
				,IsAdmin
				,Email
				,ul.ActivityDateTime
				,ROW_NUMBER() OVER (
					PARTITION BY u.userid ORDER BY ul.ActivityDateTime DESC
					) AS RowNo
				
			FROM AxisBank_Users U
			LEFT JOIN AxisBank_UserActivitiesLog UL ON U.UserId = UL.UserId
				AND UL.ActivityTypeId = 2
				) as tab
				where rowno=1
			)
		SELECT *
		FROM CTE
		WHERE RowIndex >= @StartIndex
			AND RowIndex <= @EndIndex
		ORDER BY USERID DESC

		SELECT Count(*)
		FROM AxisBank_Users
	END
	ELSE
	BEGIN
		WITH CTE
		AS (

		select *,ROW_NUMBER() OVER (
					ORDER BY UserId DESC
					) AS RowIndex from (
			SELECT U.UserId
				,LoginId
				,FirstName
				,LastName
				,CreatedOn
				,IsActive
				,IsAdmin
				,Email
				,ul.ActivityDateTime
				,ROW_NUMBER() OVER (
					PARTITION BY u.userid ORDER BY ul.ActivityDateTime DESC
					) AS RowNo
				
			FROM AxisBank_Users U
			LEFT JOIN AxisBank_UserActivitiesLog UL ON U.UserId = UL.UserId
				AND UL.ActivityTypeId = 2
			WHERE FirstName LIKE @SearchUser
				OR LastName LIKE @SearchUser
				OR LoginId LIKE @SearchUser
				OR FirstName + ' ' + LastName LIKE @SearchUser
				) as tab
				where rowno=1
			)
		SELECT *
		FROM CTE
		WHERE RowIndex >= @StartIndex
			AND RowIndex <= @EndIndex
		ORDER BY USERID DESC

		SELECT Count(*)
		FROM AxisBank_Users
		WHERE FirstName LIKE @SearchUser
			OR LastName LIKE @SearchUser
			OR LoginId LIKE @SearchUser
			OR FirstName + ' ' + LastName LIKE @SearchUser
	END
END


