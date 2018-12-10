IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SaveStockInfo_SP_Backup]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SaveStockInfo_SP_Backup]
GO

	-- =============================================
-- ModifiedBy:		Binumon George
-- Create date: 08-11-2011
-- Description:	Added ModifiedBy parameter and modified date.
-- =============================================
-- Modified By:		Surendra
-- Create date: 7th Oct, 2011
-- Description:	To addopt Securiry if user is trying to access other than his stock	

CREATE PROCEDURE [dbo].[TC_SaveStockInfo_SP_Backup]
@StockId  NUMERIC, -- Stock Id          
@VersionId  NUMERIC, -- Car Version Id          
@BranchId NUMERIC,
@StatusId INT, -- Car Status
@EntryDate DATETIME, -- Entry Date          
@MakeYear  DATETIME,          
@RegNo  VARCHAR(50),          
@Kms   NUMERIC,          
@Price   NUMERIC,          
@Colour   VARCHAR(50), 
@LastUpdatedDate DATETIME, -- Updated Time
     
-- Additional Details        
@RegPlace  VARCHAR(50),          
@Owners  NUMERIC,          
@Tax   VARCHAR(50),          
@Insurance  VARCHAR(50),          
@InsuranceExpiry DATETIME,
@ID   NUMERIC OUTPUT ,  
@CertificationId smallint,
@ModifiedBy INT      
      
AS       
      
BEGIN        
	      
IF @StockId = -1        
	BEGIN   

		INSERT INTO TC_Stock( BranchId, VersionId, StatusId, Price, Kms, MakeYear, Colour, RegNo, EntryDate, LastUpdatedDate, CertificationId, ModifiedBy)	         
		VALUES( @BranchId, @VersionId, @StatusId, @Price, @Kms, @MakeYear, @Colour, @RegNo, @EntryDate, @LastUpdatedDate,@CertificationId,@ModifiedBy )        
		      
		SET @ID = SCOPE_IDENTITY()
		    
		INSERT INTO TC_CarCondition(StockId, RegistrationPlace, Insurance, InsuranceExpiry, Owners, OneTimeTax, LastUpdatedDate,ModifiedBy)          
		VALUES (@ID, @RegPlace, @Insurance, @InsuranceExpiry, @Owners, @Tax, @LastUpdatedDate,@ModifiedBy)
	END
ELSE          
	BEGIN
		
		DECLARE @IsProperUpdate tinyint
		SELECT @IsProperUpdate=COUNT(Id) FROM TC_Stock WHERE Id=@StockId AND BranchId=@BranchId
		-- @IsProperUpdate>0 Means user is updating his stock only
		IF(@StatusId=1 AND @IsProperUpdate > 0)-- If Stock is available than only need to upadate stock info in carwale and Trading car
		BEGIN
			IF EXISTS(SELECT Top 1 * FROM SellInquiries WHERE TC_StockId = @StockId AND StatusId=1)
			BEGIN -- updating only if car is available to carwale
				UPDATE SellInquiries SET CarVersionId=@VersionId,CarRegNo=@RegNo,StatusId=@StatusId,
				Price=@Price,MakeYear=@MakeYear,Kilometers=@Kms,Color=@Colour,LastUpdated=@LastUpdatedDate,CertificationId=@CertificationId
				WHERE TC_StockId = @StockId
				
				UPDATE SellInquiriesDetails SET Owners=@Owners,OneTimeTax=@Tax,Insurance=@Insurance,InsuranceExpiry=@InsuranceExpiry
				WHERE SellInquiryId=(SELECT ID FROM SellInquiries WHERE TC_StockId = @StockId)							
			END
			-- Updating Stock details in trading car
			UPDATE TC_Stock SET VersionId=@VersionId, StatusId=@StatusId, LastUpdatedDate = @LastUpdatedDate,
			RegNo=@RegNo, Price=@Price, MakeYear=@MakeYear, Kms=@Kms, Colour=@Colour, CertificationId= @CertificationId,ModifiedBy=@ModifiedBy
			WHERE ID=@StockId
			        
			UPDATE TC_CarCondition SET RegistrationPlace = @RegPlace, Insurance = @Insurance,         
			InsuranceExpiry = @InsuranceExpiry, Owners = @Owners, OneTimeTax = @Tax, LastUpdatedDate = @LastUpdatedDate,ModifiedBy=@ModifiedBy
			WHERE StockId = @StockId 
		END
		
		ELSE-- No need to update other details because car is not available
		BEGIN
			IF EXISTS(SELECT Top 1 * FROM SellInquiries WHERE TC_StockId = @StockId AND StatusId=1 AND DealerId=@BranchId)
			BEGIN
				UPDATE SellInquiries SET StatusId=2 WHERE TC_StockId = @StockId --making car unavailable to carwale
			END
			UPDATE TC_Stock SET IsSychronizedCW=0,StatusId=@StatusId,ModifiedBy=@ModifiedBy WHERE Id=@StockId AND BranchId=@BranchId -- changing status of stock in tading cars
		END
		
		SET @ID = @StockId
	END            
END          


/****** Object:  StoredProcedure [dbo].[TC_USER_RollAssign]    Script Date: 11/10/2011 18:08:47 ******/
SET ANSI_NULLS ON
