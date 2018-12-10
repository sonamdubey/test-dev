IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SaveStockInfo_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SaveStockInfo_SP]
GO

	
-- =============================================
-- Modified By:   Reshma Shetty
-- Modified date: 30/08/2012
-- Description:   Code has been added to set EMI for HDFC empaneled dealers
-- =============================================
---Modified by :binu Date:14-06-2012, Desc:not allow to update lastupdated date in SellInquiries tbl twice in a day
-- ModifiedBy:	Surendra
-- Create date: 15 may 2012
-- Description:	last updated date will changed only in case where ip price is greater then exisiting price
-- =============================================
-- ModifiedBy:	Binumon George
-- Create date: 11 may 2012
-- Description:	checking reg no exist or not under same dealer.
-- Modified By: Tejashree Patil On 5 July 2012: Added @IsParkNSale parameter 

CREATE PROCEDURE [dbo].[TC_SaveStockInfo_SP]
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
@IsParkNSale BIT,
@CertificationId smallint ,
@ModifiedBy INT,
@ExistingStockId NUMERIC OUTPUT -- not used need to check other parent SPs
      
AS       
      
BEGIN        
	      
IF @StockId = -1
	BEGIN  
		--checking stock existing or not with same regno and banchid while insert new stock 11 apr 2012  
		--IF NOT EXISTS(SELECT TOP 1 Id FROM TC_Stock WHERE BranchId=@BranchId AND  RegNo=@RegNo)
		--	BEGIN
				INSERT INTO TC_Stock( BranchId, VersionId, StatusId, Price, Kms, MakeYear, Colour, RegNo, EntryDate, LastUpdatedDate, CertificationId, ModifiedBy, IsParkNSale)	         
				VALUES( @BranchId, @VersionId, @StatusId, @Price, @Kms, @MakeYear, @Colour, @RegNo, @EntryDate, @LastUpdatedDate,@CertificationId,@ModifiedBy,@IsParkNSale)        
				      
				SET @ID = SCOPE_IDENTITY()
				    
				INSERT INTO TC_CarCondition(StockId, RegistrationPlace, Insurance, InsuranceExpiry, Owners, OneTimeTax, LastUpdatedDate,ModifiedBy)          
				VALUES (@ID, @RegPlace, @Insurance, @InsuranceExpiry, @Owners, @Tax, @LastUpdatedDate,@ModifiedBy)
			--END
		--ELSE
			--BEGIN
				--SET @ID=-1
			--	SELECT TOP 1 @ExistingStockId=Id FROM TC_Stock WHERE BranchId=@BranchId AND  RegNo=@RegNo ORDER BY Id DESC
			--END
	END
ELSE          
	BEGIN
		--checking stock existing or not with same regno and branchid while updating 11 apr 2012  
		--IF NOT EXISTS(SELECT TOP 1 Id FROM TC_Stock WHERE BranchId=@BranchId AND  RegNo=@RegNo AND Id <> @StockId)
			--BEGIN
				DECLARE @IsProperUpdate TINYINT,@DiffYear TINYINT,@PrevPrice BIGINT
				SELECT @IsProperUpdate=COUNT(Id) FROM TC_Stock WHERE Id=@StockId AND BranchId=@BranchId
				-- @IsProperUpdate>0 Means user is updating his stock only
				IF(@StatusId=1 AND @IsProperUpdate > 0)-- If Stock is available than only need to upadate stock info in carwale and Trading car
				BEGIN
					IF EXISTS(SELECT Top 1 * FROM SellInquiries WHERE TC_StockId = @StockId AND StatusId=1)
						BEGIN -- updating only if car is available to carwale
							UPDATE SellInquiries SET CarVersionId=@VersionId,CarRegNo=@RegNo,StatusId=@StatusId,
							Price=@Price,MakeYear=@MakeYear,Kilometers=@Kms,Color=@Colour,ModifiedDate=@LastUpdatedDate,
							CertificationId=@CertificationId
							WHERE TC_StockId = @StockId
							--not allow to update LastUpdated date when if it updated with in same day
							IF EXISTS(SELECT ID FROM SellInquiries WHERE CONVERT(VARCHAR(8),LastUpdated ,112)!=CONVERT(VARCHAR(8),GETDATE(),112) AND TC_StockId=@StockId)
								BEGIN
									UPDATE SellInquiries SET LastUpdated=@LastUpdatedDate WHERE TC_StockId = @StockId AND DealerId=@BranchId
								END	
							
							UPDATE SellInquiriesDetails SET Owners=@Owners,OneTimeTax=@Tax,Insurance=@Insurance,InsuranceExpiry=@InsuranceExpiry
							WHERE SellInquiryId=(SELECT ID FROM SellInquiries WHERE TC_StockId = @StockId)
							
							-- Modified By: Reshma Shetty Date: 30/08/2012 -- Added parameter @EMI to calculate EMI for HDFC empaneled dealers
							-- Recalculate and update the EMI if the Price has been updated and if it is eligible
							-- Modified 26/09/2012 Passing @DiffYear instead of 5-@DiffYear due to change in requirements
							SET @DiffYear = DATEDIFF(YEAR,@MakeYear,GETDATE())
							IF(--@PrevPrice <>  @Price AND 
							dbo.HDFCVehicleEligiblity(@BranchId,@Price,@Owners,@DiffYear)=1)
							BEGIN
								UPDATE Sellinquiries
								SET CalculatedEMI=dbo.CalculateEMI(@Price,@DiffYear,16.5)
								WHERE TC_StockId = @StockId AND DealerId=@BranchId
							END
							ELSE
							BEGIN
								UPDATE Sellinquiries
								SET CalculatedEMI=NULL
								WHERE TC_StockId = @StockId AND DealerId=@BranchId
							END							
						END
					-- Updating Stock details in trading car
					UPDATE TC_Stock SET VersionId=@VersionId, StatusId=@StatusId, LastUpdatedDate = @LastUpdatedDate,
					RegNo=@RegNo, Price=@Price, MakeYear=@MakeYear, Kms=@Kms, Colour=@Colour, CertificationId= @CertificationId,ModifiedBy=@ModifiedBy,
					IsParkNSale=@IsParkNSale
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
			
		--	END--
		---ELSE
		--	BEGIN
			--	SET @ID = -1
			--	SELECT TOP 1 @existingStockId=Id FROM TC_Stock WHERE BranchId=@BranchId AND  RegNo=@RegNo
		--	END
	END 
	          
END          




