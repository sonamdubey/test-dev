IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_PushMixMatchLeadToMFCDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_PushMixMatchLeadToMFCDealers]
GO

	-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <12/2/2015>
-- Description:	<push lead to TC_MFCDealers  >
-- =============================================
CREATE PROCEDURE [dbo].[TC_PushMixMatchLeadToMFCDealers]
	@SendLeadID		VARCHAR(MAX),
	@RemoveLeadId	VARCHAR(MAX)
AS
BEGIN
	UPDATE TC_MFCDealers SET SendMixMatchLead = 0 WHERE DealerId
	IN(SELECT LISTMEMBER FROM fnSplitCSV(@RemoveLeadId))
	
	UPDATE TC_MFCDealers SET SendMixMatchLead = 1 WHERE DealerId
	IN(SELECT LISTMEMBER FROM fnSplitCSV(@SendLeadID))
END
