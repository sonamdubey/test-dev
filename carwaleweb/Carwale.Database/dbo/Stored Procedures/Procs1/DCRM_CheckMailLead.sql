IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_CheckMailLead]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_CheckMailLead]
GO

	
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_CheckMailLead]
	-- Add the parameters for the stored procedure here
	@CheckBit		BIT,
	@DealerId		NUMERIC(18)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		DECLARE @NumberRecords INT = 0

    -- Insert statements for procedure here
    IF(@CheckBit = 1)
		BEGIN
			SELECT DealerId FROM DCRM_MailLeads WHERE DealerId = @DealerId
			SET @NumberRecords = @@ROWCOUNT
			IF(@NumberRecords = 0)
			BEGIN
				INSERT INTO DCRM_MailLeads (DealerId) VALUES (@DealerId)
			END
		END
	ELSE
		BEGIN
			DELETE FROM DCRM_MailLeads WHERE DealerId = @DealerId
		END
END

