IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDealerIdByTCDealerId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDealerIdByTCDealerId]
GO

	CREATE PROCEDURE [dbo].[GetDealerIdByTCDealerId]
@TcDealerId	INT
AS
BEGIN
SELECT Id FROM Dealer_NewCar where TcDealerId = @TcDealerId
END
