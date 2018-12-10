IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetSyndicationWebsites]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetSyndicationWebsites]
GO

	-- =============================================
-- Author: Tejashree Patil
-- Create date: 10 Aug 2012
-- Description: Get list of Syndication websites
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetSyndicationWebsites]
@BranchId BIGINT
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;

SELECT S.TC_SyndicationWebsiteId,S.WebsiteName, D.TC_SyndicationWebsiteId AS DealerSyndicationWebsiteId
FROM TC_SyndicationWebsite S WITH (NOLOCK)
LEFT JOIN TC_SyndicationDealer D WITH(NOLOCK) ON S.TC_SyndicationWebsiteId=D.TC_SyndicationWebsiteId
AND D.BranchId=@BranchId AND D.IsActive=1
WHERE S.IsActive=1

END
