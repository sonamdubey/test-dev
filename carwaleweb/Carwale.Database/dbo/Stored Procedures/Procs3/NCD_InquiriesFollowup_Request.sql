IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_InquiriesFollowup_Request]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_InquiriesFollowup_Request]
GO

	

-- =============================================
-- Author     :	Umesh Ojha
-- Create date: 2/11/2011
-- Description:	Return Inquiries Followup with request type for a particular lead with comma sepearated request type
-- =============================================

CREATE PROCEDURE [dbo].[NCD_InquiriesFollowup_Request]
	-- Add the parameters for the stored procedure here
	@inquiryId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;   		
		
		select NIF.Id AS InquiryFollowUpId ,NIF.Comment,NIF.NextCallTime,NL.StatusName,LOWER(NL.StatusName) AS StatusClass,	
		isnull(NIF.TestDriveDate,CONVERT(datetime,'01/01/1800',103)) as TestDriveDate,NU.UserName from NCD_InquiryFollowup NIF INNER JOIN NCD_LeadStatus NL ON NL.Id=NIF.LeadStatusId 
		inner join NCD_Users NU on NU.Id=NIF.AssignedExecutive
		where NIF.InquiryId=@inquiryId Order by NextCallTime desc
		
		
		DECLARE @listStr VARCHAR(MAX)

		Select Id,InquiryId,Inq_FollowupId,case when convert(varchar(100),RequestType)= '1' then 'Price Quote'
		when convert(varchar(100),RequestType)='2' then 'Test Drive'
		when convert(varchar(100),RequestType)='4' then 'Loan Inquiry' end as RequestType
		from NCD_LeadRequest where InquiryId=@inquiryId
		
END

