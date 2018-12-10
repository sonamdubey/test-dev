IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Tc_InquiryCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Tc_InquiryCount]
GO

	
-- ===============================================================================================
-- Author      : Nilima More 
-- Create date : 26 November 2015.
-- Description : This sp return total Inquiry Count for each Dealer based on Inquiry Type.
-- ==================================================================================================
CREATE PROCEDURE [dbo].[Tc_InquiryCount] --2,5,'2013-02-14 12:54:42.553','2015-11-14 12:54:42.553'
	@TC_LeadInquiryTypeId TINYINT
	,@BranchId INT
	,@StartDate DATETIME = NULL
	,@EndDate DATETIME = NULL
AS
BEGIN
	IF (@TC_LeadInquiryTypeId = 1)
	BEGIN
		SELECT v.Model Model
			,COUNT(BI.TC_BuyerInquiriesId) InquiryCount
		FROM TC_BuyerInquiries BI WITH (NOLOCK)
		LEFT JOIN TC_InquiriesLead IL WITH (NOLOCK) ON BI.TC_InquiriesLeadId = IL.TC_InquiriesLeadId
		JOIN TC_Stock S WITH (NOLOCK) ON S.Id = BI.StockId
		JOIN vwAllMMV v WITH (NOLOCK) ON v.VersionId = s.VersionId
			AND ApplicationId = 1
		WHERE IL.BranchId = @BranchId
			AND IL.TC_LeadInquiryTypeId = 1
		AND 
			BI.CreatedOn BETWEEN @StartDate AND @EndDate
		GROUP BY v.Model
	END
	ELSE
		IF (@TC_LeadInquiryTypeId = 2)
		BEGIN
			SELECT v.Model
				,COUNT(SI.TC_SellerInquiriesId) InquiryCount
			FROM TC_SellerInquiries SI WITH (NOLOCK)
			LEFT JOIN TC_InquiriesLead IL WITH (NOLOCK) ON SI.TC_InquiriesLeadId = IL.TC_InquiriesLeadId
			JOIN TC_Stock S WITH (NOLOCK) ON SI.CarVersionId = S.VersionId
			JOIN vwAllMMV v WITH (NOLOCK) ON v.VersionId = s.VersionId
				AND ApplicationId = 1
			WHERE IL.BranchId = @BranchId
				AND IL.TC_LeadInquiryTypeId = 2
			AND 
				SI.CreatedOn BETWEEN @StartDate AND @EndDate
			GROUP BY v.Model
		END
		ELSE
		 IF(@TC_LeadInquiryTypeId = 3)
		 BEGIN
		 SELECT v.Model
				,COUNT(NSI.TC_NewCarInquiriesId) InquiryCount
			FROM TC_NewCarInquiries NSI WITH (NOLOCK)
			LEFT JOIN TC_InquiriesLead IL WITH (NOLOCK) ON NSI.TC_InquiriesLeadId = IL.TC_InquiriesLeadId
			JOIN TC_Stock S WITH (NOLOCK) ON NSI.VersionId = S.VersionId
			JOIN vwAllMMV v WITH (NOLOCK) ON v.VersionId = s.VersionId
				AND ApplicationId = 1
			WHERE IL.BranchId = @BranchId
				AND IL.TC_LeadInquiryTypeId = 3
			AND 
				NSI.CreatedOn BETWEEN @StartDate AND @EndDate
			GROUP BY v.Model
		 END
END
