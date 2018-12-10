IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_LeadRequestType]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_LeadRequestType]
GO

	
-- =============================================
-- Author:		Umesh Ojha
-- Create date: 31/10/2011
-- Description:	Return Request type (Inquiries For) Particular lead
-- =============================================
CREATE PROCEDURE [dbo].[NCD_LeadRequestType] 
	-- Add the parameters for the stored procedure here
	@InquiryId int
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @result VARCHAR(MAX)

	SELECT @result = COALESCE(@result + ', ', '') + 
	case when convert(varchar(100),RequestType)= '1' then 'Price Quote'
	when convert(varchar(100),RequestType)='2' then 'Test Drive'
	when convert(varchar(100),RequestType)='4' then 'Loan Inquiry' 
	end
	FROM NCD_LeadRequest
	WHERE InquiryId = @InquiryId

	SELECT @result as InquiriesFor
END

