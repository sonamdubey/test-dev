IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_NewCarPurchaseAddCustomerInTask]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_NewCarPurchaseAddCustomerInTask]
GO

	-- =============================================
-- Author:		Ranjeet Kumar
-- Create date: 24-feb-2014
-- Description: Get the parameters for TC_INQNewCarBuyerSave sp.
-- Table : TC_NewCarPurchaseDealerPoints, TC_NewCarPurchaseLead

-- =============================================
CREATE PROCEDURE [dbo].[TC_NewCarPurchaseAddCustomerInTask]
	-- Add the parameters for the stored procedure here
	@DealerId INT,
	@NewCarPurchaseInquiriesId INT,
	@NoOfDayOldCustomer INT
	
AS
BEGIN
	SET NOCOUNT ON;
	
	BEGIN
		-- Insert statements for procedure here
		SELECT TOP 1 TNCP.CustomerName AS CustomerName,
			     TNCP.Mobile AS CustomerMobile ,
				TNCP.email AS CustomerEmail,
				TNCP.VersionId AS VersionId,
				TNCP.CityId AS CityId,
				TNCP.BuyTime AS Buytime,
				 TNCP.ModelId AS ModelId,
				TNCP.CWCustomerId AS CW_CustomerId, 
				TNCP.ReqDate AS CreatedOn,
				NCDP.CurrentPoint AS CurrentPoint
		FROM 
		TC_NewCarPurchaseLead AS TNCP   with(NOLOCK)
		LEFT JOIN  TC_NewCarPurchaseDealerPoints AS NCDP ON NCDP.DealerId = @DealerId
		WHERE TNCP.NewCarPurchaseId = @NewCarPurchaseInquiriesId;
		END
		
		
END
