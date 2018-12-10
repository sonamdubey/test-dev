IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ESM_SaveProduct]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ESM_SaveProduct]
GO

	/*    
 This procedure created on 13 Jan 2010 by Sentil    
 for update and save for Product 
 Updated By : Vinay Kumar Prajapati 11 desc 2014
 Purpose :  To Avoid Duplicate Entry    
*/    
CREATE PROCEDURE [dbo].[ESM_SaveProduct]  
(  
	@Product AS VARCHAR(50),
	@ProductType AS Int,
	@UnitPrice AS DECIMAL(18,2),
	@MinimumPrice AS DECIMAL(18,2),
	@IsActive AS BIT,
	@ID AS NUMERIC(18,0),
	@UpdatedOn AS DATETIME,
	@UpdatedBy AS NUMERIC(18,0)
)  
AS  
BEGIN  
  
	  IF(@ID = -1)    
		  BEGIN  
		        --Avoid Duplicate Entry
			   SELECT Ep.id FROM  ESM_Products AS EP WITH(NOLOCK) WHERE EP.Product=@Product AND EP.ProductType=@ProductType 
			   IF @@ROWCOUNT = 0
				   BEGIN
					   INSERT INTO ESM_Products ( Product, ProductType, UnitPrice, MinimumPrice, IsActive, UpdatedOn, UpdatedBy )  
					   VALUES ( @Product, @ProductType, @UnitPrice, @MinimumPrice, @IsActive, @UpdatedOn, @UpdatedBy )
				   END
			
		  END  
	 ELSE  
		  BEGIN
		       --Avoid duplicate Update
		       SELECT Ep.id FROM  ESM_Products AS EP WITH(NOLOCK) WHERE EP.Product=@Product AND EP.ProductType=@ProductType  AND Ep.id<> @ID 
			   IF @@ROWCOUNT = 0
				   BEGIN  
					  UPDATE ESM_Products SET     
						   Product = @Product, ProductType = @ProductType, UnitPrice = @UnitPrice, MinimumPrice = @MinimumPrice,   
						   IsActive = @IsActive, UpdatedOn = @UpdatedOn, UpdatedBy = @UpdatedBy      
					  WHERE id = @ID
			       END		  
		  END                 
END


