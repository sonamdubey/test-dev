IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetSellCarStepsCompleted]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetSellCarStepsCompleted]
GO

	-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 16 Nov 2013
-- Description:	Proc to get the sell car steps completed by the customer.
-- =============================================
CREATE PROCEDURE [dbo].[GetSellCarStepsCompleted]

	-- Add the parameters for the stored procedure here
	--@CustomerId BIGINT,
	@InquiryId BIGINT,
	@SellCarStepsCompleted TINYINT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here	
		
	SELECT @SellCarStepsCompleted = CurrentStep
	FROM CustomerSellInquiries
	WHERE ID = @InquiryId-- AND CustomerId = @CustomerId
	
END

