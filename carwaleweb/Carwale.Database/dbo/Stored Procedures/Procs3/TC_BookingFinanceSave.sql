IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_BookingFinanceSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_BookingFinanceSave]
GO

	
-- =============================================
-- Author:		Surendra
-- Create date: 17 Oct 2011
-- Description:	This Procedure is used to Insert or Update Booking Finance details
-- =============================================
CREATE PROCEDURE [dbo].[TC_BookingFinanceSave]
(
@DealerID INT,
@StockId INT,
@TC_BookingFinance_Id INT =NULL,
@TC_DealerBank_Id INT,
@IsDocumentReceived BIT,
@IsCaseLoggedIn BIT,
@IsLoanApproved BIT,
@LoanApprovalDate DATE=NULL,
@AmountApproved INT,
@LoanTerms TINYINT,
@IsDisbursed BIT,
@DisbursedDate DATE =NULL,
@TC_BookingFinance_Id_Output INT OUTPUT
)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF EXISTS(SELECT Id FROM TC_Stock WHERE Id=@StockId AND BranchId=@DealerID)
	BEGIN
		DECLARE @TC_CarBooking_Id INT
		SELECT @TC_CarBooking_Id=TC_CarBookingId FROM TC_CarBooking WHERE StockId=@StockId AND IsCanceled=0
		IF(@TC_BookingFinance_Id IS NULL) -- means need to Insert
			BEGIN
				INSERT INTO TC_BookingFinance(TC_CarBooking_Id,TC_DealerBank_Id,IsDocumentReceived,IsCaseLoggedIn,
				IsLoanApproved,LoanApprovalDate,AmountApproved,LoanTerms,IsDisbursed,DisbursedDate)
				VALUES(@TC_CarBooking_Id,@TC_DealerBank_Id,@IsDocumentReceived,@IsCaseLoggedIn,
				@IsLoanApproved,@LoanApprovalDate,@AmountApproved,@LoanTerms,@IsDisbursed,@DisbursedDate)
				SET @TC_BookingFinance_Id_Output=SCOPE_IDENTITY()
				RETURN 0
			END
		ELSE -- Need to update
			BEGIN
				UPDATE TC_BookingFinance SET TC_DealerBank_Id=@TC_DealerBank_Id,IsDocumentReceived=@IsDocumentReceived,
				IsCaseLoggedIn=@IsCaseLoggedIn,IsLoanApproved=@IsLoanApproved,LoanApprovalDate=@LoanApprovalDate,
				AmountApproved=@AmountApproved,LoanTerms=@LoanTerms,IsDisbursed=@IsDisbursed,DisbursedDate=@DisbursedDate
				WHERE TC_BookingFinance_Id=@TC_BookingFinance_Id AND TC_CarBooking_Id=@TC_CarBooking_Id
				RETURN -1
			END	
	END
	ELSE
	BEGIN
		RETURN -2
	END 
	
END

