IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_LeadRequest_Insert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_LeadRequest_Insert]
GO

	-- =============================================
-- Author:		<Umesh Ojha>
-- Create date: <27/10/2011>
-- Description:	<Inserting Data for Inquiries for more than one type of request>
-- =============================================
CREATE PROCEDURE [dbo].[NCD_LeadRequest_Insert] 
	-- Add the parameters for the stored procedure here
	@InquiryId int,
	@Inq_FollowupId int,
	@RequestType int,
	@ret_val Int = null OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here.If this lead already assigned for particular request it will not insert. 
	
	if not exists (select Id from NCD_LeadRequest where InquiryId=@InquiryId and RequestType=@RequestType)
	begin
		Insert Into NCD_LeadRequest
		(InquiryId,Inq_FollowupId,RequestType,FollowupDate)
		Values(@InquiryId,@Inq_FollowupId,@RequestType,GETDATE())
	end
	
END