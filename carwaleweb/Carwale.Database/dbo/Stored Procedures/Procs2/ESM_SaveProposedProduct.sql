IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ESM_SaveProposedProduct]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ESM_SaveProposedProduct]
GO

	/*
	This procedure created on 22 Jan 2010 by Sentil
	for update and save for ProposedProduct	
	Updated By : vinay Kumar Praapati 4th desc 2014
	Purpose : To log the data and freeze it 
*/

CREATE PROCEDURE [dbo].[ESM_SaveProposedProduct]
(
	@Id AS NUMERIC(18,0),
	@ProposedId AS NUMERIC(18,0),
	@Property   AS Int,
	@Platform  AS Int,
	@ProductTypeId AS NUMERIC(18,0),
	@ProductId AS NUMERIC(18,0),
	@Quantity AS NUMERIC(18,0),
	@MRPAmount AS NUMERIC(18,0),
	@ProposedAmount AS NUMERIC(18,0),
	@CreatedOn AS DATETIME,
	@UpdatedOn AS DATETIME,
	@UpdatedBy	AS NUMERIC(18,0),
        @Probability AS NUMERIC(18,0),
	@FinalROValue AS NUMERIC(18,0),
	@Discount  AS NUMERIC(18,0),
	@RetValue AS NUMERIC(18,0) OUT
)
AS

DECLARE @LastProbability AS BIGINT 
DECLARE @ESM_ProposedProductId AS BIGINT 
SET @LastProbability  = 0
SET @ESM_ProposedProductId =0

BEGIN

	IF(@ID = -1)
		BEGIN
		    --Avoid dublicate Entry ....
		    SELECT PP.ID FROM ESM_ProposedProduct AS PP WITH(NOLOCK) WHERE PP.ProposedId=@ProposedId AND PP.ProductTypeId=@ProductTypeId AND PP.ProductId= @ProductId AND PP.Property=@Property AND PP.Platform=@Platform
			IF @@ROWCOUNT = 0
				BEGIN
					INSERT INTO ESM_ProposedProduct ( ProposedId, ProductTypeId, ProductId, Quantity, MRPAmount, ProposedAmount, CreatedOn,UpdatedOn,UpdatedBy,ProductProbability,Property,Platform)
											VALUES	( @ProposedId, @ProductTypeId, @ProductId, @Quantity, @MRPAmount, @ProposedAmount, 
													  @CreatedOn,@UpdatedOn,@UpdatedBy,@Probability,@Property,@Platform)
					SET @RetValue = SCOPE_IDENTITY()
					SET @ESM_ProposedProductId=@RetValue

					--Log The data 
					INSERT INTO ESM_ProposedProductLogs(ESM_ProposedProductId,ProposedId,ProductTypeId,ProductId,Property,Platform,Quantity,MRPAmount,ProposedAmount,Probability,CreatedOn,UpdatedOn,UpdatedBy) 
												 Values(@RetValue,@ProposedId, @ProductTypeId, @ProductId,@Property,@Platform, @Quantity, @MRPAmount, @ProposedAmount,@Probability,@CreatedOn,@UpdatedOn,@UpdatedBy)

					IF  @Probability = 100
					   BEGIN
							INSERT INTO ESM_ROReceives(ESM_ProposedProductId,Discount,FInalROValue) Values(@RetValue,@Discount,@FinalROValue)
							--Log Data
							INSERT INTO  ESM_FreezeROReceiveLogs(ESM_ProposedProductId,Discount,FinalROValue,UpdatedBy,UpdatedOn) 
							VALUES(@RetValue,@Discount,@FinalROValue,@UpdatedBy,@CreatedOn)
						END	

				    END		
		    ELSE
			   BEGIN
					SET @RetValue = -1 -- For duplicate Entry
			   END
		END
	ELSE
		BEGIN
		    --Avoid dublicate Entry ....
		    SELECT PP.ID FROM ESM_ProposedProduct AS PP WITH(NOLOCK) WHERE PP.ProposedId=@ProposedId AND PP.ProductTypeId=@ProductTypeId AND PP.ProductId= @ProductId  AND PP.Property=@Property AND PP.Platform=@Platform AND PP.id <> @ID
			IF @@ROWCOUNT = 0
				 BEGIN

				        ---Update Last Probability 
						 IF NOT EXISTS(SELECT  id  FROM ESM_ProposedProduct WHERE Id = @ID AND ProductProbability = @Probability) 
							 BEGIN
								  SELECT  @LastProbability = ProductProbability  FROM ESM_ProposedProduct WHERE Id = @ID
				
								  UPDATE ESM_ProposedProduct SET ProductLastProbabilityUpdated = @LastProbability WHERE Id = @ID 
							 END

							  

						UPDATE ESM_ProposedProduct SET ProductTypeId = @ProductTypeId, ProductId = @ProductId,Property=@Property,Platform=@Platform, Quantity = @Quantity, MRPAmount = @MRPAmount, 
							ProposedAmount = @ProposedAmount, UpdatedOn = @UpdatedOn, UpdatedBy = @UpdatedBy, ProductProbability= @Probability
						WHERE id = @ID		

						--Log The data 
						INSERT INTO ESM_ProposedProductLogs(ESM_ProposedProductId,ProposedId,ProductTypeId,ProductId,Property,Platform,Quantity,MRPAmount,ProposedAmount,Probability,UpdatedOn,UpdatedBy) 
											 Values(@ID,@ProposedId, @ProductTypeId, @ProductId,@Property,@Platform, @Quantity, @MRPAmount, @ProposedAmount,@Probability,@UpdatedOn,@UpdatedBy)


						-- Added By Vinay kumar  prajapati 4 desc 2014 
						--freeze proposal and keep log  when Probility is 100 
					    IF  @Probability = 100
						BEGIN
							SELECT RR.Id FROM ESM_ROReceives AS RR WITH(NOLOCK) WHERE RR.ESM_ProposedProductId=@ID
							IF @@ROWCOUNT <> 0		
								BEGIN
									UPDATE ESM_ROReceives SET Discount=@Discount ,FInalROValue= @FinalROValue WHERE ESM_ProposedProductId=@ID										
								END	  
							ELSE
							   BEGIN
									INSERT INTO ESM_ROReceives(ESM_ProposedProductId,Discount,FInalROValue) Values(@ID,@Discount,@FinalROValue)
							   END
						   --Log Data
							INSERT INTO  ESM_FreezeROReceiveLogs(ESM_ProposedProductId,Discount,FinalROValue,UpdatedBy,UpdatedOn) 
							VALUES(@ID,@Discount,@FinalROValue,@UpdatedBy,@CreatedOn)
						  END

					SET @RetValue =  @ID
				    SET @ESM_ProposedProductId=@RetValue			
		         END	
	        ELSE
			    BEGIN
					SET @RetValue = -1 --To Avoid dublicate Update
				END
	   END
	   
	   -- log the Data only when probability is change by previous update.
	  SELECT Top 1 @LastProbability  = ISNULL(UpdatedProbability,0)  FROM ESM_ProbabilityLog AS EPL WITH(NOLOCK)
	  WHERE EPL.proposalId = @ProposedId AND EPL.ESM_ProposedProductId=@ESM_ProposedProductId  ORDER BY UpdatedOn desc	 
  
  	  IF (@LastProbability <> @Probability)
		BEGIN
		   INSERT INTO ESM_ProbabilityLog (proposalId,ESM_ProposedProductId, UpdatedProbability, UpdatedBy, UpdatedOn)
		                            VALUES(@ProposedId,@ESM_ProposedProductId, @Probability, @UpdatedBy, @UpdatedOn)
		END		
END

