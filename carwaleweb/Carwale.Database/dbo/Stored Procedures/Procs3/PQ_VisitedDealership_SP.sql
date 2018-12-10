IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PQ_VisitedDealership_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PQ_VisitedDealership_SP]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/*
Name: PQ_VisitedDealership_SP
Description: This Procedure updates the information on whether the customer requesting a Price Quote
				 has visited a dealership or not.
Created By: Vikas
Created On: 14-02-2012
*/

CREATE Procedure [dbo].[PQ_VisitedDealership_SP]
@InquiryId Numeric,
@VisitedDealership Bit
As
Begin
	Update NewCarPurchaseInquiries set VisitedDealership = @VisitedDealership Where Id = @InquiryId
End

