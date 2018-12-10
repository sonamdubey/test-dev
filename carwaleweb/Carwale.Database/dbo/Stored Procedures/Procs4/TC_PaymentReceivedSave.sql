IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_PaymentReceivedSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_PaymentReceivedSave]
GO

	-- =============================================
-- Author:		Surendra Chouksey
-- Create date: 9 Nov,2011
-- Description:	Added extra parameter in Payment received Update
-- =============================================
-- Author:		Surendra Chouksey
-- Create date: 7 Sept,2011
-- Description:	Here Paymeny can be add,upadte for particular booking
-- =============================================
CREATE PROCEDURE [dbo].[TC_PaymentReceivedSave]
(
@StockId INT,
@BranchId INT,
@TC_PaymentReceived_Id BIGINT=NULL ,
@TC_PaymentOptions_Id TINYINT ,
@UserId INT,
@AmountReceived DECIMAL(18,2),
@PaymentType TINYINT,
@PayDate Date,
@ChequeNo VARCHAR(10)=NULL,
@ChequeDate DATE =NULL,
@BankName VARCHAR(50)=NULL,
@TC_PaymentReceived_Id_Output BIGINT OUTPUT,
@TotalPayRec DECIMAL(18,2) OUTPUT
)
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;
	IF EXISTS(SELECT Id FROM TC_Stock WHERE Id=@StockId AND BranchId=@BranchId AND StatusId=1)
	BEGIN
		DECLARE @TC_CarBooking_Id INT
		DECLARE @SellingAmount DECIMAL(18,2) -- to Store Actual Amount for the stock
		DECLARE @TotalRecAmount DECIMAL(18,2) -- to store total payment recieved till before this paymnt
		DECLARE @PayRecIdentity BIGINT -- to store total payment recieved till before this payment	
		
		SELECT @SellingAmount=NetPayment,@TC_CarBooking_Id=TC_CarBookingId FROM TC_CarBooking WHERE StockId=@StockId AND IsCanceled=0
		IF(@TC_PaymentReceived_Id IS NOT NULL)
		BEGIN
			SELECT @TotalRecAmount=SUM(AmountReceived) FROM TC_PaymentReceived WHERE TC_CarBooking_Id=@TC_CarBooking_Id AND IsCleared=1 AND IsActive=1 AND TC_PaymentReceived_Id<>@TC_PaymentReceived_Id
		END
		ELSE
		BEGIN
			SELECT @TotalRecAmount=SUM(AmountReceived) FROM TC_PaymentReceived WHERE TC_CarBooking_Id=@TC_CarBooking_Id AND IsCleared=1 AND IsActive=1
		END		
		
		IF (@SellingAmount IS NULL) -- This means car is not yet booked
		BEGIN
			RETURN -1 
		END
		
		IF (@TotalRecAmount IS NULL) -- If Payment is not yet received
		BEGIN
			SET @TotalRecAmount=0
		END	
		
		IF((@TotalRecAmount + @AmountReceived)>@SellingAmount) -- This means amount is exceeding the Net Amount
		BEGIN
			RETURN -2
		END
		
		SET @TotalPayRec= @TotalRecAmount + @AmountReceived
		
		IF(@TC_PaymentReceived_Id IS NOT NULL) -- This means Payment need to Update
			BEGIN
				-- following procedure will update Payment
				EXEC TC_PaymentReceivedUpdate @TC_PaymentReceived_Id,@TC_PaymentOptions_Id,@UserId,@AmountReceived,@PayDate,@ChequeNo,@ChequeDate,@BankName,@UserId			
				RETURN 0
			END
		ELSE -- This means Payment need to Add
			BEGIN
				-- following procedure will Add Payment
				EXEC TC_PaymentReceivedAdd @TC_CarBooking_Id,@TC_PaymentOptions_Id,@UserId,@AmountReceived,@PaymentType,@PayDate,@ChequeNo,@ChequeDate,@BankName,@PayRecIdentity OUTPUT 
				SET @TC_PaymentReceived_Id_Output=@PayRecIdentity
				RETURN 1
			END   
	END
	ELSE
	BEGIN
		RETURN -3 -- means user is trying to update/add wrong payament
	END	
END


/****** Object:  StoredProcedure [dbo].[TC_PaymentReceivedUpdate]    Script Date: 11/10/2011 18:07:42 ******/
SET ANSI_NULLS ON
