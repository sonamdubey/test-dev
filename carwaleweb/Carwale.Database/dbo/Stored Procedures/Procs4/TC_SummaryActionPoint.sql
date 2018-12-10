IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SummaryActionPoint]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SummaryActionPoint]
GO

	-- Modifier	:	Sachin Bharti(21st March 2013)
-- Purpose	:	To add query for Pending test drive leads and apply constraints
--				in appointment scheduled for the day				
-- Modified By: Nilesh Utture on 19th June, 2013 Data is only populated based on the users(@ReportingUsersList) who report to the logged in user
-- Modified By: Nilesh Utture on 06th July, 2013 Added Condition NCI.TDStatus <> 27 to pending test drives Query
-- Modified By: Nilesh Utture on 18th July, 2013 Changed size 0f @ReportingUsersList to MAX	
-- Modified By: Nilesh Utture on 31st July, 2013 Changed converted DATETIME to DATE Format
-- Modified By : Tejashree Patil on 28 Aug 2013, Fetched record for Pending FollowUp.
-- Modified By Vivek Gupta on 15-04-2015, replaced CRM.vwMMV with vwAllMMV and put condition of applicationid
-- TC_SummaryActionPoint -1,-1,3587,-1,'4686,4674'
--======================================================================================================
CREATE PROCEDURE [dbo].[TC_SummaryActionPoint]
@ModelId  NUMERIC (18,0),
@Versionid NUMERIC (18,0),
@BranchId  NUMERIC (18,0),
@fuelType  INT,
@ReportingUsersList VARCHAR (MAX) = '-1' -- '-1': No user is reporting to logged in user, -- Modified By: Nilesh Utture on 18th July, 2013 Changed size 0f @ReportingUsersList to MAX	
										 --	NULL: in case if logged in user is Admin, 
										 -- Else comma separated user List
AS 
BEGIN

	DECLARE @ApplicationId SMALLINT

	SELECT @ApplicationId = ApplicationId FROM Dealers WITH(NOLOCK) WHERE Id = @BranchId
	--td scheduled for today
	SELECT COUNT(DISTINCT NCI.TC_NewCarInquiriesId) AS TDScheduled
	FROM TC_NewCarInquiries NCI 
		INNER JOIN TC_InquiriesLead IL WITH(NOLOCK) ON IL.TC_InquiriesLeadId = NCI.TC_InquiriesLeadId
		INNER JOIN vwAllMMV V ON NCI.VersionId=V.VersionId AND V.ApplicationId = @ApplicationId
	WHERE IL.TC_LeadInquiryTypeId=3 
		AND IL.BranchId = @BranchId 
		AND (IL.TC_UserId IN	(SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)) OR @ReportingUsersList IS NULL) -- Modified By: Nilesh Utture on 19th June, 2013
		AND NCI.TDStatus NOT IN (27,28) 
		AND ( @ModelId IS NULL OR V.ModelId=@ModelId) AND (@Versionid IS NULL OR V.VersionId=@Versionid) 
		AND(@fuelType IS NULL OR V.FuelType=@fuelType)
		--AND NCI.TDDate = GETDATE()
		AND CONVERT(DATE,NCI.TDDate) = CONVERT(DATE,GETDATE()) 

	-- appointment scheduled for the day
	SELECT COUNT(DISTINCT A.TC_appointmentsId) AS Appointment
	FROM  TC_InquiriesLead IL WITH(NOLOCK) 
		INNER JOIN TC_Appointments A WITH(NOLOCK) ON A.TC_LeadId = IL.TC_LeadId 
		INNER JOIN TC_NewCarInquiries NCI  WITH(NOLOCK) ON IL.TC_InquiriesLeadId = NCI.TC_InquiriesLeadId
		INNER JOIN vwAllMMV V ON NCI.VersionId=V.VersionId AND V.ApplicationId = @ApplicationId
	WHERE CONVERT(DATE,A.VisitDate) = CONVERT(DATE,GETDATE()) 
		AND IL.TC_LeadInquiryTypeId=3 
		AND IL.BranchId = @BranchId
		AND (IL.TC_UserId IN	(SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)) OR @ReportingUsersList IS NULL)-- Modified By: Nilesh Utture on 19th June, 2013
		AND ( @ModelId IS NULL OR V.ModelId=@ModelId) AND (@Versionid IS NULL OR V.VersionId=@Versionid) 
		AND(@fuelType IS NULL OR V.FuelType=@fuelType)
		
	-- count for pending test drive leads
	SELECT COUNT(DISTINCT NCI.TC_NewCarInquiriesId) AS TDScheduled
	FROM TC_NewCarInquiries NCI 
		INNER JOIN TC_InquiriesLead TIL ON TIL.TC_InquiriesLeadId = NCI.TC_InquiriesLeadId
		INNER JOIN vwAllMMV V ON NCI.VersionId=V.VersionId AND V.ApplicationId = @ApplicationId
	WHERE TIL.TC_LeadInquiryTypeId=3 AND NCI.TDStatus <> 28  AND NCI.TDStatus <> 27 -- Modified By: Nilesh Utture on 06th July, 2013
	AND CONVERT(DATE,NCI.TDDate) < CONVERT(DATE,GETDATE()) -- Modified By: Nilesh Utture on 31st July, 2013
	AND ( @ModelId IS NULL OR V.ModelId=@ModelId) AND (@Versionid IS NULL OR V.VersionId=@Versionid) 
	AND(@fuelType IS NULL OR V.FuelType=@fuelType)
	AND TIL.BranchId = @BranchId 
	AND (TIL.TC_UserId IN	(SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)) OR @ReportingUsersList IS NULL)-- Modified By: Nilesh Utture on 19th June, 2013
	
	-- Modified By : Tejashree Patil on 28 Aug 2013, Fetched records for Pending FollowUp.
	-- count for Pending FollowUps leads
	
	SELECT  COUNT(DISTINCT TIL.TC_LeadId) AS PendingFollowUp
	FROM	TC_NewCarInquiries NCI WITH(NOLOCK)
			INNER JOIN TC_InquiriesLead TIL WITH(NOLOCK) ON TIL.TC_InquiriesLeadId = NCI.TC_InquiriesLeadId
			INNER JOIN vwAllMMV V ON NCI.VersionId=V.VersionId AND V.ApplicationId = @ApplicationId
			INNER JOIN TC_ActiveCalls AS TCAC WITH (NOLOCK) ON TCAC.TC_LeadId=TIL.TC_LeadId
	WHERE	TIL.TC_LeadInquiryTypeId=3 
			AND TCAC.ScheduledOn<GETDATE()
			AND(@ModelId IS NULL OR V.ModelId=@ModelId) 
			AND (@Versionid IS NULL OR V.VersionId=@Versionid) 
			AND(@fuelType IS NULL OR V.FuelType=@fuelType)
			AND TIL.BranchId = @BranchId 
			AND (TIL.TC_UserId IN	(SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)) OR @ReportingUsersList IS NULL)
		
END



