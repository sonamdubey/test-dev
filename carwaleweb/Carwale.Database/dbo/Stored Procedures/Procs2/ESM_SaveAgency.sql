IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ESM_SaveAgency]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ESM_SaveAgency]
GO

	
/*  
 This procedure created on 13 Jan 2010 by Sentil  
 for update and save for ESM_Agency   
*/ 
CREATE PROCEDURE [dbo].[ESM_SaveAgency]
(
	@ClientId AS NUMERIC(18,0),
	@AgencyId AS NUMERIC(18,0),
	@BrandId AS NUMERIC(18,0),
	@ID AS NUMERIC(18,0),
	@UpdatedOn AS DATETIME,
	@UpdatedBy AS NUMERIC(18,0)
)
AS
BEGIN

	IF(@ID = -1)  
		BEGIN  
		   INSERT INTO ESM_Agency ( ClientId, AgencyId, BrandId, UpdatedOn, UpdatedBy )  
					VALUES( @ClientId, @AgencyId, @BrandId, @UpdatedOn, @UpdatedBy )  
		END  
	ELSE  
		BEGIN  
			UPDATE ESM_Agency SET   
			ClientId = @ClientId, AgencyId = @AgencyId, BrandId = @BrandId, UpdatedOn = @UpdatedOn,UpdatedBy = @UpdatedBy  
			WHERE id = @ID  
		END   
--SELECT * FROM ESM_Agency
--TRUNCATE TABLE ESM_Agency
END
