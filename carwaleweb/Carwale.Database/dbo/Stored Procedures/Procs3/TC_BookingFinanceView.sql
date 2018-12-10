IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_BookingFinanceView]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_BookingFinanceView]
GO

	
-- =============================================
-- Author:		Surendra Chouksey	
-- Create date: 14 Oct 2011
-- Description:	Getting finance details for booking
-- =============================================
CREATE PROCEDURE [dbo].[TC_BookingFinanceView]
(
@StockId INT,
@DealerId INT
)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		
	SELECT TC_BookingFinance_Id, TC_DealerBank_Id ,IsDocumentReceived,IsCaseLoggedIn,IsLoanApproved,LoanApprovalDate,
	AmountApproved,LoanTerms,IsDisbursed,DisbursedDate
	FROM TC_BookingFinance F
	INNER JOIN TC_CarBooking B ON F.TC_CarBooking_Id=B.TC_CarBookingId
	WHERE b.StockId=@StockId AND F.IsActive=1 AND B.IsCanceled=0
	
	SELECT TC_DealerBank_Id	,BankName FROM TC_DealerBank WHERE DealerId=@DealerId AND IsActive=1
   
END

