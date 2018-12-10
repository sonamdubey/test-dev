IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AddServiceAppointment]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AddServiceAppointment]
GO

	
-- =============================================
-- Author:		Vaibhav K
-- Create date: 14-June-2012
-- Description:	Inserts a service appointment request for skoda customers
-- =============================================
CREATE PROCEDURE [dbo].[OLM_AddServiceAppointment]
	-- Add the parameters for the stored procedure here
	@ServiceCenterId	NUMERIC,
	@FullName			VARCHAR(150),
	@Email				VARCHAR(100),
	@Mobile				VARCHAR(15),
	@City				NUMERIC,
	@PreferredDate		DATETIME = NULL,
	@VehicleRegNum		VARCHAR(15) = NULL,
	@VehicleIdnNum		VARCHAR(20) = NULL,
	@CarKms				NUMERIC = 0,
	@Comments			VARCHAR(500) = NULL,
	@IPAddress			VARCHAR(20) = NULL,
	@IsMailSent			BIT = FALSE,
	@EntryDate			DATETIME = GETDATE
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO OLM_ServiceAppointments (ServiceCenterId,FullName,Email,Mobile,City,PreferredDate,
		VehicleRegNum,VehicleIdnNum,CarKms,Comments,IPAddress,IsMailSent,EntryDate)
	VALUES (@ServiceCenterId,@FullName,@Email,@Mobile,@City,@PreferredDate,@VehicleRegNum,@VehicleIdnNum,
		@CarKms,@Comments,@IPAddress,@IsMailSent,@EntryDate)
END

