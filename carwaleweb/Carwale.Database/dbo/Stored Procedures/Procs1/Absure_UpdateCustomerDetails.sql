IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_UpdateCustomerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_UpdateCustomerDetails]
GO

	-- =====================================================
--Author      : Suresh Prajapati
--Created On  : 24th Feb, 2015
--Summary     : Proc to  update customer details
-- =====================================================
CREATE PROCEDURE [dbo].[Absure_UpdateCustomerDetails]
	-- Add the parameters for the stored procedure here
	--@WarrantyType TINYINT
	@WarrantyInquiryId INT
	,@CityId BIGINT
	,@CustomerName VARCHAR(50) = NULL
	,@CustomerAddress VARCHAR(100) = NULL
	,@CustomerMobileNo VARCHAR(50) = NULL
	,@CustomerEmail VARCHAR(50) = NULL
	,@AreaId INT = NULL
AS
BEGIN
	UPDATE Absure_WarrantyInquiries
	SET --WarrantyType = @WarrantyType
		CustomerName = @CustomerName
		,CustomerEmail = @CustomerEmail
		,CustomerMobile = @CustomerMobileNo
		,CustomerAddress = @CustomerAddress
		,CityId = @CityId
		,AreaId = @AreaId
	WHERE Id = @WarrantyInquiryId
END