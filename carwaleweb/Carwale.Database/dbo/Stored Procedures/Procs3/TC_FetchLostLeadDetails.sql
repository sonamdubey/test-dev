IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FetchLostLeadDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_FetchLostLeadDetails]
GO
	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 11th Dec 2015
-- Description:	To fetch the customer details of lost leads
-- EXEC TC_FetchLostLeadDetails 5,null,1,'11/15/2015','12/16/2015',null,73
-- EXEC [dbo].[TC_FetchLostLeadDetails] 10273,355,2, '2016-01-01 18:48:27.817' , '2016-01-09 18:48:27.817', NULL, 95
-- Modified by VIvek Gupta on 13-01-2016, added @AppliactionId
-- =============================================
CREATE PROCEDURE [dbo].[TC_FetchLostLeadDetails]
@BranchId	INT,
@ModelId	INT = NULL,
@Type		INT,
@FromDate	DATETIME = NULL,
@ToDate		DATETIME = NULL,
@ReportingUsersList VARCHAR(MAX) = '-1',
@LeadDispositionID INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @ApplicationId TINYINT = 1
	SET @ApplicationId = (SELECT ISNULL(ApplicationId,1) FROM Dealers WITH(NOLOCK) WHERE ID = @BranchId)
	--PRINT @ApplicationId

	SELECT distinct (ISNULL(TCD.Salutation, '') + ' ' + TCD.CustomerName + ' ' + ISNULL(TCD.LastName, '')) AS [CustomerName]
	,TIL.CarDetails AS [InterestedIn]
	,TCD.id AS [CustomerId]
	,TIL.TC_LeadId AS LeadId
	,TIL.TC_UserId AS UserId
	,TNI.TC_LeadDispositionId
	--,TLD.Name AS Reason,TLD.TC_LeadDispositionId DispositionId,TIL.TC_InquiriesLeadId
	FROM TC_NewCarInquiries TNI WITH(NOLOCK)
	INNER JOIN  TC_InquiriesLead TIL WITH(NOLOCK) ON TIL.TC_InquiriesLeadId = TNI.TC_InquiriesLeadId
	INNER JOIN TC_CustomerDetails TCD WITH(NOLOCK) ON TIL.TC_CustomerId = TCD.Id
	INNER JOIN TC_LeadDisposition TLD WITH(NOLOCK) ON TLD.TC_LeadDispositionId = TNI.TC_LeadDispositionId and TNI.TC_LeadDispositionId<>4
	INNER JOIN vwAllMMV VW WITH(NOLOCK) ON  VW.VersionId = TNI.VersionId AND vw.ApplicationId = @ApplicationId
	WHERE TIL.TC_LeadInquiryTypeId = 3 AND TIL.BranchId = @BranchId 
	AND ((@Type = 2 AND VW.ModelId = @ModelId) OR (@Type=1)) --@TYPE = 1 --to get reasons on all models for which Dealer works  @TYPE = 2 --to get reasons on model that user selected for which dealer works
	AND (TIL.TC_UserId IN (SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)) OR @ReportingUsersList IS NULL)
	AND TIL.CreatedDate BETWEEN @FromDate AND @ToDate 
	--AND TIL.TC_LeadStageId=3 --closed leads
	AND TLD.TC_LeadDispositionID=@LeadDispositionID
END












