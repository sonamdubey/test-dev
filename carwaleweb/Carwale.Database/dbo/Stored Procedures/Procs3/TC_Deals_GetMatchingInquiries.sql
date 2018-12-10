IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_GetMatchingInquiries]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_GetMatchingInquiries]
GO

	-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date: Jan 22,2016
-- Description:	To get matching inquiries of stock
-- EXEC TC_Deals_GetMatchingInquiries 74,5,null
-- Modified By : Ruchira Patil on 14 April 2016 (to fetch matching inquiries in case of CW advantage Masking Number Inquiry)
-- Modified by : Ashwini Dhamankar on May 12,2016 (Added sourceId 147 and 148)
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_GetMatchingInquiries] 
@TC_Deals_StockId INT,
@BranchId BIGINT,
@TC_Deals_StockVINId INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
		WITH CTE0 AS (
    SELECT		DISTINCT NCI.TC_NewCarInquiriesId,(CMK.Name + ' ' + CM.Name + ' ' + CV.Name) AS Car,C.CustomerName, AC.LastCallComment AS RecentComments,AC.ScheduledOn,
				NCI.CreatedOn AS InquiryDate,
				CASE	WHEN ISNULL(L.TC_LeadStageId,0) <> 3 
						THEN 'OPEN' ELSE 'CLOSED' 
				END AS Status,
				NCI.TC_Deals_StockId,NCI.TC_InquirySourceId,--,DSV.TC_DealsStockVINId
				ROW_NUMBER() OVER (PARTITION BY NCI.TC_NewCarInquiriesId ORDER By AC.ScheduledOn DESC) as ROW
	FROM		TC_NewCarInquiries NCI WITH(NOLOCK) 
	INNER JOIN	TC_InquiriesLead IL WITH(NOLOCK) ON IL.TC_InquiriesLeadId = NCI.TC_InquiriesLeadId
	INNER JOIN	TC_Lead L WITH(NOLOCK) ON L.TC_LeadId = IL.TC_LeadId
	LEFT JOIN	TC_ActiveCalls AC WITH(NOLOCK) ON AC.TC_LeadId = L.TC_LeadId    --if lead get closed entry get delete from active calls thats why left join
	INNER JOIN	TC_CustomerDetails C WITH(NOLOCK) ON C.Id = L.TC_CustomerId
	INNER JOIN	TC_Deals_Stock DS WITH(NOLOCK) ON DS.Id = NCI.TC_Deals_StockId
	--INNER JOIN	TC_Deals_StockVIN DSV WITH(NOLOCK) ON DSV.TC_Deals_StockId = Ds.Id
	INNER JOIN	CarVersions CV WITH (NOLOCK) ON CV.Id = DS.CarVersionId
	INNER JOIN	CarModels CM WITH (NOLOCK) ON CM.ID = CV.CarModelId
	INNER JOIN	CarMakes CMK WITH (NOLOCK) ON CMK.ID = CM.CarMakeId 
	WHERE		NCI.TC_Deals_StockId = @TC_Deals_StockId --AND ISNULL(NCI.TC_DealsStockVINId,0) <> @TC_Deals_StockVINId
				AND DS.BranchId = @BranchId AND (NCI.TC_InquirySourceId IN (134,140,147,148) OR (NCI.TC_InquirySourceId = 146 AND NCI.TC_Deals_StockId IS NOT NULL))-- CW advantage Masking Number Inquiry
				)

	SELECT * FROM CTE0 WHERE ROW = 1
END
------------------------------------------------------------------------------------------------------------------------------------------------------------------------

