IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetInquirySourceByGroupSourceId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetInquirySourceByGroupSourceId]
GO

	
-- =============================================
-- Author:		Khushaboo Patil
-- Create date: 7th Oct 2016
-- Description:	Get InquirySource By Group Source Id
-- =============================================
CREATE PROCEDURE  [dbo].[TC_GetInquirySourceByGroupSourceId]
	@GroupSourceId INT
AS
BEGIN
	SELECT Id,Source AS Value
	FROM TC_InquirySource TIS WITH(NOLOCK)
	WHERE TIS.TC_InquiryGroupSourceId = @GroupSourceId AND IsActive = 1
	ORDER BY Source 
END



