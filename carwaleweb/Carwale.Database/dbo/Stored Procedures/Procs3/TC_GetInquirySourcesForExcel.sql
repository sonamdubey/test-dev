IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetInquirySourcesForExcel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetInquirySourcesForExcel]
GO
	-- =============================================
-- Author:		Tejashree Patil
-- Create date: 27 May,2013
-- Description:	This returns inquiry sources for excel sheet.
-- [TC_GetInquirySourcesForExcel]
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetInquirySourcesForExcel]
	
AS
BEGIN
	SELECT	Id, Source 
	FROM	TC_InquirySource 
	WHERE	IsActive=1 AND IsVisibleForExcelSheet=1
END
