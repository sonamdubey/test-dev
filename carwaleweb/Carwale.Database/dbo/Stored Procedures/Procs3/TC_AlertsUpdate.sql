IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_AlertsUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_AlertsUpdate]
GO

	-- ====================================================================================================
-- Author:		Surendra Chouksey
-- Create date: 5th Dec,2011
-- Description:	This Procedure will set permission to access Trading car from DDCRM
-- Modified By: Surendar on 13 march for changing roles table
-- Modified By: Nilesh Utture on 21st May, 2013 Added roles to carWale User upon creation and updation
-- Modified By : Tejashree Patil on 10 Jan 2016,Given deals dealer role for dealettype 2 and 3.
-- Modified By : Suresh Prajapati on 10th Mar, 2016
-- description : 1. Removed Password Column check for TC_User
--				 2. Added HashSalt and PasswordHash Check for TC_User
--				 3. Inserted HashSalt and PasswordHash for new TC_User
-- Modified By: Tejashree Patil on 4 May 2016, Checked role existance before insert into TC_UsersRole
-- ====================================================================================================
CREATE PROCEDURE [dbo].[TC_AlertsUpdate] (
	@BranchId NUMERIC
	,@Status BIT
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	--SET NOCOUNT ON;
	DECLARE @UserId VARCHAR(50)
		--,@Password VARCHAR(50)
		,@RoleList VARCHAR(100)
		,@DealerTypeId TINYINT
		,@TC_UserId INT
		,@IsDealsDealer BIT
		,@HashSalt VARCHAR(10) = '8g2GlY'
		,@PasswordHash VARCHAR(100) = '3517e98e1007e43e47ac78e3c2b0bdfbe347d15ec0e77b078f6dfb4087c5e484'
		,@DealsRole VARCHAR(5) = '18,'

	SET @UserId = CONVERT(VARCHAR, @BranchId) + 'CW@CarWale.com'

	--SET @Password = 'Default'
	DECLARE @Separator_position INT -- This is used to locate each separator character  
	DECLARE @array_value VARCHAR(1000) -- this holds each array value as it is returned  
	DECLARE @Separator VARCHAR(1) = ','

	-- Modified By: Nilesh Utture on 21st May, 2013
	SELECT @DealerTypeId = TC_DealerTypeId
		,@IsDealsDealer = ISNULL(DD.IsDealerDealActive, 0)
	FROM Dealers D WITH (NOLOCK)
	LEFT JOIN TC_Deals_Dealers DD WITH (NOLOCK) ON DD.DealerId = D.ID
	WHERE ID = @BranchId

	IF (@DealerTypeId = 1)
	BEGIN
		SET @RoleList = '1,2,3,5,6,'
	END

	IF (@DealerTypeId = 3)
	BEGIN
		SET @RoleList = '1,2,3,4,5,6,'

		IF (@IsDealsDealer = 1)
			SET @RoleList = @RoleList + @DealsRole
	END

	IF (@DealerTypeId = 2)
	BEGIN
		SET @RoleList = '1,4,'

		IF (@IsDealsDealer = 1)
			SET @RoleList = @RoleList + @DealsRole
	END

	-- Modified By: Nilesh Utture on 21st May, 2013
	SELECT @TC_UserId = Id
	FROM TC_Users WITH (NOLOCK)
	WHERE Email = @UserId
		--AND Password = @Password
		AND BranchId = @BranchId
		AND HashSalt = '8g2GlY'
		AND PasswordHash = '3517e98e1007e43e47ac78e3c2b0bdfbe347d15ec0e77b078f6dfb4087c5e484'

	UPDATE TC_Alerts
	SET STATUS = @Status
		,ResponseDatetime = GETDATE()
	WHERE BranchId = @BranchId
		AND STATUS IS NULL

	IF (@Status = 1)
	BEGIN
		IF @TC_UserId IS NOT NULL
		BEGIN
			UPDATE TC_Users
			SET IsActive = 1
			WHERE Email = @UserId
				--AND Password = @Password
				AND BranchId = @BranchId
				AND HashSalt = '8g2GlY'
				AND PasswordHash = '3517e98e1007e43e47ac78e3c2b0bdfbe347d15ec0e77b078f6dfb4087c5e484'

			-- Modified By: Nilesh Utture on 21st May, 2013
			WHILE PATINDEX('%' + @Separator + '%', @RoleList) <> 0
			BEGIN
				-- patindex matches the a pattern against a string  
				SELECT @Separator_position = PATINDEX('%' + @Separator + '%', @RoleList)

				SELECT @array_value = LEFT(@RoleList, @Separator_position - 1)

				-- This is where you process the values passed.
				
				-- Modified By: Tejashree Patil on 4 May 2016, Checked role existance before insert into TC_UsersRole
				IF NOT EXISTS ( 
									select TC_UsersRoleId from TC_UsersRole WITH (NOLOCK) 
									where UserId = @TC_UserId AND RoleId = @array_value
								)
				BEGIN
					INSERT INTO TC_UsersRole (
						UserId
						,RoleId
						)
					VALUES (
						@TC_UserId
						,@array_value
						)
				END
				SELECT @RoleList = STUFF(@RoleList, 1, @Separator_position, '')
			END
		END
		ELSE
		BEGIN
			-- Creating Role with limited task list to perform	
			/* 14-03-2013 BEGIN TRY

					BEGIN TRAN

					--commenting TaskSet.because its puting TC_RoleTasks table

						DECLARE @ROLEID BIGINT

						INSERT INTO TC_Roles(RoleName,BranchId,RoleCreationDate)

						VALUES('CWAccess',@BranchId,GETDATE())		

						--inserting combination of role and task in TC_RoleTasks table  

						DECLARE @DealerType TINYINT

						SELECT @DealerType=TC_DealerTypeId FROM Dealers WHERE ID=@BranchId						

						

						SET @ROLEID=SCOPE_IDENTITY()--seting identity to variablr.because this need to tc_roletask and tc_usersTables

						/*

						INSERT INTO TC_RoleTasks(RoleId,TaskId) SELECT @ROLEID, T.ID FROM TC_Tasks T

						WHERE (T.TC_DealerTypeId=@DealerType OR T.TC_DealerTypeId IS NULL) AND T.Id NOT IN(1,14)

						*/

						---checking dealer type here

						IF(@DealerType=0 OR @DealerType IS NULL)-- For normal ucd dealer

							BEGIN

								SET @DealerType=1

								

								--T.Id<>3 = Hide manage website tab from CarWale user. 

								--T.Id NOT IN(1,14)(Admin, SuperAdmin), Don't assign these tasks to CarWale user.

								INSERT INTO TC_RoleTasks(RoleId,TaskId) SELECT @ROLEID, T.ID FROM TC_Tasks T

								WHERE T.Id<>3 AND (T.TC_DealerTypeId=@DealerType OR T.TC_DealerTypeId IS NULL) AND T.Id NOT IN(1,14)

							END

						ELSE IF(@DealerType =3)-- for  ucd and ncd dealer

							BEGIN

								INSERT INTO TC_RoleTasks(RoleId,TaskId) SELECT @ROLEID, T.ID FROM TC_Tasks T WHERE T.Id NOT IN(1,14)

							END

						ELSE IF(@DealerType !=0)-- for  ucd or ncd dealer

							BEGIN

								INSERT INTO TC_RoleTasks(RoleId,TaskId) SELECT @ROLEID, T.ID FROM TC_Tasks T

								WHERE T.Id NOT IN(1,14) AND (T.TC_DealerTypeId=@DealerType OR T.TC_DealerTypeId IS NULL)

							END

						---checking dealer type ending here	

						INSERT INTO TC_Users(UserName,Email,Password,BranchId,RoleId,IsCarwaleUser)

						VALUES('CarWale',@UserId,@Password,@BranchId,@ROLEID,1)

					COMMIT TRAN 

				END TRY

				-- The previous GO breaks the script into two batches,

				-- generating syntax errors. The script runs if this GO

				-- is removed.

				BEGIN CATCH

					ROLLBACK TRAN

					SELECT ERROR_NUMBER() AS ErrorNumber;

				END CATCH;	  14-03-2013*/
			INSERT INTO TC_Users (
				UserName
				,Email
				--,Password
				,BranchId
				,RoleId
				,IsCarwaleUser
				,HashSalt
				,PasswordHash
				)
			VALUES (
				'CarWale'
				,@UserId
				--,@Password
				,@BranchId
				,NULL
				,1
				,@HashSalt
				,@PasswordHash
				)

			DECLARE @UsersId INT

			SET @UsersId = SCOPE_IDENTITY()

			-- Modified By: Nilesh Utture on 21st May, 2013
			WHILE PATINDEX('%' + @Separator + '%', @RoleList) <> 0
			BEGIN
				-- patindex matches the a pattern against a string  
				SELECT @Separator_position = PATINDEX('%' + @Separator + '%', @RoleList)

				SELECT @array_value = LEFT(@RoleList, @Separator_position - 1)

				-- This is where you process the values passed.
				
				-- Modified By: Tejashree Patil on 4 May 2016, Checked role existance before insert into TC_UsersRole
				IF NOT EXISTS (select TC_UsersRoleId from TC_UsersRole WITH (NOLOCK) where UserId = @UsersId AND RoleId = @array_value)
				BEGIN
					INSERT INTO TC_UsersRole (
						UserId
						,RoleId
						)
					VALUES (
						@UsersId
						,@array_value
						)
				END

				SELECT @RoleList = STUFF(@RoleList, 1, @Separator_position, '')
			END
		END
	END
	ELSE
	BEGIN
		UPDATE TC_Users
		SET IsActive = 0
		WHERE Email = @UserId
			--AND Password = @Password
			AND BranchId = @BranchId
			AND HashSalt = '8g2GlY'
			AND PasswordHash = '3517e98e1007e43e47ac78e3c2b0bdfbe347d15ec0e77b078f6dfb4087c5e484'

		DELETE
		FROM TC_UsersRole
		WHERE UserId = @TC_UserId -- Modified By: Nilesh Utture on 21st May, 2013
	END
END


