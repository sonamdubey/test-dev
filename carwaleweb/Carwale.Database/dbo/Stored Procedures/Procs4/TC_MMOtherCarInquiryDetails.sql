IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_MMOtherCarInquiryDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_MMOtherCarInquiryDetails]
GO

	-- =============================================
--
-- Name:		TC_MMOtherCarInquiryDetails
-- Create date: 13-11-2013
-- Description:	Get the customer other car inquiries
-- Tables :      TC_MMvwUsedCarInquiries,CarModels
-- Created by :  Ranjeet kumar
-- Modified By: Nilesh Utture on on 27-12-2013 Added  C.FuelType, MvUC.AreaName in SELECT field
-- =============================================
CREATE PROCEDURE [dbo].[TC_MMOtherCarInquiryDetails]
@CustomerId int,
@InquiryId int
AS
BEGIN
	
	SET NOCOUNT ON;
	SELECT CM.Name, MvUC.MakeYear, MvUC.Kms, MvUC.Price, C.FuelType, MvUC.AreaName 
	FROM  TC_MMvwUsedCarInquiries AS MvUC 
	INNER JOIN CarModels CM ON  MvUC.CarModelId = CM.ID AND MvUC.CustomerId = @CustomerId AND MvUC.InquiryId <> @InquiryId
	LEFT JOIN CarFuelType C ON C.FuelTypeId = MvUC.FuelTypeId
END
