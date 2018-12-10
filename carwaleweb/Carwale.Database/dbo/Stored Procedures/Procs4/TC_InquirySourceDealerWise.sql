IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InquirySourceDealerWise]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InquirySourceDealerWise]
GO

	-- Author:		Umesh Ojha
-- Create date: 27 June 2013
-- Description:	This procedure is used to get Inquiry Source with Dealer wise for NCD
-- exec TC_InquirySourceDealerWise 1028
-- Modified By : Umesh Ojha for fetching orderby column also for adding optgroup in dropdown
-- =============================================
CREATE PROCEDURE [dbo].[TC_InquirySourceDealerWise]
@DealerId BIGINT
AS
BEGIN	
SET NOCOUNT ON;
SELECT	Id, Source,OrderBY 
		FROM	TC_InquirySource 
		WHERE (MakeId IS NULL OR MakeId IN
		(SELECT MakeId FROM TC_DealerMakes WHERE DealerId = @DealerId))
		AND IsActive=1 AND IsVisible=1
		ORDER by OrderBY
END
