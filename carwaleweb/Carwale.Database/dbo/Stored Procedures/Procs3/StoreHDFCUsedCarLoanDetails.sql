IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[StoreHDFCUsedCarLoanDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[StoreHDFCUsedCarLoanDetails]
GO

	-- =============================================
-- Author:        <kush kumar>
-- Create date: <20/9/2012>
-- Description:    <For HDFC Used Car Loan Implementation>
-- =============================================
CREATE PROCEDURE  [dbo].[StoreHDFCUsedCarLoanDetails]

    @SellerInquiryId BigInt,
    @BuyerId BigInt,
    @EntryDate DateTime 
    AS 
    
    IF EXISTS ( SELECT LL.CalculatedEMI from LiveListings LL WHERE LL.Inquiryid = @SellerInquiryId AND CalculatedEMI IS NOT NULL) 
    BEGIN    
        INSERT INTO HDFCUsedCarLoanData(SellerInquiryId,BuyerId,EntryDate) 
        VALUES (@SellerInquiryId,@BuyerId,@EntryDate)
    END   