IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_ADM_AddFLCMsg]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_ADM_AddFLCMsg]
GO

	
--THIS PROCEDURE INSERTS AND UPDATES FLC Message
--Modified by : Amit Kumar on 23rd jan 2013 (inserted value in table CRM_FLCMessageLog )
--			  : Amit Kumar on 31st jan 2013(added field StateId)
--            : Vinay Kumar on 21st may 2013(added field MakeId,ModelId,StateId,CityId In Table CRM_FLCMessageLog
--                and updated and inserted information Stored in CRM_FLCMessageLog Table)
--			  : Chetan Navin - 27 June 2013 ( Insertion of FLC Message data for selected state and brand ,with city-model having one to many relation.)
--Module      : NCS CRM
--Exec CRM_ADM_AddFLCMsg -1,7,0,0,'Chetan Testing',3,3,'2013-06-27 19:08:43.393',8,0

CREATE PROCEDURE [dbo].[CRM_ADM_AddFLCMsg]
	@ID				NUMERIC,
	@MakeId		    NUMERIC = NULL,
	@ModelId		NUMERIC = NULL,
	@CityId         NUMERIC = NULL,
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
	--Temp table to get models
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
			@TempId         INT
			
    --Insert into Table    
	IF @ID = -1
		BEGIN
			SELECT Id FROM CRM_FLCMessage WITH (NOLOCK) WHERE CityId = @CityId AND ModelId=@ModelId
			IF @@ROWCOUNT = 0
				BEGIN
					
					--for all cities and models
					IF @CityId = 0 OR @CityId = -1
						SET @CityId = NULL
						
					IF @ModelId = 0 OR @ModelId = -1
						SET @ModelId = NULL
					
					IF(@CityId IS NULL OR @ModelId IS NULL)
						BEGIN
							
							--Fills temp tables with cities and models
							INSERT INTO @TempTblCity(City) SELECT Id FROM Cities WITH (NOLOCK) WHERE StateId = @StateId AND IsDeleted = 0 AND (ID = @CityId OR @CityId IS NULL)
							INSERT INTO @TempTblModel(Model) SELECT CMO.ID FROM CarModels CMO WITH (NOLOCK) WHERE CMO.CarMakeId = @MakeId AND CMO.IsDeleted = 0 AND CMO.New = 1 AND (CMO.ID = @ModelId OR @ModelId IS NULL)
				            
							--Sets counter for looping through @TempTblCity table
							DECLARE @i TINYINT = 1,@j TINYINT = 1
							SELECT @CityCounter = COUNT(ID) FROM @TempTblCity 	
							
							WHILE(@CityCounter > 0)
								BEGIN
									
									--PRINT  @i
									SELECT @TempCity = City FROM @TempTblCity WHERE ID = @i
									
									--Decreament model counter and increamet identity column of @TempTblCity table
									SET @i = @i + 1
									SET @CityCounter = @CityCounter -1
									--Set counter for looping through @TempTblModel
									SELECT @ModelCounter = COUNT(ID) FROM @TempTblModel
									
									SET @j = 1
															
									WHILE (@ModelCounter > 0)
										
										BEGIN
											SELECT @TempModel = Model FROM @TempTblModel WHERE ID = @j
											
											SELECT @TempMsg = CFM.Message,@TempId = CFM.ID FROM CRM_FLCMessage CFM WITH (NOLOCK) 
											WHERE CFM.CityId = @TempCity AND CFM.ModelId = @TempModel
								
											--Checks if message is present for city and model.
											IF @@ROWCOUNT = 0
												BEGIN  
													--PRINT 'In Insert-'
													--Inserts FLC Message data in to CRM_FLCMessage.								
													INSERT INTO CRM_FLCMessage(CityId, ModelId, Message, AddedBy, AddedOn,StateId,LatestUpdateOn,LatestUpdateBy)			
													VALUES(@TempCity, @TempModel, @Message, @AddedBy, GETDATE(),@StateId,GETDATE(),@AddedBy)
											        
													SET @ID = SCOPE_IDENTITY()
													--Inserts FLC Message data in to CRM_FLCMessageLog.	
													INSERT INTO CRM_FLCMessageLog (FLCMessageId,MakeId,ModelId,StateId,CityId,UpdatedBy,UpdatedOn, Message) 
													VALUES (@ID,@MakeId,@TempModel,@StateId,@TempCity,@UpdatedBy,@UpdatedOn, @Message)	
								
												END	
												--Update for multiple citites and models with city-model having one to many relation.		
											ELSE 
												BEGIN
													--PRINT 'In Update-'
													UPDATE CRM_FLCMessage SET Message = @TempMsg + '<hr>'+ @Message,LatestUpdateOn= @UpdatedOn,LatestUpdateBy= @UpdatedBy  
													WHERE Id = @TempId	
										
													INSERT INTO CRM_FLCMessageLog (FLCMessageId,MakeId,ModelId,StateId,CityId,UpdatedBy,UpdatedOn, Message) 
													VALUES (@TempId,@MakeId,@TempModel,@StateId,@TempCity,@UpdatedBy,@UpdatedOn, @Message)	
												END
												--Decreament model counter and increamet identity of @TempModel table.	
											SET @j = @j + 1
											SET @ModelCounter = @ModelCounter -1	
										END
																				
								END					
							SET @Status = 1
						END
						
					ELSE
					--for specific city and models.
						BEGIN
							INSERT INTO CRM_FLCMessage(CityId, ModelId, Message, AddedBy, AddedOn,StateId,LatestUpdateOn,LatestUpdateBy)			
							VALUES(@CityId, @ModelId, @Message, @AddedBy, GETDATE(),@StateId,GETDATE(),@AddedBy)	
					  		
			  				SET @ID = SCOPE_IDENTITY() 
		                    
							INSERT INTO CRM_FLCMessageLog (FLCMessageId,MakeId,ModelId,StateId,CityId,UpdatedBy,UpdatedOn, Message) 
							VALUES (@ID,@MakeId,@ModelId,@StateId,@CityId,@UpdatedBy,@UpdatedOn, @Message)	
							
							SET @Status = 1
						END
				END
			ELSE
				BEGIN
					SET @Status = 0	
				END
		END
	--Update table for specific city and models.	
	ELSE
		BEGIN
			SELECT Id FROM CRM_FLCMessage WITH (NOLOCK) WHERE CityId = @CityId AND ModelId=@ModelId AND ID <> @Id
			IF @@ROWCOUNT = 0
				BEGIN
					UPDATE CRM_FLCMessage SET Message= @Message, ModelId=@ModelId,LatestUpdateOn= @UpdatedOn,LatestUpdateBy= @UpdatedBy  WHERE Id = @Id	
					-- This part is used to save log of message when the message is updated.		
					INSERT INTO CRM_FLCMessageLog (FLCMessageId,MakeId,ModelId,UpdatedBy,UpdatedOn, Message) VALUES (@Id,@MakeId,@ModelId,@UpdatedBy,@UpdatedOn, @Message)	

					SET @Status = 1
				END
			ELSE
				SET @Status = 0	
		END
END
