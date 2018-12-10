IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateDealerSponsorDailyCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateDealerSponsorDailyCount]
GO

	
-- =============================================
-- Author:		Vinayak
-- Create date: 31/12/2013
-- Description:	Set the count value for Sponsored dealer
-- =============================================
CREATE PROCEDURE [dbo].[UpdateDealerSponsorDailyCount] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	UPDATE PQ_DealerSponsored SET DailyCount=0;
END
