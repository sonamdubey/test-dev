IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_PaymentRaceivedUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_PaymentRaceivedUpdate]
GO

	-- =============================================
-- Author:		Surendra Chouksey
-- Create date: 7 Sept,2011
-- Description:	This is the inner procedure of TC_PaymentRaceivedSave to update Payment Recieved
-- =============================================
CREATE PROCEDURE [dbo].[TC_PaymentRaceivedUpdate]
(
@TC_PaymentReceived_Id BIGINT,
@TC_PaymentOptions_Id TINYINT,
@UserId INT,
@AmountReceived DECIMAL,
@PayDate Date,
@ChequeNo VARCHAR(10),
@ChequeDate DATE,
@BankName VARCHAR(50)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	UPDATE TC_PaymentReceived SET TC_PaymentOptions_Id=@TC_PaymentOptions_Id,UserId=@UserId,
	AmountReceived=@AmountReceived,PayDate=@PayDate,ChequeNo=@ChequeNo,ChequeDate=@ChequeDate,
	BankName=@BankName	WHERE TC_PaymentReceived_Id=@TC_PaymentReceived_Id
   
END