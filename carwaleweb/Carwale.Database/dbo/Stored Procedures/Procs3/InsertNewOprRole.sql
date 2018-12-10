IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertNewOprRole]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertNewOprRole]
GO

	
--THIS PROCEDURE INSERTS THE THE new role

CREATE PROCEDURE [dbo].[InsertNewOprRole]
	@ID				NUMERIC,
	@RoleName			VARCHAR(50),
	@Description			VARCHAR(100),
	@TaskList			VARCHAR(5000)	--THIS LIST CAONTAINS THE VEHICLE IDS SEPARATED BY COMMA
 
AS
	DECLARE
		@STARTINDEX	AS INT,		--HOLDS STARTINDEX 
		@ENDINDEX		AS INT,		--HOLDS ENDINDEX
		@TEMPID		AS NUMERIC,
		@RoleId		AS NUMERIC
  	
BEGIN
	IF @ID = -1 
	BEGIN

		--insert the role
		INSERT INTO OPRROLES	(Name, Description, IsActive)
			VALUES	(@RoleName, @Description, 1)

		SET @RoleId = SCOPE_IDENTITY()

		--set @ENDINDEX to 0
		SET @ENDINDEX = 0
		SET @STARTINDEX = 0

		WHILE @ENDINDEX < LEN(@TaskList)		
		BEGIN
			
			SET @ENDINDEX = CHARINDEX(',',@TaskList,@ENDINDEX+1)
	
			SET @TEMPID = SUBSTRING(@TaskList, @STARTINDEX, @ENDINDEX - @STARTINDEX)
			
			print  @STARTINDEX
			print  @ENDINDEX

			--NOW INSERT THE RoleId And the task idinto roletask table
					
			INSERT INTO OPRRoleTasks ( RoleID, TasksID) VALUES( @RoleId, @TEMPID)
			
			SET @STARTINDEX = @ENDINDEX + 1

			--check whether @ENDINDEX < LEN(@TaskList)	
			IF @ENDINDEX >= LEN(@TaskList)
				BREAK;
			
				
		
		END
	END
	ELSE
	BEGIN 
		UPDATE  OPRROLES	SET
		Name  =  @RoleName ,
		 Description  =  @Description
	
		WHERE ID = @ID

		DELETE FROM OPRROLETASKS WHERE ROLEID = @ID
			
		SET @ENDINDEX = 0
		SET @STARTINDEX = 0

		WHILE @ENDINDEX < LEN(@TaskList)		
		BEGIN
			
			SET @ENDINDEX = CHARINDEX(',',@TaskList,@ENDINDEX+1)
	
			SET @TEMPID = SUBSTRING(@TaskList, @STARTINDEX, @ENDINDEX - @STARTINDEX)
			
			print  @STARTINDEX
			print  @ENDINDEX

			--NOW INSERT THE RoleId And the task idinto roletask table
					
			INSERT INTO OPRRoleTasks ( RoleID, TasksID) VALUES( @ID, @TEMPID)
			
			SET @STARTINDEX = @ENDINDEX + 1

			--check whether @ENDINDEX < LEN(@TaskList)	
			IF @ENDINDEX >= LEN(@TaskList)
				BREAK;
			
				
		
		END



	END
	
END
