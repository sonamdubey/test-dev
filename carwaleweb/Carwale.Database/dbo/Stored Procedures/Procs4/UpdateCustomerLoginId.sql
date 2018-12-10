IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateCustomerLoginId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateCustomerLoginId]
GO

	

--Modifier : Vinay Kumar Prajapati 12 aug 2014
--purpose  : To Update EmailId from  table  CustomerSellInquiris
 
CREATE PROCEDURE [dbo].[UpdateCustomerLoginId] 
	@InquiryId		NUMERIC,
	@NewLoginId		VARCHAR(50),
	@IsUpdated		BIT OUTPUT
AS

BEGIN
	SELECT CSI.Id FROM CustomerSellInquiries AS CSI WITH(NOLOCK)
    WHERE CSI.CustomerEmail=@NewLoginId
	
	IF @@RowCount > 0
		BEGIN
			PRINT 'Not Updating'
			SET @IsUpdated = 0
		END
	ELSE
		BEGIN	
			UPDATE CustomerSellInquiries SET CustomerEmail=@NewLoginId WHERE Id=@InquiryId				
			SET @IsUpdated = 1
		END
END
