IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveCarDealerAssignmentData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveCarDealerAssignmentData]
GO

	
CREATE PROCEDURE [dbo].[CRM_SaveCarDealerAssignmentData]
	
	@CDAId				    Numeric,
	@CarBasicDataId			Numeric,
	@DealerId				Numeric,
	@StatusId				SmallInt,
	@CreatedById			Numeric,
	@UpdatedById			Numeric,
	@Comments				VarChar(5000),

	@CreatedOn				DateTime,
	@UpdatedOn				DateTime,
	@LostDate				DateTime,
	@NoResponseDate			DateTime,
	@ContactPerson			VarChar(50),
	@Contact				VarChar(50),
	@LostName				VarChar(100),
	@ReasonLost				VarChar(100),
	@currentId				Numeric OutPut,
	@CampaignId             BIGINT = NULL,
	@IsConCall              BIT = NULL,
	@DealershipStatus		BIT = NULL,
	@IsFollowDone			BIT = NULL,
	@DealershipTagDate		DATETIME = NULL
				
 AS
BEGIN
DECLARE @DailyDel INT ,@DelLeads INT ,@DailyCount INT ,@TargetLeads INT
	SET @currentId = -1
	IF @CDAId = -1
		BEGIN
			
			SELECT @CDAId = ID FROM CRM_CarDealerAssignment WHERE CBDId = @CarBasicDataId
			
			IF @@ROWCOUNT = 0
				BEGIN
					INSERT INTO CRM_CarDealerAssignment
					(
						CBDId, DealerId, Status, CreatedOn, 
						CreatedBy, UpdatedOn, UpdatedBy, Comments, LostDate,
						ContactPerson, Contact, LostName, ReasonLost ,IsConCall,CampaignId,
						LastConnectedStatus, LatestStatus, LastConnectedStatusDate, LatestStatusDate, DealershipStatus, IsFollowDone, DealershipTagDate
					)
					VALUES
					(
						@CarBasicDataId, @DealerId, @StatusId, @CreatedOn, 
						@CreatedById, @UpdatedOn, @UpdatedById, @Comments, @LostDate,
						@ContactPerson, @Contact, @LostName, @ReasonLost,@IsConCall,@CampaignId,
						31, 31, GETDATE(), GETDATE(),@DealershipStatus,@IsFollowDone,@DealershipTagDate
					)
					
					SET @currentId = Scope_Identity()
					
					UPDATE CRM_CarBasicData  SET IsDealerAssigned = 1 WHERE ID = @CarBasicDataId
					
					IF @CampaignId > 0
						BEGIN
							--UPDATE NCS_Dealers  SET DelLeads = ISNULL(DelLeads,0) + 1 WHERE  ID = @DealerId
							UPDATE NCS_Dealers  SET DailyDel = ISNULL(DailyDel,0) + 1 , DelLeads = ISNULL(DelLeads,0) + 1 WHERE  ID = @DealerId
						END
					
					SELECT @DailyDel = DailyDel,@DelLeads = DelLeads,@DailyCount = DailyCount,@TargetLeads = TargetLeads 
					FROM NCS_Dealers
					WHERE ID = @DealerId

					IF @DailyDel = @DailyCount
						BEGIN
							UPDATE CRM_ADM_QueueRuleParams SET IsActive = 0 WHERE DealerId = @DealerId
						END
						
					IF @TargetLeads = @DelLeads
						BEGIN
							INSERT INTO CRM_ADM_QueueRuleParamsLog (ID,QueueId,MakeId,ModelId,CityId,CreatedOn,SourceId,IsResearch,DeletedBy,DeletedOn) 
							SELECT ID,QueueId,MakeId,ModelId,CityId,CreatedOn,SourceId,IsResearch,@UpdatedById,GETDATE() FROM CRM_ADM_QueueRuleParams WHERE 
							DealerId = @DealerId

							DELETE FROM CRM_ADM_QueueRuleParams WHERE DealerId = @DealerId
						END

				END
			ELSE
				BEGIN
					UPDATE CRM_CarDealerAssignment
					SET DealerId = @DealerId, Status = @StatusId, Comments = @Comments,
						UpdatedBy = @UpdatedById, UpdatedOn = @UpdatedOn, LostDate = @LostDate,
						ContactPerson = @ContactPerson, Contact = @Contact, LostName = @LostName,	
						ReasonLost = @ReasonLost, NoResponseDate = @NoResponseDate,IsConCall = @IsConCall,CampaignId = @CampaignId
					WHERE Id = @CDAId
				END
		END

	ELSE

		BEGIN
			UPDATE CRM_CarDealerAssignment
			SET DealerId = @DealerId, Status = @StatusId, Comments = @Comments,
				UpdatedBy = @UpdatedById, UpdatedOn = @UpdatedOn, LostDate = @LostDate,
				ContactPerson = @ContactPerson, Contact = @Contact, LostName = @LostName,	
				ReasonLost = @ReasonLost, NoResponseDate = @NoResponseDate ,IsConCall = @IsConCall,CampaignId = @CampaignId,
				DealershipStatus=@DealershipStatus, IsFollowDone=@IsFollowDone, DealershipTagDate=@DealershipTagDate
			WHERE Id = @CDAId

			SET @currentId = @CDAId
		END
END



