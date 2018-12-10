IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UpdateMFCDealerPerDayLeadCnt]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UpdateMFCDealerPerDayLeadCnt]
GO

	-- =============================================
-- Author:		<Khushaboo patil>
-- Create date: <12/03/2015>
-- Description:	<Update lead count in TC_MFCDealers>
-- =============================================
CREATE PROCEDURE [dbo].[TC_UpdateMFCDealerPerDayLeadCnt]
	@LeadCnt [dbo].[TC_DealerLeadCnt] READONLY	
AS
BEGIN
	UPDATE
		TC_MFCDealers 
	SET
		LeadCntPerDay = L.LeadCnt
	FROM
		@LeadCnt AS L
	INNER JOIN
		TC_MFCDealers MD WITH(NOLOCK) ON L.DealerId = MD.DealerId 
END
