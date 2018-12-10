IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ESM_DeleteProposedProduct]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ESM_DeleteProposedProduct]
GO

	
--Author : Vinay Kumar Prajapati  12 Desc 2014

--Purpose : To save log information when delete any product and delete proposal when  no any product exist in specific proposal 

--Modified By : Vinay Kumar Prajapati To add CampaignType 

CREATE Procedure [dbo].[ESM_DeleteProposedProduct]

(   @ESM_ProposedProductId  Int,

	@DeletedReason Varchar(500),

	@DeletedBy Int ,

	@RetValue AS NUMERIC(18,0) OUT  

)  

AS  

DECLARE @ProposedId Int,  ---This is ProposalId

        @ProductTypeId Int,

		@ProductId Int,

		@Quantity Int,

		@MRPAmount NUMERIC(18,0),

		@ProposedAmount Int,

		@ProductProbability Int,

		@ProductLastProbabilityUpdated Int,

		@Title  Varchar(50),

		@ClientId Int,

		@BrandId Int,

		@AgencyId  Int,

		@CampaignType Int



BEGIN  



      SELECT  @ProposedId= EPP.ProposedId,@ProductTypeId= EPP.ProductTypeId,@ProductId=EPP.ProductId ,@Quantity=EPP.Quantity,@MRPAmount=EPP.MRPAmount,@ProposedAmount=EPP.ProposedAmount,@ProductProbability= EPP.ProductProbability,@ProductLastProbabilityUpdated=EPP.ProductLastProbabilityUpdated

	  FROM ESM_ProposedProduct AS EPP WITH(NOLOCK) WHERE  EPP.Id=@ESM_ProposedProductId 

  	

	   DELETE ESM_ProposedProduct WHERE id = @ESM_ProposedProductId 

	  --Save deleted  Data 

	   INSERT INTO ESM_DeleteProposedProducts(ProposedId,ProductTypeId,ProductId,Quantity,MRPAmount,ProposedAmount,ProductProbability,ProductLastProbabilityUpdated,DeletedReason,DeletedOn,DeletedBy)

	   VALUES(@ProposedId, @ProductTypeId,@ProductId,@Quantity,@MRPAmount,@ProposedAmount,@ProductProbability,@ProductLastProbabilityUpdated,@DeletedReason,getdate(),@DeletedBy)

    

	 ---------------------------------------------------------- Deletion Of Of Proposal-------------------------------------------------------

	   -- If there is No  Any product exist   In proposal then Proposal will also be deleted 

	   SELECT TOP 1 EPP.id  FROM ESM_ProposedProduct AS EPP WITH(NOLOCK) WHERE EPP.ProposedId=@ProposedId

	   IF @@ROWCOUNT=0

	   BEGIN

			--Save The proposal before Deleting this 

			SELECT @Title= ESP.Title,@ClientId= ESP.ClientId,@AgencyId = ESP.OrgId,@BrandId=ESP.BrandId,@CampaignType=ESP.CampaignType FROM ESM_Proposal AS ESP WITH(NOLOCK)  WHERE ESP.Id=@ProposedId



			INSERT  INTO ESM_ProposalLogs(ESM_ProposalId,Title,ClientId,BrandId,AgencyId,CampaignTypeId, DeletedBy,DeletedOn) VALUES(@ProposedId,@Title,@ClientId,@BrandId,@AgencyId,@CampaignType,@DeletedBy,Getdate())

			-- Delete This proposal from ESM_Proposal



			DELETE FROM  ESM_Proposal WHERE id=@ProposedId

 	   END

	   -------------------------------------------------------------------********-------------------------------------------------------------------



	  SET  @RetValue = 1 					    	   

END