IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InquiriesCheckCustomerType]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InquiriesCheckCustomerType]
GO

	
-- =============================================
-- Author:		Surendra
-- Create date: 9th Feb,2012
-- Description:	Checking customer type
-- =============================================
CREATE PROCEDURE [dbo].[TC_InquiriesCheckCustomerType]
	@CustomerId BIGINT,
	@Type VARCHAR(8) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Count TINYINT
	SELECT @Count=COUNT(*) FROM TC_InquiriesLead WHERE TC_CustomerId =@CustomerId 
	
	IF(@Count=2)
	BEGIN 
		SET @Type='Both'
		RETURN -1
	END
	
	IF EXISTS(SELECT TC_InquiriesLeadId FROM TC_InquiriesLead WHERE TC_CustomerId =@CustomerId AND TC_InquiryTypeId=1)
		BEGIN
			SET @Type='Buyer'		
		END
	ELSE
		BEGIN
			SET @Type='Seller'	
		END	
END




