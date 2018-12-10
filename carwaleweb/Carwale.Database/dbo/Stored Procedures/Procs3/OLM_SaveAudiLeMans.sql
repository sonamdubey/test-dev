IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_SaveAudiLeMans]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_SaveAudiLeMans]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 11-June-2013
-- Description:	Save the record for the Audi Le Mans form
-- =============================================
CREATE PROCEDURE [dbo].[OLM_SaveAudiLeMans]
	-- Add the parameters for the stored procedure here
	@CustomerName		VARCHAR(100),
	@ContactNumber		VARCHAR(15),
	@CurrentCar			VARCHAR(50),
	@AudiDealershipId	INT,
	@AudiDealershipName	VARCHAR(50), 
	@PreferredTime		VARCHAR(50),
	@Comments			VARCHAR(500),
	@NewId				NUMERIC OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SET @NewId = -1
	
    -- Insert statements for procedure here
	INSERT INTO OLM_AudiLeMans( CustomerName, ContactNumber, CurrentCar, AudiDealershipId, AudiDealershipName, 
		PreferredTime, Comments )
	VALUES( @CustomerName, @ContactNumber, @CurrentCar, @AudiDealershipId, @AudiDealershipName, 
		@PreferredTime, @Comments )
		
	SET @NewId = SCOPE_IDENTITY()
END
