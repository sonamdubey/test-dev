IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_UpdateSubDisposition]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_UpdateSubDisposition]
GO

	-- ==================================================================
-- Author: Ruchira Patil
-- Create date: 31 oct 2013
-- Description:	to update the carbased and lead based subdispositions 
-- ==================================================================
CREATE PROCEDURE [dbo].[CRM_UpdateSubDisposition]
	@IsLeadType BIT,
	@LeadId BIGINT = NULL,
	@CBDId VARCHAR(MAX),
	@SubDisposition VARCHAR(MAX),
	@UpdatedBy INT = -1
AS
BEGIN
	
	IF @SubDisposition <> '' AND @SubDisposition <> '-1'
		BEGIN
	
			IF @IsLeadType= 0
					--UPDATE CDA
					--	SET CDA.LatestStatus = TBL2.ListMember, CDA.LastConnectedStatus = TBL2.ListMember,
					--		CDA.LatestStatusDate = GETDATE(), LastConnectedStatusDate = GETDATE()
					--FROM CRM_CarDealerAssignment CDA
					--JOIN  (SELECT Id, ListMember FROM fnSplitCSV_WIthId(@CBDId)) AS TBL1 ON TBL1.ListMember = CDA.CBDId
					--JOIN  (SELECT Id, ListMember FROM fnSplitCSV_WIthId(@SubDisposition)) AS TBL2 ON TBL2.Id = TBL1.Id	
					
				BEGIN
					DECLARE @custidIndx SMALLINT,@strId VARCHAR(20),@nameIndx SMALLINT,@strName VARCHAR(200)
					WHILE @CBDId <> ''                 
						BEGIN                
							SET @custidIndx = CHARINDEX(',', @CBDId) 
							SET @nameIndx = CHARINDEX(',', @SubDisposition)
							
							IF @custidindx > 0                
								BEGIN	
									SET @strId = LEFT(@CBDId, @custidIndx-1)  
									SET @CBDId = RIGHT(@CBDId, LEN(@CBDId)-@custidIndx) 
		            
									SET @strName = LEFT(@SubDisposition, @nameIndx-1)   
									SET @SubDisposition = RIGHT(@SubDisposition, LEN(@SubDisposition)-@nameIndx) 
									
									IF @strName <> -1 AND @strName <> ''
										BEGIN
											SELECT Id FROM CRM_SubDisposition WHERE Id = @strName AND IsStatusReceived = 1

											IF @@ROWCOUNT = 1
												BEGIN
													UPDATE CRM_CarDealerAssignment 
													SET LatestStatus = @strName , LatestStatusDate = GETDATE(),
														LastConnectedStatus = @strName, LastConnectedStatusDate = GETDATE()
													WHERE CBDId = @strId

													INSERT INTO CRM_CarStatusUpdateLog(CBDId,SubDispositionId,UpdatedBy)
													VALUES (@strId,@strName,@UpdatedBy)	
												END
											ELSE
												BEGIN
													UPDATE CRM_CarDealerAssignment 
													SET LatestStatus = @strName , LatestStatusDate = GETDATE()
													WHERE CBDId = @strId

													INSERT INTO CRM_CarStatusUpdateLog(CBDId,SubDispositionId,UpdatedBy)
													VALUES (@strId,@strName,@UpdatedBy)		
												END
										END
									
								END
							ELSE                
								BEGIN                
									SET @strId = @CBDId  
									SET @CBDId = ''
									SET @strName = @SubDisposition
									
									IF @strName <> -1 AND @strName <> ''
										BEGIN
											SELECT Id FROM CRM_SubDisposition WHERE Id = @strName AND IsStatusReceived = 1
											IF @@ROWCOUNT = 1
												BEGIN
													UPDATE CRM_CarDealerAssignment 
													SET LatestStatus = @strName , LatestStatusDate = GETDATE(),
														LastConnectedStatus = @strName, LastConnectedStatusDate = GETDATE()
													WHERE CBDId = @strId

													INSERT INTO CRM_CarStatusUpdateLog(CBDId,SubDispositionId,UpdatedBy)
													VALUES (@strId,@strName,@UpdatedBy)		
												END
											ELSE
												BEGIN
													UPDATE CRM_CarDealerAssignment 
													SET LatestStatus = @strName , LatestStatusDate = GETDATE()
													WHERE CBDId = @strId

													INSERT INTO CRM_CarStatusUpdateLog(CBDId,SubDispositionId,UpdatedBy)
													VALUES (@strId,@strName,@UpdatedBy)		
												END
										END
								END
						END		
					
				END
			ELSE  --Lead Based
				BEGIN
					SET @CBDId = ''
					SELECT Id FROM CRM_SubDisposition WHERE Id = @SubDisposition AND IsStatusReceived = 1
					
					IF @@ROWCOUNT = 1
						BEGIN
							UPDATE CRM_CarDealerAssignment 
							SET LatestStatus = @SubDisposition , LatestStatusDate = GETDATE(),LastConnectedStatus = @SubDisposition, LastConnectedStatusDate = GETDATE()
							WHERE CBDId IN (SELECT ID FROM CRM_CarBasicData WHERE LeadId = @LeadId)
							AND ISNULL(LatestStatus,0) <> 29	--not update in case of NCD/Hello 5000 leads

							--Log the data					
							INSERT INTO CRM_CarStatusUpdateLog(CBDId,SubDispositionId,UpdatedBy)
							SELECT CBD.ID, @SubDisposition, @UpdatedBy
							FROM CRM_CarBasicData CBD INNER JOIN CRM_CarDealerAssignment CDA ON CDA.CBDId=CBD.ID
							WHERE LeadId = @LeadId AND ISNULL(LatestStatus,0) <> 29
					
						END
					ELSE
						BEGIN
							UPDATE CRM_CarDealerAssignment 
							SET LatestStatus = @SubDisposition , LatestStatusDate = GETDATE()
							WHERE CBDId IN (SELECT ID FROM CRM_CarBasicData WHERE LeadId = @LeadId)
							AND ISNULL(LatestStatus,0) <> 29	--not update in case of NCD/Hello 5000 leads

							INSERT INTO CRM_CarStatusUpdateLog(CBDId,SubDispositionId,UpdatedBy)
							SELECT CBD.ID, @SubDisposition, @UpdatedBy
							FROM CRM_CarBasicData CBD INNER JOIN CRM_CarDealerAssignment CDA ON CDA.CBDId=CBD.ID
							WHERE LeadId = @LeadId AND ISNULL(LatestStatus,0) <> 29	
						END
					
					--SELECT Id FROM CRM_SubDisposition WHERE Id = @SubDisposition AND IsFinalStatus = 1
				END
		END
END

