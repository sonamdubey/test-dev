IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ESM_SaveProposal]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ESM_SaveProposal]
GO

	/*  
 This procedure created on 18 Jan 2010 by Sentil  
 for update and save for Proposal
 Modified By: Vinay kumar prajapti 3 desc 2014
 Purpose : To save Property, Platform, Campaign and keep log when update . 
 Modified By : Vinay Kumar Prajapati 6th Jan 2015   to keep probability log information for proposal 
 Modified : Amit Yadav(5th Oct 2015)
 Purpose : Added parameter @ProposedAmount,@ProductTypeId,@Property,@IsApp,@IsDesktop,@IsMobile  
*/  
CREATE Procedure [dbo].[ESM_SaveProposal]  
(  
	@ID AS NUMERIC(18,0),  
	@Title AS VARCHAR(50),
	@ClientId AS  NUMERIC(18,0),  
	@BrandId AS NUMERIC(18,0),  
	@OrgId AS NUMERIC(18,0),    
	@Remark AS VARCHAR(50),  --this is Next step
	@ESMUser AS NUMERIC(18,0),  
	@CreatedOn AS DATETIME,  
	@LastUpdatedOn AS DATETIME,  
	@UpdatedBy AS NUMERIC(18,0),
	@CampaignTypeId AS Int,
	@Probability AS Int,
	@RetValue AS NUMERIC(18,0) OUT,  
	@ProposedAmount AS NUMERIC(18,0),
	@ProductTypeId AS NUMERIC(18,0),
	@Property   AS Int,
	@IsApp  AS BIT,
	@IsDesktop  AS BIT,
	@IsMobile  AS BIT
)
AS  

DECLARE @LastProbability AS INT 

BEGIN  
  
DECLARE @ProposedId AS BIGINT

SET @ProposedId = 0  
  
 IF(@ID = -1)  
	  BEGIN  
	       --Avoid Duplicate entry
		   SELECT EPP.id FROM ESM_proposal AS EPP WITH(NOLOCK) WHERE EPP.ClientId=@ClientId AND EPP.BrandId=@BrandId AND EPP.OrgId=@OrgId AND EPP.CampaignType=@CampaignTypeId AND  EPP.Title=@Title AND ISNULL(IsDeleted,0) <> 1
		   IF @@ROWCOUNT = 0
		   BEGIN
			   INSERT INTO ESM_proposal (Title,ClientId, BrandId, OrgId, ESMUserId,Probability,Remark, CreatedOn, UpdatedBy,CampaignType,ProposedAmount,ProductTypeId,Property,IsApp,IsDesktop,IsMobile)
								  VALUES( @Title,@ClientId, @BrandId, @OrgId, @ESMUser,@Probability, @Remark, @CreatedOn, @UpdatedBy,@CampaignTypeId,@ProposedAmount,@ProductTypeId,@Property,@IsApp,@IsDesktop,@IsMobile)
		  
			   SET @ProposedId = SCOPE_IDENTITY()  
		     
			   --Log the Data 
			   INSERT INTO ESM_ProposalLogs(ESM_ProposalId,ClientId,BrandId, AgencyId ,CampaignTypeId,UpdatedBy,UpdatedOn,Probability) 
			   VALUES(@ProposedId,@ClientId, @BrandId, @OrgId,@CampaignTypeId,@UpdatedBy,Getdate(),@Probability)
		  
			   SET @RetValue = @ProposedId
			   SET @ID =  @ProposedId
		   END

	  END  
 ELSE  
	  BEGIN  
	       --Avoid Duplicate entry
		   SELECT EPP.id FROM ESM_proposal AS EPP WITH(NOLOCK) WHERE EPP.ClientId=@ClientId AND EPP.BrandId=@BrandId AND EPP.OrgId=@OrgId AND EPP.CampaignType=@CampaignTypeId AND  EPP.Title=@Title AND ISNULL(IsDeleted,0) <> 1
		   AND EPP.id <> @ID
		   IF @@ROWCOUNT = 0
				BEGIN

				    ---Update Last Probability 
					IF NOT EXISTS(SELECT  id  FROM ESM_Proposal WITH(NOLOCK) WHERE Id = @ID AND Probability = @Probability) 
						BEGIN
							SELECT  @LastProbability = Probability  FROM ESM_Proposal WITH(NOLOCK) WHERE Id = @ID
				
							UPDATE ESM_Proposal SET LastProbabilityUpdated = @LastProbability WHERE Id = @ID 
							SET @RetValue=@ID
						END


					  UPDATE ESM_proposal SET   
							Title=@Title, ClientId = @ClientId, BrandId = @BrandId, OrgId = @OrgId, ESMUserId = @ESMUser,Probability=@Probability, ProposedAmount=@ProposedAmount,  
							Remark = @Remark, LastUpdatedOn = @LastUpdatedOn, UpdatedBy = @UpdatedBy,CampaignType=@CampaignTypeId,ProductTypeId=@ProductTypeId,Property=@Property,IsApp=@IsApp,IsDesktop=@IsDesktop,IsMobile=@IsMobile  
					  WHERE id = @ID
					  SET @RetValue=@ID

					  ---Log the  data 
					   INSERT INTO ESM_ProposalLogs(ESM_ProposalId,ClientId,BrandId,AgencyId,CampaignTypeId,UpdatedBy,UpdatedOn,Probability) 
					   VALUES(@ID,@ClientId, @BrandId, @OrgId,@CampaignTypeId,@UpdatedBy,Getdate(),@Probability)
		  	   
					   SET @RetValue = -1 	
					   SET @ProposedId = @ID     	     
	              END
		END		
		
			-- log the Data only when probability is change by previous update.
	  SELECT Top 1 @LastProbability  = ISNULL(UpdatedProbability,0)  FROM ESM_ProposalProbabilityLogs AS EPL WITH(NOLOCK)
	  WHERE EPL.ESM_ProposalId = @ProposedId   ORDER BY UpdatedOn DESC	 
  
  	  IF (@LastProbability <> @Probability)
		BEGIN
		   INSERT INTO ESM_ProposalProbabilityLogs (ESM_ProposalId, UpdatedProbability, UpdatedBy, UpdatedOn)
		                            VALUES(@ProposedId, @Probability, @UpdatedBy, GETDATE())
		END					    		  						    	   
END 
