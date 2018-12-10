IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[LA_SaveLmsData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[LA_SaveLmsData]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 9-Apr-2013
-- Description:	Save the returned data once lead is pushed into SKODA LMS
-- =============================================
CREATE PROCEDURE [dbo].[LA_SaveLmsData]
	-- Add the parameters for the stored procedure here
	@LAId			NUMERIC,	
	@TokenNo		VARCHAR(50),
	@LeadStatus		VARCHAR(50),
	@ResultCode		VARCHAR(50),
	@PushStatus		VARCHAR(200),
	@DmsId			VARCHAR(50),
	@Status			SMALLINT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	--Intial status to be returned is -1
	SET @Status = -1
	SELECT LAId FROM LA_LmsData WHERE LAId = @LAId

    -- Insert statements for procedure here	
	IF @@ROWCOUNT = 0
		BEGIN
			INSERT INTO LA_LmsData (LAId, TokenNo, LeadStatus, ResultCode, PushStatus, DmsId)
			VALUES (@LAId, @TokenNo, @LeadStatus, @ResultCode, @PushStatus, @DmsId)
			
			--for success return 1
			SET @Status = 1
		END
	ELSE
		BEGIN
			UPDATE LA_LmsData
				SET TokenNo = @TokenNo, LeadStatus = @LeadStatus, ResultCode = @ResultCode,
					PushStatus = @PushStatus, DmsId = @DmsId
			WHERE LAId = @LAId
			
			--for success return 1
			SET @Status = 1
		END
END
