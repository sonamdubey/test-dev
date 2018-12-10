IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ESM_SaveContact]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ESM_SaveContact]
GO

	
/*    
 This procedure created on 13 Jan 2010 by Sentil    
 for update and save for Contacts     
*/    
CREATE PROCEDURE [dbo].[ESM_SaveContact]  
(  
	@ClientId AS NUMERIC(18,0),  
	@Name AS VARCHAR(50),  
	@Mobile AS NUMERIC(18,0),  
	@email AS VARCHAR(50),  
	@LandLine AS NUMERIC(18,0),  
	@Designation AS VARCHAR(50),  
	@Fax AS NUMERIC(18,0),   
	@Address AS VARCHAR(50),  
	@stateId AS NUMERIC(18,0),  
	@cityId AS NUMERIC(18,0),  
	@ID AS NUMERIC(18,0),  
	@Remarks AS VARCHAR(250),  
	@UpdatedOn AS DATETIME,  
	@UpdatedBy AS NUMERIC(18,0),
	@retVal AS BIGINT OUT  
)  
AS  
BEGIN  
  
	  IF(@ID = -1)    
		  BEGIN    
			   INSERT INTO ESM_Contact ( ClientId, Name, Mobile, email, LandLine, Designation, Fax, Address,   
										 stateId, cityId, Remarks, UpdatedOn, UpdatedBy )  
								VALUES ( @ClientId, @Name, @Mobile, @email, @LandLine, @Designation, @Fax, @Address,   
										 @stateId, @cityId, @Remarks, @UpdatedOn, @UpdatedBy )
				
				SET @retVal = SCOPE_IDENTITY()						   
		  END  
	 ELSE  
		  BEGIN  
			  UPDATE ESM_Contact SET     
				   ClientId = @ClientId, Name = @Name, Mobile = @Mobile, email = @email,   
				   LandLine = @LandLine, Designation = @Designation, Fax = @Fax, Address = @Address,   
				   stateId = @stateId, cityId = @cityId, Remarks = @Remarks, UpdatedOn = @UpdatedOn, UpdatedBy = @UpdatedBy      
			  WHERE id = @ID
			  
			  SET @retVal = @ID     
		  END                 
         
--SELECT * FROM ESM_Contact  
--TRUNCATE TABLE ESM_Contact  
END

