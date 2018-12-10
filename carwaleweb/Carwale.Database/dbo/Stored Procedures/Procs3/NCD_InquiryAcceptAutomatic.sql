IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_InquiryAcceptAutomatic]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_InquiryAcceptAutomatic]
GO

	
-- =============================================
-- Author:		<Vinay Kumar Prajapati>
-- Create date: <28/04/2014>
-- Description:	<Get all NCD_inquiry leads which exceed 72 hour(3 days) difference b/w EntryDate and currentDate and accept all leads automatic.>
-- Modified By : Vinay on 21-05-2014 for Getting all NCD_inquiry leads which exceed 120 hour(5 days) .>
-- =============================================
CREATE PROCEDURE [dbo].[NCD_InquiryAcceptAutomatic]
	-- Add the parameters for the stored procedure here
	@DealerId int,
	@Executive int,
    @ret_val Int = null OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @NumberRecords AS INT
	DECLARE @RowCount AS INT

	DECLARE @TempInquiry Table(	RowID INT IDENTITY(1, 1),InqId INT,CustId INT)	

	--all Ncd inquiry leads which have more than 120 hour difference b/w EntryDate and current date.
	INSERT INTO @TempInquiry
	SELECT NI.Id AS InqId,NC.Id AS CustId FROM NCD_Inquiries AS NI WITH(NOLOCK)
			INNER JOIN  NCD_Customers AS NC WITH(NOLOCK) ON  NI.CustomerId=NC.Id 
	 WHERE  NI.DealerId=@DealerId AND  NI.RequestType IN (1,2,4) 
			AND NI.IsActive=1 and NI.IsActionTaken=0  
			AND DATEDIFF(minute,NI.EntryDate,GETDATE()) >=7200  --120 hour(5 days)

	SET @NumberRecords = @@ROWCOUNT
	SET @RowCount = 1

	DECLARE @CustomerId	INT,
			@InquiryId INT,
			@NextCallTime DATETIME = GETDATE()+1,
			@LeadStatusId int=2,--Warm lead
			@Comment varchar(500)='Automatically Approved Lead.',
			@IsAccepted bit = 1,
			@PrefTdDate datetime =GETDATE()

    --Take action on all ncd leads which have more than 120 hour difference b/w EntryDate and current date.  
	WHILE @RowCount <= @NumberRecords 
		BEGIN

			SELECT @CustomerId = CustId,@InquiryId = InqId
		    FROM @TempInquiry
		    WHERE RowID = @RowCount

			---Same working functionality as this SP "NCD_InquiryFollowup_Insert"

			 DECLARE @Id INT

			 INSERT INTO NCD_InquiryFollowup
				(InquiryId,CustomerId,LastCallTime,NextCallTime,LeadStatusId,Comment,TestDriveDate,AssignedExecutive)
			 Values(@InquiryId,@CustomerId,GETDATE(),@NextCallTime,@LeadStatusId,@Comment,@PrefTdDate,@Executive)

			 SET @Id=@@IDENTITY  --This id is Inq_FollowupId
			 
			UPDATE NCD_Inquiries SET NextCallTime=@NextCallTime,LeadStatusId=@LeadStatusId,LatestActionDesc=@Comment,IsActionTaken=1,IsAccepted=@IsAccepted,AssignedExecutive=@Executive
		    WHERE Id=@InquiryId

			--same working functionality as SP "NCD_LeadRequest_Insert"
			--insert data into NCD_LeadRequest Table ,RequestType = 1 is define as PQRequest.
			IF NOT EXISTS(SELECT NLR.Id FROM NCD_LeadRequest AS NLR WITH(NOLOCK) WHERE NLR.InquiryId=@InquiryId and NLR.RequestType=1)
				BEGIN
				    --RequestType=1 for PQRequest
					INSERT Into NCD_LeadRequest (InquiryId,Inq_FollowupId,RequestType,FollowupDate)
					Values(@InquiryId,@Id,1,GETDATE())
				END
			SET @RowCount = @RowCount + 1
		END
END
