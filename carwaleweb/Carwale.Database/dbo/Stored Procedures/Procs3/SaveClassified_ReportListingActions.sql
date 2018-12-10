IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SaveClassified_ReportListingActions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SaveClassified_ReportListingActions]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 30-Aug-2012
-- Description:	Save the action taken for classified listings
-- =============================================
CREATE PROCEDURE [dbo].[SaveClassified_ReportListingActions]
	-- Add the parameters for the stored procedure here
	@ProfileId		NUMERIC,
	@Reason			VARCHAR(200),
	@ActionTakenBy	NUMERIC,
	@ActionTakenOn	DATETIME,
	@Type			SMALLINT,
	@NewId			NUMERIC OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SET @NewId = -1
    -- Insert statements for procedure here
	INSERT INTO Classified_ReportListingActions (ProfileId, Comments, ActionTakenBy, ActionTakenOn, Type)
	VALUES (@ProfileId, @Reason, @ActionTakenBy, @ActionTakenOn, @Type)
	
	SET @NewId = SCOPE_IDENTITY()
	
	UPDATE Classified_ReportListing
		SET IsActionTaken = 1
	WHERE InquiryId = @ProfileId
END
