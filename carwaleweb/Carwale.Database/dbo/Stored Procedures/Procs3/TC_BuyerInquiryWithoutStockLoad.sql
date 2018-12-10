IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_BuyerInquiryWithoutStockLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_BuyerInquiryWithoutStockLoad]
GO

	
--modified by :Binu, 17-jul 2012, Removed carmake part
-- Author:		Surendra
-- Create date: 12 Jan 2012
-- Description:	This procedure will be used to bind Controls in Buyer Inquiry ADD form during load
-- Modified By : Tejashree Patil on 14 Jan 2013 ; Added parameters @BranchId and CustomerId and executed sp TC_GetCustomerDetails
-- EXEC [TC_BuyerInquiryWithoutStockLoad] 5,NULL
-- Modifeid By : umesh on 29 june 2013 for selecting source via dealer wise
-- =============================================
CREATE PROCEDURE [dbo].[TC_BuyerInquiryWithoutStockLoad]
	-- Modified By : Tejashree Patil on 14 Jan 2013	
	@BranchId BIGINT = NULL,
	@CustomerId BIGINT = NULL

AS
BEGIN
	
	-- Give All live Make as Table
	--EXECUTE TC_GetCarMake -- Commented by Binumon George  09 Jul 2012
	-- Give all Source as Table
	--EXECUTE TC_InquirySourceSelect -- Commented by umesh  29 June 2012

	EXECUTE TC_InquirySourceDealerWise @BranchId
	-- Give all status as Table
	EXECUTE TC_InquiryStatusSelect	
	
	-- Modified By : Tejashree Patil on 14 Jan 2013
	EXECUTE TC_GetCustomerDetails @BranchId, @CustomerId 
	
	
	--EXECUTE TC_InquirySourceSelectNew @ApplicationId  --Added by Afrose
END



