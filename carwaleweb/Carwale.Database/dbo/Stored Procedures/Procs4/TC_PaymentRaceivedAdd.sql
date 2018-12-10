IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_PaymentRaceivedAdd]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_PaymentRaceivedAdd]
GO

	-- =============================================
-- Author:		Surendra Chouksey
-- Create date: 7 Sept,2011
-- Description:		This is the inner procedure of TC_PaymentRaceivedSave to insert Payment Recieved
-- =============================================
CREATE PROCEDURE [dbo].[TC_PaymentRaceivedAdd]
(
@TC_CarBooking_Id INT,
@TC_PaymentOptions_Id TINYINT,
@UserId INT,
@AmountReceived DECIMAL,
@PaymentType TINYINT,
@PayDate Date,
@ChequeNo VARCHAR(10),
@ChequeDate DATE,
@BankName VARCHAR(50),
@TC_PaymentReceived_Id_Output BIGINT OUTPUT
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	INSERT INTO TC_PaymentReceived(TC_CarBooking_Id,TC_PaymentOptions_Id,UserId,AmountReceived,PaymentType,PayDate,ChequeNo,ChequeDate,BankName)
	VALUES(@TC_CarBooking_Id,@TC_PaymentOptions_Id,@UserId,@AmountReceived,@PaymentType,@PayDate,@ChequeNo,@ChequeDate,@BankName)
	
	set @TC_PaymentReceived_Id_Output=SCOPE_IDENTITY()
	
END
