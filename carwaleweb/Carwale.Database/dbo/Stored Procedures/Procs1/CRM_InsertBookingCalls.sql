IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_InsertBookingCalls]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_InsertBookingCalls]
GO

	-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <14/2/2014>
-- Description:	<Insert Data into CRM_BookingCalls>
-- =============================================
CREATE PROCEDURE [dbo].[CRM_InsertBookingCalls] 
	-- Add the parameters for the stored procedure here
	@CallId		BIGINT,
	@TaggedBy	INT

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO CRM_BookingCalls(CallId,TaggedBy) VALUES (@CallId,@TaggedBy)
END
