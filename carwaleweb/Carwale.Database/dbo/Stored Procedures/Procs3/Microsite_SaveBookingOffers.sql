IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_SaveBookingOffers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_SaveBookingOffers]
GO

	
CREATE PROCEDURE [dbo].[Microsite_SaveBookingOffers]
@TransactionId numeric(18,0),
@OffersIds varchar(50)
AS 
-- Author: Rakesh Yadav
-- Date Created: 10 April 2015
-- Desc: Remove all offers for transactionId and resave newly selected offers
BEGIN

	DELETE FROM Microsite_BookingTransactionOffers 
	WHERE Microsite_BookingTransactionId=@TransactionId

	INSERT INTO Microsite_BookingTransactionOffers (Microsite_BookingTransactionId,Microsite_DealerOffersId) 
	SELECT @TransactionId,a.ListMember FROM dbo.[fnSplitCSV](@OffersIds) as a
END


