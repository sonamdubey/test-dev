IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_PaymentReceivedDelete]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_PaymentReceivedDelete]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- =============================================
-- Author:		SURENDRA CHOUKSEY
-- Create date: 21st Sept,2011
-- Description:	Updating(delete for client) payment received for particular booking
-- =============================================
CREATE PROCEDURE TC_PaymentReceivedDelete
(
	@DealerId INT,
	@TC_PaymentReceived_Id INT
)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @CheckCount smallint
	
	
	-- IN following code we are checking that updating @TC_PaymentReceived_Id is exist for same logged in user or not
	SELECT @CheckCount=COUNT(PR.TC_PaymentReceived_Id) FROM TC_PaymentReceived PR
	INNER JOIN TC_CarBooking CB ON PR.TC_CarBooking_Id=CB.TC_CarBookingId
	INNER JOIN TC_Stock ST ON CB.StockId=ST.Id
	WHERE ST.BranchId=@DealerId AND PR.TC_PaymentReceived_Id=@TC_PaymentReceived_Id AND PR.IsActive=1
	
	IF(@CheckCount=0) -- Unathorised(User is trying to update/delete other's payment
	BEGIN
		RETURN -1
	END
	
	
	BEGIN
		UPDATE TC_PaymentReceived SET IsActive=0 WHERE TC_PaymentReceived_Id=@TC_PaymentReceived_Id
		RETURN 0
	END
END

