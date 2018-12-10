IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CW_UpdateNewCarPurchaseCities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CW_UpdateNewCarPurchaseCities]
GO

	-- =============================================          
-- Author:  <Vinayak M>          
-- Description: <Update customer information to NewPurchaseCities  >          
-- Create By: Vinayak M <11/08/2014>    

-- =============================================          
CREATE PROCEDURE [dbo].[CW_UpdateNewCarPurchaseCities] 
	@InquiryId numeric,
	@Name varchar(100),
	@EmailId varchar(100),
	@PhoneNo varchar(100)	
AS
BEGIN
	
	UPDATE NewPurchaseCities 
	SET Name=@Name,EmailId=@EmailId,PhoneNo=@PhoneNo
	WHERE InquiryId= @InquiryId;

END



