IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[MM_UpdateInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[MM_UpdateInquiry]
GO

	
-- =============================================
-- Author:		Vaibhav K
-- Create date: 18-Apr-2013
-- Description:	Update MM_Inquiries once the inquiry is pushed to trading cars
-- =============================================
CREATE PROCEDURE [dbo].[MM_UpdateInquiry]
	-- Add the parameters for the stored procedure here
	@MM_InquiriesId			INT,
	@TC_RecordId			INT,
	@IsPushedToTC			BIT,	
	@PushedOn				DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE MM_Inquiries
		SET TC_RecordId = @TC_RecordId, IsPushedToTC = @IsPushedToTC, PushedOn = @PushedOn
	WHERE MM_InquiriesId = @MM_InquiriesId
END

