IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetConfigurableAppointmentDate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetConfigurableAppointmentDate]
GO

	-- =============================================
-- Author:		Yuga Hatolkar
-- Create date: 24th June, 2015
-- Description:	To Get Configurable Appointment Date
-- EXEC AbSure_GetConfigurableAppointmentDate date,day
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetConfigurableAppointmentDate] 
	@Category VARCHAR(100)
	,@Parameter VARCHAR(100)		
AS
BEGIN
	SELECT ConstantValue FROM AbSure_ConfigurableParameters WHERE Category = @Category AND Parameter = @Parameter
END









