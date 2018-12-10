IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Service_FetchInqDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Service_FetchInqDetails]
GO

	
-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <29/06/2016>
-- Description:	<Fetch existing servive and customer details>
-- Modified by: Kritika Choudhary on 5th July 2016, added parameter @ContactNum and condition to check whether any active inquiry exists for that number
-- Modified by : Kritika Choudhary on 8th July 2016, added TC_LeadStageId, Make, Model and Version in select query
-- Modified By : Ashwini Dhamankar on July 19,2016 (Fecthed comments)
-- exec TC_Service_FetchInqDetails NULL,NULL,'MH 04 KC 7788',20466,NULL
-- Modified by : Kritika Choudhary on 27th July 2016, added ServiceDueDate and removed LastServiceDate from select query
-- Modified by : Kritika Choudhary on 28th July 2016, added LastServiceDate.
-- Modified By : Nilima More On 4th August 2016,added service due date on lead stageid condition.
-- Modified By : Tejashree Patil On 16th August 2016,Fetched IsPickUpRequested, PickUpRequestedDate, ServiceDate
-- exec [TC_Service_FetchInqDetails] null,29673,null,20466,null
-- =============================================
CREATE PROCEDURE [dbo].[TC_Service_FetchInqDetails] --null,26938,null,20466,null
	@ServiceInqId INT = NULL,
	@InquiriesLeadId INT = NULL,
	@RegNum VARCHAR(50) = NULL,
	@BranchId INT,
	@ContactNum VARCHAR(10) = NULL
AS
BEGIN
	SELECT TOP 1 SI.TC_Service_InquiriesId,SI.RegistrationNumber,VW.VersionId,VW.ModelId,VW.MakeId,SI.Kms,
	CD.Id AS CustomerId , CD.CustomerName CustomerName , CD.Mobile ,CD.AlternateNumber,CD.Email,
	SI.ServiceType,SI.ServiceDate, IL.TC_LeadStageId, VW.Make, VW.Model, VW.Version,CD.Address -- Added by : Kritika Choudhary on 8th July 2016, added TC_LeadStageId, Make, Model and Version in select query
	,SI.Comments As Comments,IL.TC_LeadStageId,
	CASE WHEN IL.TC_LeadStageId <> 3 THEN  SI.ServiceDate ELSE  TSR.ServiceDueDate END AS ServiceDueDate,SI.LastServiceDate --Modified By : Nilima More On 4th August 2016,added service due date on lead stageid condition.
	, ISNULL(SI.IsPickUpRequested,0) AS IsPickUpRequested ,SI.PickUpRequestedDate, SI.ServiceDate, SI.BookComments -- Modified By : Tejashree Patil On 16th August 2016,Fetched IsPickUpRequested, PickUpRequestedDate, ServiceDate
	FROM TC_Service_Reminder TSR WITH(NOLOCK)
	INNER JOIN TC_Service_Inquiries SI WITH(NOLOCK) ON  TSR.RegistrationNumber = SI.RegistrationNumber
	INNER JOIN TC_InquiriesLead IL WITH(NOLOCK) ON SI.TC_InquiriesLeadId = IL.TC_InquiriesLeadId
	INNER JOIN TC_CustomerDetails CD WITH(NOLOCK) ON CD.Id = IL.TC_CustomerId
	INNER JOIN vwAllMMV VW WITH(NOLOCK) ON VW.VersionId = SI.VersionId
	WHERE ((@RegNum IS NULL OR SI.RegistrationNumber = @RegNum) AND               --Modified by: Kritika Choudhary on 5th july 2016,changed OR condition to AND
	(@ServiceInqId IS NULL OR SI.TC_Service_InquiriesId = @ServiceInqId) AND       --Modified by: Kritika Choudhary on 5th july 2016,changed OR condition to AND
	(@InquiriesLeadId IS NULL OR SI.TC_InquiriesLeadId = @InquiriesLeadId) AND
	(@ContactNum IS NULL OR (CD.Mobile = @ContactNum AND IL.TC_LeadStageId <> 3))) --Added by: Kritika Choudhary on 5th july 2016
	AND IL.BranchId = @BranchId AND VW.ApplicationId = 1
	ORDER BY SI.TC_Service_InquiriesId DESC
END
