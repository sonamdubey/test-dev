IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_FetchDCTasklistData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_FetchDCTasklistData]
GO

	-- =============================================
-- Author      : Chetan Navin
-- Create date : 20 May 2014
-- Description : To get dc tasklist data
-- Module      : NewCRM
-- Modifier    : Chetan Navin - 25 Aug 2014 (Added dealer mobile number field to fetch )
-- =============================================
CREATE PROCEDURE [dbo].[CRM_FetchDCTasklistData] 
	@CallerId BIGINT 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	SELECT ISNULL(DAC.CallId, -1) CallId, ISNULL(DAC.CallType, -1) CallType, TD.DealerId, ND.Name AS DealerName, CT.Name AS City, DAC.Subject,
    TD.TotalRequest, TD.PQPending, TD.TDPending, TD.BookingPending, TD.ActiveFBPending, TD.InvoicePending, TD.ApprovalPending, DAC.ScheduledOn,ND.Mobile AS Mobile
    FROM CRM_DCTaskListData TD 
    JOIN NCS_Dealers ND ON TD.DealerId = ND.ID
    JOIN Cities CT ON ND.CityId = CT.ID
    JOIN CRM_ADM_DCDealers CAD ON TD.DealerId = CAD.DealerId
    LEFT JOIN CRM_DCActiveCalls DAC ON DAC.DealerId = TD.DealerId
    WHERE CAD.DCID = @CallerId
    ORDER BY TotalRequest DESC

END
