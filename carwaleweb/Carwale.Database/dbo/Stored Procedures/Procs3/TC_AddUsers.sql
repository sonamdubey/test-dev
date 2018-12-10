IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_AddUsers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_AddUsers]
GO

	
-- =============================================================================================================================================
-- Author:		<Author,Nilesh Utture>
-- Create date: <Create Date,15th March, 2013>
-- Modified By :  Modified by Manish Chourasiya(AE1665) on 18-03-2013 for updation of hierarchy column in TC_Users table.
-- Description:	<Description, Add/Edit Users at dealership>
-- Modified By: Nilesh Utture on 03/04/2013 Added check while updating users
-- Modified By: Manish on 20-06-2013 for not update hierarchy if reporting to is same
-- Modified By Vivek on 27thAug,2013..Added IsActive=1 for reactivating the user.
-- Modified by Nilesh on 17-09-2013 for maintaining logs of Dealers masters for Mobile APP
-- Modified by Vishal on 02-04-2014 adding VTO Number for VW users
-- Modified By Vivek Gupta on 15-10-2014, added @BranchLocationId(AreaId) to get branch location of stock for multiple branch dealers
-- Modified By: Ashwini Dhamankar on 17-12-2014, Added CityId and AreaId
-- Modified By : Suresh Prajapati on 25th June, 2015
-- Description : To save/update user serving area(s)
-- Modified By : Suresh Prajapati on 24th Feb, 2016
-- Description : 1. To save/update user's HashSalt and PasswordHash
--			     2. Removed Operation on Password Column 
-- =============================================================================================================================================
CREATE PROCEDURE [dbo].[TC_AddUsers]
	-- Add the parameters for the stored procedure here
	@UserId INT
	,@UserName VARCHAR(50)
	,@Email VARCHAR(100)
	,@Mobile VARCHAR(10)
	--,@Password VARCHAR(20)
	,@Sex VARCHAR(6)
	,@Address VARCHAR(200)
	,@ModifiedBy INT
	,@DOJ DATETIME
	,@DOB DATETIME
	,@RoleList VARCHAR(50)
	,@DivisionList VARCHAR(50)
	,@ReportingTo INT = NULL
	,@BranchId BIGINT
	,@Status TINYINT OUTPUT
	,@VTO VARCHAR(50)
	,@AreaIdTable [dbo].[TC_UserAreaMap] READONLY --Added By : Suresh Prajapati on 25th June, 2015
	-- Modified by Vishal on 02-04-2014 adding VTO Number for VW users
	,@BranchLocationId INT = NULL
	,@CityId INT
	,@AreaId INT = NULL
	,@IsAgency BIT = NULL
	,@HashSalt VARCHAR(10) = NULL -- Added By Suresh Prajapati on 24th Feb, 2016
	,@PasswordHash VARCHAR(100) = NULL -- Added By Suresh Prajapati on 24th Feb, 2016
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @TC_UserId INT -- to store User Generated on Insert operation
	DECLARE @Separator_position INT -- This is used to locate each separator character  
	DECLARE @array_value VARCHAR(1000) -- this holds each array value as it is returned  
	DECLARE @Separator VARCHAR(1) = ','

	SET @RoleList = @RoleList + @Separator

	IF (@DivisionList IS NOT NULL)
	BEGIN
		SET @DivisionList = @DivisionList + @Separator
	END

	PRINT 0

	-- Insert statements for procedure here
	IF (@UserId IS NULL) --IF id parameter is null, we inserting new user to TC_Users table
	BEGIN
		IF EXISTS (
				SELECT Id
				FROM TC_Users WITH(NOLOCK)
				WHERE Email = @Email
				)
		BEGIN
			SET @Status = 0
		END
		ELSE
		BEGIN
			-- Modified by Vishal on 02-04-2014 adding VTO Number for VW users
			INSERT INTO TC_Users (
				BranchId
				,UserName
				,Email
				--,Password
				,Mobile
				,EntryDate
				,DOB
				,DOJ
				,Sex
				,Address
				,ModifiedBy
				,VTONumbers
				,BranchLocationId
				,CityId
				,AreaId
				,IsAgency
				,IsActive
				,ReportingTo
				,HashSalt
				,PasswordHash
				)
			VALUES (
				@BranchId
				,@UserName
				,@Email
				--,@Password
				,@Mobile
				,GETDATE()
				,@DOB
				,@DOJ
				,@Sex
				,@Address
				,@ModifiedBy
				,@VTO
				,@BranchLocationId
				,@CityId
				,@AreaId
				,@IsAgency
				,1
				,@ReportingTo
				,@HashSalt
				,@PasswordHash
				)

			SET @TC_UserId = SCOPE_IDENTITY()

			PRINT @TC_UserId

			IF EXISTS (
					SELECT U.Id
					FROM TC_Users U WITH (NOLOCK)
					JOIN TC_UsersRole R WITH (NOLOCK) ON R.UserId = U.Id
						AND U.BranchId = @BranchId
						AND R.RoleId = 10
					)
			BEGIN
				PRINT 1

				IF (
						@ReportingTo IS NOT NULL
						AND @TC_UserId <> @ReportingTo
						)
				BEGIN
					PRINT 2

					EXEC TC_UsersUpdateHierarchy @UpdateId = @TC_UserId
						,@ParentID = @ReportingTo
				END
				ELSE
					IF (@ReportingTo IS NULL)
					BEGIN
						PRINT 3

						SELECT @ReportingTo = U.Id
						FROM TC_Users U WITH (NOLOCK)
						INNER JOIN TC_UsersRole R WITH (NOLOCK) ON U.Id = R.UserId
							AND U.BranchId = @BranchId
							AND R.RoleId = 10

						EXEC TC_UsersUpdateHierarchy @UpdateId = @TC_UserId
							,@ParentID = @ReportingTo

						PRINT 4
					END
			END

			---------------------------------------------------------------------------------------------*/                                                                                 
			WHILE PATINDEX('%' + @Separator + '%', @RoleList) <> 0
			BEGIN
				-- patindex matches the a pattern against a string  
				SELECT @Separator_position = PATINDEX('%' + @Separator + '%', @RoleList)

				SELECT @array_value = LEFT(@RoleList, @Separator_position - 1)

				-- This is where you process the values passed.
				INSERT INTO TC_UsersRole (
					UserId
					,RoleId
					)
				VALUES (
					@TC_UserId
					,@array_value
					)

				PRINT 6

				------------------------below code is added by Nilesh on 17-09-2013 for maintaining logs of Dealers masters for Mobile APP
				IF (@array_value = 4)
				BEGIN
					INSERT INTO TC_DealerMastersLog (
						DealerId
						,TableName
						,CreatedOn
						)
					VALUES (
						@BranchId
						,'TC_Users'
						,GETDATE()
						)

					PRINT 7
				END

				----------------------------------------------------------------------------------------------------------------------
				SELECT @RoleList = STUFF(@RoleList, 1, @Separator_position, '')

				PRINT @roleList
			END

			IF (@DivisionList IS NOT NULL)
			BEGIN
				WHILE PATINDEX('%' + @Separator + '%', @DivisionList) <> 0
				BEGIN
					PRINT 8

					-- patindex matches the a pattern against a string  
					SELECT @Separator_position = PATINDEX('%' + @Separator + '%', @DivisionList)

					SELECT @array_value = LEFT(@DivisionList, @Separator_position - 1)

					-- This is where you process the values passed.
					INSERT INTO TC_UserModelsPermission (
						TC_UsersId
						,ModelId
						)
					VALUES (
						@TC_UserId
						,@array_value
						)

					PRINT 9

					SELECT @DivisionList = STUFF(@DivisionList, 1, @Separator_position, '')

					PRINT @divisionList
				END
			END

			SET @Status = 1
		END
	END
	ELSE
	BEGIN
		PRINT 10

		IF EXISTS (
				SELECT Id
				FROM TC_Users WITH (NOLOCK)
				WHERE Email = @Email
					AND Id <> @UserId
				) -- Modified By: Nilesh Utture on 03/04/2013
		BEGIN
			SET @Status = 0
		END
		ELSE
		BEGIN
			UPDATE TC_Users
			SET UserName = @UserName
				,Email = @Email
				--,Password = @Password
				,Mobile = @Mobile
				,DOB = @DOB
				,DOJ = @DOJ
				,Sex = @Sex
				,Address = @Address
				,VTONumbers = @VTO
				,-- Modified by Vishal on 02-04-2014 adding VTO Number for VW users
				ModifiedBy = @ModifiedBy
				,ModifiedDate = GETDATE()
				,IsActive = 1
				,-- Modified By Vivek on 27thAug,2013..Added IsActive=1 for reactivating the user.
				BranchLocationId = @BranchLocationId
				,CityId = @CityId
				,AreaId = @AreaId
				,IsAgency = @IsAgency
				,HashSalt=@HashSalt
				,PasswordHash=@PasswordHash
			WHERE Id = @UserId
				AND BranchId = @BranchId

			SET @TC_UserId = @UserId

			DECLARE @OldParentId INT

			EXEC TC_GetImmediateParent @UserId = @UserId
				,@TC_ReportingTo = @OldParentId OUTPUT

			IF EXISTS (
					SELECT U.Id
					FROM TC_Users U WITH (NOLOCK)
					JOIN TC_UsersRole R WITH (NOLOCK) ON R.UserId = U.Id
						AND U.BranchId = @BranchId
						AND R.RoleId = 10
					)
			BEGIN
				IF (
						@ReportingTo IS NOT NULL
						AND @UserId <> @ReportingTo
						)
				BEGIN
					IF (
							@OldParentId <> @ReportingTo
							OR @OldParentId IS NULL
							) ----modified by manish on 20-06-2013
					BEGIN
						/*EXEC TC_UsersUpdateHierarchy  @UpdateId=@UserId,@ParentID=@ReportingTo*/
						EXEC TC_MaintainUserHierarchy @UpdateId = @UserId
							,@NewParentId = @ReportingTo
					END
				END
				ELSE
					IF (@ReportingTo IS NULL) ----modified by manish on 20-06-2013
					BEGIN
						SELECT @ReportingTo = U.Id
						FROM TC_Users U WITH (NOLOCK)
						INNER JOIN TC_UsersRole R WITH (NOLOCK) ON U.Id = R.UserId
							AND U.BranchId = @BranchId
							AND R.RoleId = 10

						IF (@OldParentId IS NOT NULL)
						BEGIN
							/*	EXEC TC_UsersUpdateHierarchy  @UpdateId=@UserId,0@ParentID=@ReportingTo*/
							EXEC TC_MaintainUserHierarchy @UpdateId = @UserId
								,@NewParentId = @ReportingTo
						END
					END
			END

			---------------------------------------------------------------------------------------------*/
			DELETE
			FROM TC_UsersRole
			WHERE UserId = @UserId
				AND RoleId <> 9

			DELETE
			FROM TC_UserModelsPermission
			WHERE TC_UsersId = @UserId

			WHILE PATINDEX('%' + @Separator + '%', @RoleList) <> 0
			BEGIN
				-- patindex matches the a pattern against a string  
				SELECT @Separator_position = PATINDEX('%' + @Separator + '%', @RoleList)

				SELECT @array_value = LEFT(@RoleList, @Separator_position - 1)

				-- This is where you process the values passed.
				INSERT INTO TC_UsersRole (
					UserId
					,RoleId
					)
				VALUES (
					@UserId
					,@array_value
					)

				------------------------below code is added by Nilesh on 17-09-2013 for maintaining logs of Dealers masters for Mobile APP
				IF (@array_value = 4)
				BEGIN
					INSERT INTO TC_DealerMastersLog (
						DealerId
						,TableName
						,CreatedOn
						)
					VALUES (
						@BranchId
						,'TC_Users'
						,GETDATE()
						)
				END

				----------------------------------------------------------------------------------------------------------------------
				SELECT @RoleList = STUFF(@RoleList, 1, @Separator_position, '')
			END

			IF (@DivisionList IS NOT NULL)
			BEGIN
				WHILE PATINDEX('%' + @Separator + '%', @DivisionList) <> 0
				BEGIN
					-- patindex matches the a pattern against a string  
					SELECT @Separator_position = PATINDEX('%' + @Separator + '%', @DivisionList)

					SELECT @array_value = LEFT(@DivisionList, @Separator_position - 1)

					-- This is where you process the values passed.
					INSERT INTO TC_UserModelsPermission (
						TC_UsersId
						,ModelId
						)
					VALUES (
						@UserId
						,@array_value
						)

					SELECT @DivisionList = STUFF(@DivisionList, 1, @Separator_position, '')
				END
			END

			SET @Status = 2
		END
	END

	--==========================================================
	--		Added By : Suresh Prajapati on 25th June, 2015
	--		To Save/Update user area(s) mapping 
	--==========================================================
	IF (ISNULL(@TC_UserId, 0) <> 0)
	BEGIN
		IF EXISTS (
				SELECT 1
				FROM TC_UserAreaMapping WITH (NOLOCK)
				WHERE TC_UserId = @UserId
				)
		BEGIN
			-- IF USER ALREADY EXISTS IN TC_UserAreaMapping TABLE
			IF EXISTS (
					SELECT 1
					FROM TC_Users WITH (NOLOCK)
					WHERE Id = @TC_UserId
						AND IsAgency = 1
					)
			BEGIN
				-- AND User IS AN AGENCY
				CREATE TABLE #NewAreaTable (AreaId INT)

				--SELECT ONLY NEW CITIES FROM @AreaIdTable AND INSERT THAT CITY(IES) INTO TC_UserAreaMapping AGAINST THAT AGENCY
				INSERT INTO #NewAreaTable
				SELECT AT.AreaId
				FROM TC_UserAreaMapping AS AM WITH (NOLOCK)
				RIGHT JOIN @AreaIdTable AS AT ON AT.AreaId = AM.AreaId
					AND TC_UserId = @TC_UserId
				WHERE AM.AreaId IS NULL

				IF EXISTS (
						SELECT 1
						FROM #NewAreaTable WITH (NOLOCK)
						)
				BEGIN
					INSERT INTO TC_UserAreaMapping (
						TC_UserId
						,AreaId
						,IsActive
						,UpdatedBy
						,EntryDate
						,IsAssigned
						)
					SELECT @TC_UserId
						,AreaId
						,1
						,@ModifiedBy
						,GETDATE()
						,0
					FROM #NewAreaTable WITH (NOLOCK)
				END

				-- AT THE END DELETE TEMPORARY TABLE 
				DROP TABLE #NewAreaTable
			END
			ELSE
			BEGIN
				-- AND User IS A SURVEYOR
				CREATE TABLE #NewSurveyorAreaTable (AreaId INT)

				INSERT INTO #NewSurveyorAreaTable
				SELECT AT.AreaId
				FROM TC_UserAreaMapping AS AM WITH (NOLOCK)
				RIGHT JOIN @AreaIdTable AS AT ON AT.AreaId = AM.AreaId
					AND AM.IsActive = 1
				WHERE AM.AreaId IS NULL
					AND TC_UserId = @TC_UserId

				--AND AM.IsActive = 1
				IF EXISTS (
						SELECT 1
						FROM #NewSurveyorAreaTable WITH (NOLOCK)
						)
				BEGIN
					INSERT INTO TC_UserAreaMapping (
						TC_UserId
						,AreaId
						,IsActive
						,UpdatedBy
						,EntryDate
						,IsAssigned
						)
					SELECT @TC_UserId
						,AreaId
						,1
						,@ModifiedBy
						,GETDATE()
						,1
					FROM #NewSurveyorAreaTable WITH (NOLOCK)

					UPDATE TC_UserAreaMapping
					SET IsAssigned = 1
						,UpdatedBy = @UserId
						,UpdatedOn = GETDATE()
					WHERE TC_UserId = @ReportingTo
						AND AreaId IN (
							SELECT AreaId
							FROM #NewSurveyorAreaTable WITH (NOLOCK)
							)
						AND IsActive = 1
				END
				ELSE
				BEGIN
					IF EXISTS (
							SELECT AT.AreaId
							FROM @AreaIdTable AS AT
							WHERE AT.AreaId NOT IN (
									SELECT AreaId
									FROM TC_UserAreaMapping WITH (NOLOCK)
									WHERE TC_UserId = @TC_UserId
										AND IsActive = 1
									)
							)
					BEGIN
						CREATE TABLE #FilterNewArea (AreaId INT)

						INSERT INTO #FilterNewArea
						SELECT AT.AreaId
						FROM @AreaIdTable AS AT
						WHERE AT.AreaId NOT IN (
								SELECT AreaId
								FROM TC_UserAreaMapping WITH (NOLOCK)
								WHERE TC_UserId = @TC_UserId
									AND IsActive = 1
								)

						INSERT INTO TC_UserAreaMapping (
							TC_UserId
							,AreaId
							,IsActive
							,UpdatedBy
							,EntryDate
							,IsAssigned
							)
						SELECT @TC_UserId
							,AreaId
							,1
							,@ModifiedBy
							,GETDATE()
							,1
						FROM #FilterNewArea WITH (NOLOCK)

						UPDATE TC_UserAreaMapping
						SET IsAssigned = 1
							,UpdatedBy = @UserId
							,UpdatedOn = GETDATE()
						WHERE AreaId IN (
								SELECT AreaId
								FROM #FilterNewArea WITH (NOLOCK)
								)
							AND TC_UserId = @ReportingTo
							AND IsActive = 1
					END
				END

				CREATE TABLE #TempNewAreaTable (AreaId INT)

				CREATE TABLE #TempOldAreaTable (AreaId INT)

				INSERT INTO #TempOldAreaTable
				SELECT AM.AreaId
				FROM TC_UserAreaMapping AS AM WITH (NOLOCK)
				LEFT JOIN @AreaIdTable AS AT ON AT.AreaId = AM.AreaId
				WHERE AT.AreaId IS NULL
					AND AM.TC_UserId = @TC_UserId
					AND AM.IsActive = 1 -- @UserId

				INSERT INTO #TempNewAreaTable
				SELECT AM.AreaId
				FROM TC_UserAreaMapping AS AM WITH (NOLOCK)
				RIGHT JOIN @AreaIdTable AS AT ON AT.AreaId = AM.AreaId
				WHERE AM.AreaId IS NULL
					AND AM.TC_UserId = @TC_UserId
					AND AM.IsActive = 1 -- @UserId

				IF EXISTS (
						SELECT 1
						FROM #TempNewAreaTable WITH (NOLOCK)
						)
				BEGIN
					UPDATE TC_UserAreaMapping
					SET IsAssigned = 1
						,UpdatedBy = @UserId
						,UpdatedOn = GETDATE()
					WHERE TC_UserId = @ReportingTo
						AND AreaId IN (
							SELECT AreaId
							FROM #TempNewAreaTable WITH (NOLOCK)
							)
						AND IsActive = 1
				END

				IF EXISTS (
						SELECT 1
						FROM #TempOldAreaTable WITH (NOLOCK)
						)
				BEGIN
					UPDATE TC_UserAreaMapping
					SET IsAssigned = 0
						,UpdatedBy = @UserId
						,UpdatedOn = GETDATE()
					WHERE TC_UserId = @ReportingTo
						AND AreaId IN (
							SELECT AreaId
							FROM #TempOldAreaTable WITH (NOLOCK)
							)
						AND IsActive = 1

					UPDATE TC_UserAreaMapping
					SET IsActive = 0
						,UpdatedBy = @UserId
						,UpdatedOn = GETDATE()
					WHERE TC_UserId = @UserId
						AND AreaId IN (
							SELECT AreaId
							FROM #TempOldAreaTable WITH (NOLOCK)
							)
						AND IsActive = 1
				END

				DROP TABLE #TempNewAreaTable

				DROP TABLE #TempOldAreaTable

				DROP TABLE #NewSurveyorAreaTable
			END
		END
		ELSE
		BEGIN
			--IT'S A NEW RECORD
			IF EXISTS (
					SELECT 1
					FROM TC_Users WITH (NOLOCK)
					WHERE Id = @TC_UserId
						AND IsAgency = 1
					)
			BEGIN
				-- AND USER IS A NEW AGENCY
				INSERT INTO TC_UserAreaMapping (
					TC_UserId
					,AreaId
					,IsActive
					,UpdatedBy
					,EntryDate
					,IsAssigned
					)
				SELECT @TC_UserId
					,AreaId
					,1
					,@ModifiedBy
					,GETDATE()
					,0
				FROM @AreaIdTable
			END
			ELSE
			BEGIN
				-- OR USER IS A NEW SURVEYOR
				-- SET IsAssigned 1 FOR AREA UNDER REPORTING AGENCY
				UPDATE TC_UserAreaMapping
				SET IsAssigned = 1
					,UpdatedBy = @UserId
					,UpdatedOn = GETDATE()
				WHERE AreaId IN (
						SELECT AreaId
						FROM @AreaIdTable
						)
					AND TC_UserId = @ReportingTo
					AND IsActive = 1

				-- AND ENTER A NEW SURVEYOR
				INSERT INTO TC_UserAreaMapping (
					TC_UserId
					,AreaId
					,IsActive
					,UpdatedBy
					,EntryDate
					,IsAssigned
					)
				SELECT @TC_UserId
					,AreaId
					,1
					,@ModifiedBy
					,GETDATE()
					,1
				FROM @AreaIdTable
			END
		END
	END
END

