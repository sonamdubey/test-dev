IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SellerInquiryLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SellerInquiryLoad]
GO

	--modified by:Binu,17 jul 2012, removed carmake proc
-- Author:		Surendra
-- Create date: 12 Jan 2012
-- Description:	This procedure will be used to bind Controls in Seller Inquiry ADD form during load
-- Modified By : Tejashree Patil on 14 Jan 2013 ; Added parameters @BranchId and CustomerId and executed sp TC_GetCustomerDetails
-- EXEC [TC_SellerInquiryLoad] 5,21
-- Modifeid By : umesh on 29 june 2013 for selecting source via dealer wise
-- modified by vivek gupta on 25-11-2015, added @CustomerId to get extra source which we need to show in autobiz pannel
-- =============================================
CREATE PROCEDURE [dbo].[TC_SellerInquiryLoad]
	-- Modified By : Tejashree Patil on 14 Jan 2013	
	@BranchId BIGINT = NULL,
	@CustomerId BIGINT = NULL
AS
BEGIN
	-- Give all Make as Table
	--EXECUTE TC_GetCarMake -- Commented by Binumon George  09 Jul 2012
	
	--EXECUTE TC_InquirySourceSelect -- Commented by umesh  29 June 2012
	EXECUTE TC_GetInquirySource 1, @CustomerId
	-- Give all status as Table 
	EXECUTE TC_InquiryStatusSelect
	
	-- Modified By : Tejashree Patil on 14 Jan 2013
	EXECUTE TC_GetCustomerDetails @BranchId, @CustomerId 
END







/****** Object:  StoredProcedure [dbo].[TC_GetInquirySource]    Script Date: 12/2/2015 3:24:20 PM ******/
SET ANSI_NULLS ON
