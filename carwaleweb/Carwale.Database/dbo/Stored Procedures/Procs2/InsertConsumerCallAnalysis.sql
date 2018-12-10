IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertConsumerCallAnalysis]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertConsumerCallAnalysis]
GO
	--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE

CREATE PROCEDURE [dbo].[InsertConsumerCallAnalysis]
	@BuyerCallDue		INT,
	@BuyerCalled		INT,
	@BuyerConversion	INT,
	@BuyerRevenue	INT,
	@SellerCallDue		INT,
	@SellerCalled		INT,
	@SellerConversion	INT,
	@SellerRevenue	INT,
	@RenewalCallDue	INT,
	@RenewalCalled	INT,
	@RenewalConversion	INT,
	@EntryDate		DATETIME
	

 AS
	BEGIN
		SELECT Id FROM ConsumerCallAnalysis WHERE EntryDate = @EntryDate
		
		IF @@ROWCOUNT =  0
			BEGIN
				INSERT INTO ConsumerCallAnalysis
				(
					BuyerCallDue, BuyerCalled, BuyerConversion, BuyerRevenue, SellerCallDue, 
					SellerCalled, SellerConversion, SellerRevenue, RenewalCallDue, RenewalCalled, RenewalConversion, EntryDate
				)
				VALUES
				(
					@BuyerCallDue, @BuyerCalled, @BuyerConversion, @BuyerRevenue, @SellerCallDue, 
					@SellerCalled, @SellerConversion, @SellerRevenue, @RenewalCallDue, @RenewalCalled, @RenewalConversion, @EntryDate
				)
			END
		ELSE
			BEGIN
				UPDATE ConsumerCallAnalysis SET 
					BuyerCallDue = @BuyerCallDue, BuyerCalled = @BuyerCalled, 
					BuyerConversion = @BuyerConversion, BuyerRevenue = @BuyerRevenue, 
					SellerCallDue = @SellerCallDue, SellerCalled = @SellerCalled, 
					SellerConversion = @SellerConversion, SellerRevenue = @SellerRevenue, 
					 RenewalCalled = @RenewalCalled, RenewalConversion = @RenewalConversion
				WHERE  EntryDate = @EntryDate
			END
	END
