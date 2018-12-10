IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AddNewCarPurchaseInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AddNewCarPurchaseInquiry]
GO

	
-- =============================================
-- Author:		Vaibhav K
-- Create date: 09-July-2012
-- Description:	Adding new car purchase inquiry for the customer
-- =============================================
CREATE PROCEDURE [dbo].[AddNewCarPurchaseInquiry]
	-- Add the parameters for the stored procedure here
	@CustomerId			INT,
	@CarVersion			INT,
	@BuyTime			VARCHAR(50),
	@RequestDate		DATETIME,
	@ForwardedLead		BIT,
	@SourceId			INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO NewCarPurchaseInquiries (CustomerId,CarVersionId,BuyTime,RequestDateTime,ForwardedLead,SourceId)
	VALUES (@CustomerId,@CarVersion,@BuyTime,@RequestDate,@ForwardedLead,@SourceId)
END

