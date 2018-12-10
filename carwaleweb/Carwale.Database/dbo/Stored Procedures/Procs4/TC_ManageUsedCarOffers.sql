IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ManageUsedCarOffers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ManageUsedCarOffers]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 27-12-2013
-- Description:	Saving and updating Details of Used Car Offers.TC_UsedCarOffers
-- Modified by Vinayak Patil on 21-01-2014 -- Upadated existing stock having given offerid with new startdate and enddate
-- =============================================
CREATE PROCEDURE [dbo].[TC_ManageUsedCarOffers]
@BranchId INT,
@TC_UsedCarOfferId INT,
@OfferName VARCHAR(80),
@ValidFrom DATETIME,
@ValidTo DATETIME,
@OfferAmount INT,
@Description VARCHAR(200),
@Terms VARCHAR(200),
@ReturnId INT = 0 OUTPUT 
AS
BEGIN	
	SET NOCOUNT ON;
	SELECT COUNT(TC_UsedCarOfferId) FROM TC_UsedCarOffers WITH(NOLOCK) WHERE BranchId = @BranchId
		   IF(@TC_UsedCarOfferId IS NULL AND (SELECT COUNT(TC_UsedCarOfferId) FROM TC_UsedCarOffers WITH(NOLOCK) WHERE BranchId = @BranchId AND IsActive = 1) < 5)
		   BEGIN      
			  INSERT INTO TC_UsedCarOffers 
						 (
							  BranchId,
							  OfferName,
							  StartDate,
							  EndDate,
							  OfferAmount,
							  Description,
							  Terms
						  )

			   VALUES 
						  ( 
							  @BranchId,
							  @OfferName,
							  @ValidFrom,
							  @ValidTo,
							  @OfferAmount,
							  @Description,
							  @Terms
						  )
            SET @ReturnId = SCOPE_IDENTITY()

			END

			ELSE IF(@TC_UsedCarOfferId IS NOT NULL) 
			BEGIN
				UPDATE TC_UsedCarOffers
				SET
				   BranchId = @BranchId,
				   OfferName = @OfferName,
				   StartDate = @ValidFrom,
				   EndDate = @ValidTo,
				   OfferAmount = @OfferAmount,
				   Description = @Description,
				   Terms = @Terms,
				   ModifiedDate = GETDATE()
				WHERE 
				TC_UsedCarOfferId = @TC_UsedCarOfferId AND BranchId = @BranchId

		    --Modified by Vinayak Patil on 21-01-2014 -- Upadated existing stock having UsedCarOfferId as
			-- given offerid with new startdate and enddate
			
			IF EXISTS(SELECT TOP 1 * FROM TC_MappingOfferWithStock
					  WHERE TC_UsedCarOfferId=@TC_UsedCarOfferId)
			BEGIN
				UPDATE TC_MappingOfferWithStock
				SET StartDate = @ValidFrom
					,EndDate = @ValidTo
				WHERE TC_UsedCarOfferId = @TC_UsedCarOfferId
				
			END	

			SET @ReturnId = @TC_UsedCarOfferId

			END

END













/****** Object:  StoredProcedure [dbo].[TC_UsedCarOfferDetails]    Script Date: 2/5/2014 7:22:33 PM ******/
SET ANSI_NULLS ON
