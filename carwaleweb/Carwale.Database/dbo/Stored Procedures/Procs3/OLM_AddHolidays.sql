IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AddHolidays]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AddHolidays]
GO

	
-- =============================================
-- Author:		Vaibhav K
-- Create date: 12-June-2012
-- Description:	Inserts a record for holidays for skoda dealerships
-- =============================================
CREATE PROCEDURE [dbo].[OLM_AddHolidays]
	-- Add the parameters for the stored procedure here
	@Holiday	DATETIME,
	@DealerId	INT =-1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO OLM_HolidayList (DealerId,Holiday)
	VALUES (@DealerId,@Holiday)
END

