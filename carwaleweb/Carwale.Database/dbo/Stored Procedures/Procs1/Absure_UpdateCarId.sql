IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_UpdateCarId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_UpdateCarId]
GO

	-- =====================================================
--Author      : Suresh Prajapati
--Created On  : 24th Feb, 2015
--Summary     : Proc to  update customer details
-- =====================================================
CREATE PROCEDURE [dbo].[Absure_UpdateCarId]
	-- Add the parameters for the stored procedure here
	@AbsureCarId INT
	,@WarrantyInquiryId INT
AS
BEGIN
	UPDATE Absure_WarrantyInquiries
	SET AbsureCarId = @AbsureCarId
	WHERE Id = @WarrantyInquiryId
END


