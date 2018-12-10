IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_ADM_AddFLCMsgNEW]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_ADM_AddFLCMsgNEW]
GO

	--THIS PROCEDURE INSERTS AND UPDATES FLC Message

CREATE PROCEDURE [dbo].[CRM_ADM_AddFLCMsgNEW]
	@ID				NUMERIC,
	@MakeId		    NUMERIC = NULL,
	@ModelId		VARCHAR(MAX),
	@CityId         VARCHAR(MAX),
	@Message		VARCHAR(1500),
	@AddedBy        NUMERIC,
	@UpdatedBy		NUMERIC = NULL,
	@UpdatedOn		DATETIME ,
	@StateId		NUMERIC = NULL,
	@Status			BIT OUTPUT
 AS
	
BEGIN
	--Declaration Section
    --Temp table to get cities
	DECLARE @TempTblCity TABLE
	(
	 ID INT IDENTITY(1,1),
	 City INT
	)

	DECLARE @TempTblModel TABLE
	(
	 ID INT IDENTITY(1,1),
	 Model INT
	)
	DECLARE @TempCity		INT = -1,
			@TempModel		INT = -1,
			@CityCounter	INT,
			@ModelCounter	INT,
			@TempMsg		VARCHAR(1500),
			@TempId         INT,
			@i				TINYINT = 1,
			@j				TINYINT = 1

	--for all cities and models
	IF @CityId = '0' OR @CityId = '-1'
		SET @CityId = NULL
						
	IF @ModelId = '0' OR @ModelId = '-1'
		SET @ModelId = NULL
					
	IF(@CityId IS NULL)
		BEGIN
			--Fills temp tables with all the cities
			INSERT INTO @TempTblCity(City) SELECT Id FROM Cities WITH (NOLOCK) WHERE StateId = @StateId AND IsDeleted = 0 
			--SELECT * FROM @TempTblCity
		END  
	ELSE
		BEGIN
		--Fills temp tables with cities
			INSERT INTO @TempTblCity(City) SELECT ListMember FROM fnSplitCSV(@CityId)
			--SELECT * FROM @TempTblCity
		END

	IF(@ModelId IS NULL)
		BEGIN
			--Fills temp tables with all the models
			INSERT INTO @TempTblModel(Model) SELECT CMO.ID FROM CarModels CMO WITH (NOLOCK) WHERE CMO.CarMakeId = @MakeId AND CMO.IsDeleted = 0 AND CMO.New = 1 
			--SELECT * FROM @TempTblModel
		END  
	ELSE
		BEGIN
		--Fills temp tables with models
			INSERT INTO @TempTblModel(Model) SELECT ListMember FROM fnSplitCSV(@ModelId)
			--SELECT * FROM @TempTblModel
		END

	
    --Insert into Table    
	IF @ID = -1
		BEGIN
		--Sets counter for looping through @TempTblCity table
			
			SELECT @CityCounter = COUNT(ID) FROM @TempTblCity 	
							
			WHILE(@CityCounter > 0)
				BEGIN
									
					--PRINT  @i
					SELECT @TempCity = City FROM @TempTblCity WHERE ID = @i
							
					--Decreament city counter and increamet identity column of @TempTblCity table
					SET @i = @i + 1
					SET @CityCounter = @CityCounter -1
					--Set counter for looping through @TempTblModel
					SELECT @ModelCounter = COUNT(ID) FROM @TempTblModel
									
					SET @j = 1
															
					WHILE (@ModelCounter > 0)
						BEGIN
							SELECT @TempModel = Model FROM @TempTblModel WHERE ID = @j
							
							SET @j = @j + 1
							SET @ModelCounter = @ModelCounter -1
							SELECT ID FROM CRM_FLCMessage WHERE CityId = @TempCity AND ModelId = @TempModel
							
							IF @@ROWCOUNT = 0
								BEGIN 
									PRINT 'In Insert-'
									--Inserts FLC Message data in to CRM_FLCMessage.			
									INSERT INTO CRM_FLCMessage(CityId, ModelId, Message, AddedBy, AddedOn,StateId,LatestUpdateOn,LatestUpdateBy)			
									VALUES(@TempCity, @TempModel, @Message, @AddedBy, GETDATE(),@StateId,GETDATE(),@AddedBy)
											        
									SET @ID = SCOPE_IDENTITY()
									--Inserts FLC Message data in to CRM_FLCMessageLog.	
									INSERT INTO CRM_FLCMessageLog (FLCMessageId,MakeId,ModelId,StateId,CityId,UpdatedBy,UpdatedOn, Message) 
									VALUES (@ID,@MakeId,@TempModel,@StateId,@TempCity,@UpdatedBy,@UpdatedOn, @Message)	
								
									SET @Status = 1
								END	
							ELSE
								BEGIN
									SELECT @TempMsg = CFM.Message,@TempId = CFM.ID FROM CRM_FLCMessage CFM WITH (NOLOCK) 
									WHERE CFM.CityId = @TempCity AND CFM.ModelId = @TempModel
									print @TempMsg
									print @Message
									IF @@ROWCOUNT > 0
									BEGIN
										IF @TempMsg = @Message
											SET @Status = 0
										ELSE
										BEGIN
											UPDATE CRM_FLCMessage SET Message = @TempMsg + '<hr>'+ @Message,LatestUpdateOn= @UpdatedOn,LatestUpdateBy= @UpdatedBy  
											WHERE Id = @TempId	
										
											INSERT INTO CRM_FLCMessageLog (FLCMessageId,MakeId,ModelId,StateId,CityId,UpdatedBy,UpdatedOn, Message) 
											VALUES (@TempId,@MakeId,@TempModel,@StateId,@TempCity,@UpdatedBy,@UpdatedOn, @Message)	
											
											SET @Status = 1
										END
									END
								END
						END														
				END
		END
	--Update table for specific city and models.	
	ELSE
		BEGIN
			DECLARE @IsUpdated BIT = 0

			SELECT @ModelCounter = COUNT(ID) FROM @TempTblModel
	
			WHILE (@ModelCounter > 0)
			BEGIN
				SELECT @TempModel = Model FROM @TempTblModel WHERE ID = @j
		
				SELECT Id FROM CRM_FLCMessage WITH (NOLOCK) WHERE CityId IN (SELECT ListMember FROM fnSplitCSV(@CityId))  AND ModelId=@TempModel AND ID <> @Id
				IF @@ROWCOUNT = 0
				BEGIN
					IF @IsUpdated = 0
					BEGIN
						--print 'in update'
						UPDATE CRM_FLCMessage SET Message= @Message, ModelId=@TempModel,LatestUpdateOn= @UpdatedOn,LatestUpdateBy= @UpdatedBy  WHERE Id = @Id	
						-- This part is used to save log of message when the message is updated.		
						INSERT INTO CRM_FLCMessageLog (FLCMessageId,MakeId,ModelId,UpdatedBy,UpdatedOn, Message) 
						VALUES (@Id,@MakeId,@TempModel,@UpdatedBy,@UpdatedOn, @Message)	

						SET @IsUpdated = 1
					END
					ELSE
					BEGIN
						--Inserts FLC Message data in to CRM_FLCMessage.			
						INSERT INTO CRM_FLCMessage(CityId, ModelId, Message, AddedBy, AddedOn,StateId,LatestUpdateOn,LatestUpdateBy)			
						SELECT ListMember,@TempModel, @Message, @AddedBy, GETDATE(),@StateId,GETDATE(),@AddedBy FROM fnSplitCSV(@CityId)
											        
						SET @ID = SCOPE_IDENTITY()
						--Inserts FLC Message data in to CRM_FLCMessageLog.	
						INSERT INTO CRM_FLCMessageLog (FLCMessageId,MakeId,ModelId,StateId,CityId,UpdatedBy,UpdatedOn, Message) 
						SELECT @ID,@MakeId,@TempModel,@StateId,ListMember,@UpdatedBy,@UpdatedOn, @Message FROM fnSplitCSV(@CityId)
					END
					SET @Status = 1
				END
				ELSE
					SET @Status = 0
				SET @j = @j + 1
				SET @ModelCounter = @ModelCounter -1
			END
		END	
END

