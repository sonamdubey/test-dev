IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_InquireisFolloup_RequestFor]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_InquireisFolloup_RequestFor]
GO

	
-- =============================================
-- Author     :	Umesh Ojha
-- Create date: 7/11/2011
-- Description:	Return Inquiries Followup with request type for a particular lead with comma sepearated request type
-- =============================================
CREATE PROCEDURE [dbo].[NCD_InquireisFolloup_RequestFor]
	-- Add the parameters for the stored procedure here
	@inqFollowupId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @listStr VARCHAR(MAX)

	SELECT @listStr = COALESCE ( COALESCE(@listStr+' , ' ,'') + 
	case when convert(varchar(100),RequestType)= '1' then 'Price Quote'
	when convert(varchar(100),RequestType)='2' then 'Test Drive'
	when convert(varchar(100),RequestType)='4' then 'Loan Inquiry' end , @listStr)
	FROM NCD_LeadRequest where Inq_FollowupId =@inqFollowupId
	
	select @listStr as RequestFor
	
END

