IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Service_SaveCompletedInquiries]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Service_SaveCompletedInquiries]
GO

	-- =============================================
-- Author:	Nilima More	
-- Create date: 11th July 2016
-- Description:	Save complted inquiries details.
--exec TC_Service_SaveCompletedInquiries 26281,100,'name',12,'2016-07-13 11:36:35.787','re',88916,25721,26250
--Modified By : Nilima More On 13th Aug 2016,optimize sp code and emoe lead updation part.
-- =============================================
CREATE PROCEDURE [dbo].[TC_Service_SaveCompletedInquiries] 
@TC_InquiriesLeadId INT,
@Amount INT ,
@Invoice VARCHAR(100) = NULL,
@NextServiceKms INT,
@NextServiceDate DATETIME,
@Comments VARCHAR(200) = NULL,
@UserId INT,
@TC_LeadId INT,
@CustomerId INT,
@NextServiceType TINYINT = 1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;
	DECLARE @TC_Service_InquiriesId INT

	 SELECT @TC_Service_InquiriesId = TC_Service_InquiriesId
	 FROM TC_Service_Inquiries WITH(NOLOCK) WHERE TC_InquiriesLeadId = @TC_InquiriesLeadId

	 --Modified By : Nilima More On 13th Aug 2016,optimize sp code and emoe lead updation part.

	----Insert data into TC_Service_CompletedInquiries.
	INSERT INTO  TC_Service_CompletedInquiries(TC_Service_InquiriesId,Amount,NextServiceKms,NextServiceDate,Comments,invoiceKey)
	VALUES(@TC_Service_InquiriesId,@Amount,@NextServiceKms,@NextServiceDate,@Comments,@Invoice)

	
END


