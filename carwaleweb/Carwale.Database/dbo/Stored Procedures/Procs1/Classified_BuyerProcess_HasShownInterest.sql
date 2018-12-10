IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_BuyerProcess_HasShownInterest]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_BuyerProcess_HasShownInterest]
GO

	-- =============================================
-- Author:		Purohith Guguloth
-- Create date: 06th jan, 2016
-- Description:	Checks Whether User has shown interest in the Car during the Buyer's Process in the Classified Section for the last one day
			  --Takes InquiryId, CustomerId and IsDealer as Input Parameters 
			  --Returns True if there is an Entry present in Inquiries Table and False when the Entry is not Present
-- =============================================
CREATE PROCEDURE [dbo].[Classified_BuyerProcess_HasShownInterest] 
	-- Add the parameters for the stored procedure here
	@CustomerId INT,
	@InquiryId INT,
	@IsDealer BIT,
	@ShownInterest BIT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF @IsDealer = 1
	BEGIN
		IF EXISTS (SELECT ID FROM UsedCarPurchaseInquiriesSentSMSDetail WITH (NOLOCK) WHERE SMSSentDate > GETDATE() -1 AND CustomerID = @CustomerId AND SellInquiryId = @InquiryId)
		BEGIN
			SET @ShownInterest = 1;
		END
		ELSE
		BEGIN
			--UPDATE UsedCarPurchaseInquiries SET SMSSentDate = GETDATE() WHERE CustomerID = @CustomerId AND SellInquiryId = @InquiryId 
			SET @ShownInterest = 0;
		END
	END
	ELSE
	BEGIN
		IF EXISTS(SELECT ID FROM ClassifiedRequestsSentSMSDetail WITH (NOLOCK) WHERE SMSSentDate > GETDATE() -1 AND CustomerID = @CustomerId AND SellInquiryId = @InquiryId)
		BEGIN
			SET @ShownInterest = 1
		END
		ELSE
		BEGIN
			--UPDATE ClassifiedRequests SET SMSSentDate = GETDATE() WHERE CustomerID = @CustomerId AND SellInquiryId = @InquiryId
			SET  @ShownInterest = 0
		END
	END
END

