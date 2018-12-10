IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetINQStatusId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetINQStatusId]
GO
	-- =============================================
-- Author:		Yuga Hatolkar
-- Create date: 14th Dec, 2014
-- Description:	Get Inquiry Status Id
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetINQStatusId]
	-- Add the parameters for the stored procedure here

	@LeadId				BIGINT = NULL
AS
BEGIN
		
	SELECT TIL.TC_LeadInquiryTypeId AS TC_InquiryTypeId FROM TC_Lead TL WITH(NOLOCK)
	INNER JOIN TC_InquiriesLead TIL WITH(NOLOCK) ON TL.TC_LeadId = TIL.TC_LeadId
	WHERE TL.TC_LeadId=@LeadId

END



--------------------------------------------------------------------------------------------------------------------------------------




