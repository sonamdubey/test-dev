IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_UpdateWarrantyType]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_UpdateWarrantyType]
GO

	-- =============================================
-- Author      : Suresh Prajapati
-- Create date : 23rd Mar, 2015
-- Description : Procedure to Update Warranty Type
-- =============================================
CREATE PROCEDURE [dbo].[Absure_UpdateWarrantyType]
	-- Add the parameters for the stored procedure here
	@WarrantyInquiryId INT
	,@WarrantyType TINYINT
	,@WarrantyPrice FLOAT
	--,@ProductType TINYINT
AS
BEGIN
	UPDATE Absure_WarrantyInquiries
	SET WarrantyPrice = @WarrantyPrice
		,WarrantyType = @WarrantyType
	WHERE Id = @WarrantyInquiryId
END
