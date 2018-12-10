IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UsersDelete]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UsersDelete]
GO

	
-- =============================================
-- Author:		Binumon George
-- Create date: 07-03-2012
-- Description:	User Deleting on bsis of user Id
-- [dbo].[TC_UsersDelete] 1,5,@Status
-- Modified by: Vivek Gupta on 11th july,2013, Declared  parameters @HasActiveCalls AND @HasChild to promt deletion of the user
-- Modified by Nilesh on 17-09-2013 for maintaining logs of Dealers masters for Mobile APP
-- Modified By : Suresh Prajapati on 29th June, 2015
-- Description : To update deleted record in TC_UserAreaMapping table
-- =============================================
CREATE PROCEDURE [dbo].[TC_UsersDelete]
	-- Add the parameters for the stored procedure here
	@UserId INT
	,@BranchId INT
	,@Status INT OUTPUT
AS
BEGIN
	DECLARE @IsSuperAdmin BIT
	DECLARE @IsSurveyor BIT
	DECLARE @ParentId INT

	EXEC TC_IsSuperAdmin @UserId
		,@BranchId
		,@IsSuperAdmin OUTPUT

	DECLARE @HasActiveCalls BIT

	SELECT @HasActiveCalls = COUNT(TC_CallsId)
	FROM TC_ActiveCalls
	WHERE TC_UsersId = @UserId

	DECLARE @HasChild BIT
	DECLARE @TblAllChild TABLE (Id INT)

	INSERT INTO @TblAllChild
	EXEC TC_GetImmediateChild @UserId

	SELECT @HasChild = COUNT(*)
	FROM @TblAllChild

	IF (
			SELECT TC_DealerTypeId
			FROM Dealers
			WHERE ID = @BranchId
			) = 4
	BEGIN
		IF (
				SELECT RoleId
				FROM TC_UsersRole
				WHERE UserId = @UserId
				) = 13
			SELECT @HasActiveCalls = COUNT(AbSure_CarDetailsId)
			FROM AbSure_CarSurveyorMapping
			WHERE TC_UserId = @UserId

		IF (
				SELECT RoleId
				FROM TC_UsersRole
				WHERE UserId = @UserId
				) = 15
			SELECT @HasActiveCalls = COUNT(AbSure_CarDetailsId)
			FROM AbSure_CarSurveyorMapping
			WHERE TC_UserId = @UserId
	END

	IF (
			@IsSuperAdmin != 1
			AND @HasActiveCalls != 1
			AND @HasChild != 1
			) --checking here basic super adminor not
	BEGIN
		UPDATE TC_Users
		SET IsActive = 0
		WHERE Id = @UserId
			AND BranchId = @BranchId

		--Aadded By : Suresh Prajapati on 29th, 2015
		UPDATE TC_UserAreaMapping
		SET IsActive = 0
		WHERE TC_UserId = @UserId

		SET @IsSurveyor = CASE (
					SELECT IsAgency
					FROM TC_Users WITH (NOLOCK)
					WHERE Id = @UserId
					)
				WHEN 1
					THEN 0
				ELSE 1
				END

		IF (ISNULL(@IsSurveyor, 0) <> 0)
		BEGIN
			-- i.e. Surveyor
			EXEC TC_GetImmediateParent @UserId
				,@ParentId OUTPUT

			UPDATE TC_UserAreaMapping
			SET IsAssigned = 0
				,UpdatedBy = @UserId
				,UpdatedOn = GETDATE()
			WHERE AreaId IN (
					SELECT AreaId
					FROM TC_UserAreaMapping WITH (NOLOCK)
					WHERE TC_UserId = @UserId
						--AND IsActive = 1
						AND IsAssigned = 1
					)
				AND TC_UserId = @ParentId
		END

		SET @Status = 1

		------------------------below code is added by Nilesh on 17-09-2013 for maintaining logs of Dealers masters for Mobile APP
		IF EXISTS (
				SELECT TC_UsersRoleId
				FROM TC_UsersRole
				WHERE UserId = @UserId
					AND RoleId = 4
				)
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
	END

	IF (
			@HasActiveCalls = 1
			AND @HasChild = 0
			)
	BEGIN
		SET @Status = 0 --@Status=0 means the user has active calls so he can't be deleted 
	END
	ELSE
		IF (
				@HasActiveCalls = 0
				AND @HasChild = 1
				)
		BEGIN
			SET @Status = 2 --@Status=2 means the user has childs reporting to him so he can't be deleted
		END
		ELSE
			IF (
					@HasActiveCalls = 1
					AND @HasChild = 1
					)
			BEGIN
				SET @Status = 3 --@Status=3 means the user has childs reporting to him and also the user has active calls
			END -- so he can't be deleted
END

