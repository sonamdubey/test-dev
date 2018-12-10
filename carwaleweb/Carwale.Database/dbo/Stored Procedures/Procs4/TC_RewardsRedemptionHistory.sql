IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_RewardsRedemptionHistory]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_RewardsRedemptionHistory]
GO

	-- =============================================
-- Author:		<Vivek,,Gupta>
-- Create date: <Create Date,14-05-2015,>
-- Description:	<Description,Retrieve Points Redemption History,>
-- Modified By Vivek Gupta on 06-11-2015, added @Userid parameter to calculater points user wise(not dealer wise)
-- Modified by : Kritika Choudhary on 6th Jan 2015, added RequestStatus
-- Modified by : Kritika Choudhary on 10th may 2016, added RequestStatus RedeemedPoints > 0 inside where clause
-- =============================================
CREATE PROCEDURE [dbo].[TC_RewardsRedemptionHistory]
@DealerId INT,
@FromDate DATETIME,
@ToDate DATETIME,
@UserId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT CONVERT(VARCHAR, DATENAME(day, RedeemDate)) AS Day,
		   CONVERT(VARCHAR, DATENAME(Month, RedeemDate)) + ' ' + CONVERT(VARCHAR, DATENAME(year, RedeemDate)) AS MonthYear,
		   RedeemedPoints,
		   Description,
		   EmailSentOn,CASE WHEN RequestType=1 THEN 'Requested' WHEN RequestType=2 THEN 'Accepted' ELSE 'Delivered' END AS RequestStatus
	FROM TC_RedeemedPoints WITH(NOLOCK)
	WHERE DealerId = @DealerId AND UserId = @UserId
	AND CONVERT(DATE,RedeemDate) BETWEEN CONVERT(DATE,@FromDate) AND CONVERT(DATE,@ToDate) AND RedeemedPoints > 0 -- Modified by : Kritika Choudhary on 10th may 2016, added RequestStatus RedeemedPoints > 0 inside where clause
   
END
