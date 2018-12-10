IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveDealerFollowUp]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveDealerFollowUp]
GO

	

--Summary							: Save Dealer and RM Follow up
--Author							: Dilip V. 
--Modification history				: 1.Dilip V. 25-May-2012 (Made follow for RM panel differentiating using column PanelType)

CREATE PROCEDURE [dbo].[CRM_SaveDealerFollowUp]

@Id NUMERIC,
@CustId NUMERIC,
@LeadId NUMERIC,
@ProductStatus NUMERIC,
@Eagerness NUMERIC,
@NextCallDate DATETIME = NULL,
@LastComment VARCHAR(1250),
@Comment VARCHAR(8000),
@UpdatedBy INT,
@DealerId NUMERIC,
@PanelType BIT,
@subDispositionId INT,
@Status NUMERIC OUTPUT


AS
	
BEGIN
	IF @Id = -1
		BEGIN
			IF EXISTS (SELECT Id FROM CRM_DealerFollowUp WHERE CustId = @CustId AND LeadId = @LeadId AND DealerId = @DealerId AND PanelType = @PanelType AND SubDispositionId=@subDispositionId)
				BEGIN
					SET @Status = 0
				END
			ELSE
				BEGIN
					INSERT INTO CRM_DealerFollowUp (CustId,LeadId,ProductStatus,Eagerness,CreatedOn,NextCallDate,LastCallDate,LastComment,Comment,UpdatedBy,DealerId,PanelType,SubDispositionId)
					VALUES (@CustId, @LeadId,@ProductStatus,@Eagerness,GETDATE(),@NextCallDate,GETDATE(),@LastComment,@Comment,@UpdatedBy,@DealerId,@PanelType,@subDispositionId)
					SET @Status = SCOPE_IDENTITY()
				END
				
		END
	ELSE
		BEGIN
		
			IF EXISTS (SELECT Id FROM CRM_DealerFollowUp WHERE ID <> @Id AND CustId = @CustId AND LeadId = @LeadId AND DealerId = @DealerId AND PanelType = @PanelType AND SubDispositionId=@subDispositionId)
				BEGIN
					SET @Status = 0
				END
			ELSE	
				BEGIN
					UPDATE CRM_DealerFollowUp 
					SET ProductStatus=@ProductStatus,Eagerness=@Eagerness, NextCallDate = @NextCallDate , LastCallDate = GETDATE(), LastComment = @LastComment,Comment = @Comment,					
					UpdatedBy = @UpdatedBy, DealerId = @DealerId, PanelType = @PanelType,SubDispositionId=@subDispositionId
					WHERE Id = @Id
					SET @Status = 1
				END
		END
END



