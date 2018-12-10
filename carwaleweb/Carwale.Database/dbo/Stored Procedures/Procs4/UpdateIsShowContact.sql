IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateIsShowContact]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateIsShowContact]
GO

	
-- =============================================
-- Author:		Aditi Dhaybar
-- Create date: 15 April 2015
-- Description:	Proc to update the IsShowContact flag in usedcarsellinquiries
-- =============================================
CREATE PROCEDURE [dbo].[UpdateIsShowContact]
	-- Add the parameters for the stored procedure here
	@InquiryId  NUMERIC(18, 0)	
   ,@ShowContact BIT
   ,@IsUpdated  BIT = 0 OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE  CustomerSellInquiries 
	SET IsShowContact = @ShowContact 
	WHERE Id = @InquiryId

	SET @IsUpdated = 1 ;
END


