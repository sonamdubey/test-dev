IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_AP_DailyChangeDealerType]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_AP_DailyChangeDealerType]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 06-Aug-2012
-- Description:	Automated SP to change the DealerType as per its Package expiry date
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_AP_DailyChangeDealerType]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
    DECLARE @CurrentDate	DATETIME
    --Set Current date to a variable
    SET @CurrentDate = GETDATE()
    
    UPDATE DCRM_SalesDealer
		SET DealerType = 
			CASE 
				WHEN DATEDIFF(DD,PackageExpiryDate,@CurrentDate) <= 0 THEN 3 -- Renewal Before Expiry Date
				WHEN DATEDIFF(DD,PackageExpiryDate,@CurrentDate) BETWEEN 1 AND 30 THEN 5 -- Expiry Within 30 Days
				WHEN DATEDIFF(DD,PackageExpiryDate,@CurrentDate) BETWEEN 31 AND 90 THEN 2 -- Expiry Days above 30 Days but Below 90 Days
				WHEN DATEDIFF(DD,PackageExpiryDate,@CurrentDate) > 90 THEN 4 -- Expiry Aboove 90Days
				ELSE 1 -- New Dealer 
			END
	WHERE LeadStatus = 1
	
END
