IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FetchLeadDispositions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_FetchLeadDispositions]
GO

	
-- =============================================
-- Author:		<Author,,Vivek Gupta>
-- Create date: <Create Date,28-12-2015,>
-- Description:	<Description,Fetching leaddisposition,>
-- =============================================
CREATE PROCEDURE [dbo].[TC_FetchLeadDispositions]
	@BranchId INT,
	@TC_LeadInquiryTypeId TINYINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @ApplicationId TINYINT
	SET @ApplicationId = (SELECT ApplicationId FROm Dealers WITH(NOLOCK) WHERE ID = @BranchId)

    SELECT TC_LeadDispositionId, Name AS LeadDisposition 
	FROM TC_LeadDisposition WITH(NOLOCK) 
	WHERE ISNULL(TC_LeadInquiryTypeId,0) = @TC_LeadInquiryTypeId 
	AND 
	    (
			(ISNULL(IsVisibleCW,0) = 1 AND @ApplicationId = 1) 
		OR  (ISNULL(IsVisibleBW,0) = 1 AND @ApplicationId = 2)

		)
END

