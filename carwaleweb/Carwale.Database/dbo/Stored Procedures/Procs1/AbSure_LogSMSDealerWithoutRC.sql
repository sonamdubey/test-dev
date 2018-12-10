IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_LogSMSDealerWithoutRC]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_LogSMSDealerWithoutRC]
GO

	-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date: July 31,2015
-- Description:	Save log of sended SMS
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_LogSMSDealerWithoutRC] 
	@CarId BIGINT
AS
BEGIN
    INSERT INTO AbSure_DealerWithoutRCSMSLog
				(CarId,DealerId,MobileNo,StockId,SendedOn)

	SELECT @CarId,D.ID,D.MobileNo,ACD.StockId,GETDATE()
	FROM AbSure_CarDetails ACD WITH(NOLOCK)
	INNER JOIN Dealers D WITH(NOLOCK) ON D.ID = ACD.DealerId 
	WHERE ACD.Id = @CarId

	SELECT SCOPE_IDENTITY()

	Delete from AbSure_CarsWithoutRCImage where AbSure_CarDetailsId = @CarId

END

