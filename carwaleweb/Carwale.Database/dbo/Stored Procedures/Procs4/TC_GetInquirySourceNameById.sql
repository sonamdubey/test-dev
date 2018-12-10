IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetInquirySourceNameById]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetInquirySourceNameById]
GO

	-- =============================================
-- Author:		Suresh Prajapati
-- Create date: 17th June, 2016
-- Description:	Get Inquiry Source Name by Id
-- EXEC TC_GetInquirySourceNameById 1
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetInquirySourceNameById] @InquirySourceId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Source]
	FROM TC_InquirySource
	WHERE Id = @InquirySourceId
END
