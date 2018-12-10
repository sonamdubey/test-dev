IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[HR_InsertJobs]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[HR_InsertJobs]
GO

	-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <26/3/2014>
-- Description:	<Add New Jobs>
-- =============================================
CREATE PROCEDURE [dbo].[HR_InsertJobs]
	@FunctionId			INT,
	@RoleDescription	VARCHAR(MAX),
	@RoleSummary		VARCHAR(100),
	@JobTitle			VARCHAR(100),
	@StateId			INT,
	@CityId				VARCHAR(MAX),
	@HR_JobId			INT,
	@NewId				INT,
	@User				INT,
	@ID					INT		OUTPUT
AS
BEGIN
	SET @ID = -1
	DECLARE @LEN INT
	--FIND NO OF CITYID SELECTED 
	SELECT @LEN=(LEN(@CityId) - LEN(REPLACE(@CityId, ',', '')))

	--CHECK IS JOB ALREADY EXISTS AT ALL SELECTED CITIES
	SELECT HR_JobId FROM HR_Jobs HJ
	INNER JOIN HR_JobLocations HJL ON HJL.HR_JobId=HJ.HR_JobsId 
	WHERE HJ.FunctionId=@FunctionId AND HJ.IsActive=1 
	AND (HJL.StateId=@StateId OR HJL.StateId=-1) AND (HJL.CityId IN (SELECT ListMember FROM fnSplitCSV(@CityId)) OR HJL.CityId = -1 )
	AND HJL.IsActive=1

	IF @@ROWCOUNT < @LEN+1 OR @NewId = 1
	BEGIN
		print @NewId
		--CHECK JOB EXISTS OR NOT 
		SELECT @ID=HR_JobsId FROM HR_Jobs HJ
		INNER JOIN HR_JobLocations HJL ON HJL.HR_JobId=HJ.HR_JobsId 
		WHERE FunctionId = @FunctionId 
		AND RoleDescription = @RoleDescription AND RoleSummary = @RoleSummary AND JobTitle = @JobTitle AND HJ.IsActive = 1 AND HJL.StateId = @StateId

		IF @@ROWCOUNT < 1 AND @NewId <> 1
		BEGIN
			--INSERT JOB 
			INSERT INTO HR_Jobs(FunctionId,RoleDescription,RoleSummary,PositionOpenDate,JobTitle,IsActive)
			VALUES(@FunctionId,@RoleDescription,@RoleSummary,GETDATE(),@JobTitle,1)

			SET @ID=SCOPE_IDENTITY()
		END
		IF @NewId = 1
		BEGIN
			UPDATE HR_Jobs SET RoleSummary = @RoleSummary , RoleDescription = @RoleDescription , JobTitle = @JobTitle,UpdatedOn=GETDATE(),UpdatedBy=@User WHERE HR_JobsId = @HR_JobId
			--DELETE FROM HR_JobLocations WHERE HR_JobId = @HR_JobId 
			DELETE JL
			FROM HR_JobLocations AS JL
			INNER JOIN HR_Jobs HJ ON HJ.HR_JobsId = JL.HR_JobId 
			WHERE HJ.HR_JobsId = @HR_JobId  AND HJ.FunctionId = @FunctionId AND JL.StateId =@StateId 
			SET @ID = @HR_JobId
		END
					
		DECLARE @DELIMITER NCHAR(1)   --DELIMITER USED TO SEPARATE THE CITYID
		DECLARE @tmpCityId NVARCHAR(MAX)
		SET @tmpCityId =@CityId
		SET @DELIMITER = ','       --DELIMITER IS A COMMA
		DECLARE @commaIndex INT    
		DECLARE @commaSepCityId NVARCHAR(MAX)
		--@commaSepCityId IS THE VARIABLE WHICH HOLDS EACH ITEM IN THE COMMA-SEPARATED STRING

		SELECT @commaIndex = 1   
		PRINT @tmpCityId 
		IF LEN(@tmpCityId)<1 OR @tmpCityId IS NULL  RETURN   
			
		WHILE @commaIndex <> 0    
		BEGIN    
				
			SET @commaIndex= CHARINDEX(@DELIMITER,@tmpCityId)   
			PRINT @commaIndex
			IF @commaIndex <> 0    
				SET @commaSepCityId= LEFT(@tmpCityId,@commaIndex- 1)    
			ELSE    
				SET @commaSepCityId = @tmpCityId 
			PRINT '@commaSepCityId: '+@commaSepCityId   
			IF(LEN(@commaSepCityId)>0)
			BEGIN    
				
				--CHECK IS SAME JOB AVAILABLE AT THAT LOCATION
				SELECT HR_JobId FROM HR_Jobs HJ
				INNER JOIN HR_JobLocations HJL ON HJL.HR_JobId=HJ.HR_JobsId 
				WHERE HJ.FunctionId=@FunctionId AND HJ.IsActive=1 
				AND (HJL.StateId=@StateId OR HJL.StateId=-1) AND (HJL.CityId IN (@commaSepCityId) OR HJL.CityId = -1 ) 
				AND HJL.IsActive=1

				IF @@ROWCOUNT < 1
				BEGIN
					INSERT INTO HR_JobLocations(HR_JobId,StateId,CityId,IsActive)VALUES(@ID,@StateId,@commaSepCityId,1)				
				END
			END
			SET @tmpCityId = RIGHT(@tmpCityId,LEN(@tmpCityId) - @commaIndex)   
			PRINT  '@tmpCityId: '+@tmpCityId
			PRINT LEN(@tmpCityId)
			IF LEN(@tmpCityId) = 0 BREAK    
		END
	END	
	ELSE
		SET @ID = 0
END
