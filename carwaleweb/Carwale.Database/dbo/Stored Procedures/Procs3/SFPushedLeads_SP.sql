IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SFPushedLeads_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SFPushedLeads_SP]
GO

	CREATE PROCEDURE [dbo].[SFPushedLeads_SP] 
	-- Add the parameters for the stored procedure here
	@QuoteId		NUMERIC,
	@SFLeadId		Varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF EXISTS (SELECT QuoteId FROM SFPushedLeads WHERE QuoteId=@QuoteId)
		BEGIN
			-- Update statements for procedure here
			UPDATE SFPushedLeads 
			SET SFLeadId = @SFLeadId WHERE QuoteId = @QuoteId	
		END
	ELSE
		BEGIN
			-- Insert statements for procedure here
			INSERT INTO SFPushedLeads(QuoteId, SFLeadId)
			VALUES(@QuoteId, @SFLeadId)
		END
END