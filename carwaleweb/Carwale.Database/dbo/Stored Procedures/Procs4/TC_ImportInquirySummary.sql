IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ImportInquirySummary]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ImportInquirySummary]
GO

	
-- =============================================
-- Author:		Umesh
-- Create date: 19 Apr 2013
-- Description:	Select import inquiry summary after importing data from excel.
-- =============================================
CREATE PROCEDURE [dbo].[TC_ImportInquirySummary]
@InquiryType TINYINT

AS
BEGIN
	IF (@InquiryType = 1) -- buyer inquiry
	BEGIN
		select
			COUNT(case when IsNew=1 then 1 end) AS [NewCount],
			COUNT(case when IsNew=0 then 1 end ) AS [OldCount]
		from TC_ImportBuyerInquiries
		where CONVERT(date,entrydate)= CONVERT(date,GETDATE())
	END
	ELSE IF (@InquiryType = 2) -- seller inquiry
	BEGIN
		select
			COUNT(case when IsNew=1 then 1 end) AS [NewCount],
			COUNT(case when IsNew=0 then 1 end ) AS [OldCount]
		from TC_ImportSellerInquiries
		where CONVERT(date,entrydate)= CONVERT(date,GETDATE())
	END
	ELSE
	BEGIN
		select
			COUNT(case when IsNew=1 then 1 end) AS [NewCount],
			COUNT(case when IsNew=0 then 1 end ) AS [OldCount]
		from TC_ExcelInquiries
		where CONVERT(date,entrydate)= CONVERT(date,GETDATE())
	END
END
