IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ct_insertmigrationfailedleads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ct_insertmigrationfailedleads]
GO

	
-- =============================================
-- Author:		Vaibhav K
-- Create date: 25 Aug 216
-- Description:	insert migration failed to be logged in db table
-- =============================================
CREATE PROCEDURE [dbo].[ct_insertmigrationfailedleads]
	-- Add the parameters for the stored procedure here
	@custName varchar(100)=null,
	@custMobile varchar(10)=null,
	@custEmail varchar(50)=null,
	@cw_lead_id int=null,
	@cw_dealer_id int=null,
	@model_id int=null,
	@source smallint=null,
	@lead_date varchar(50)=null,
	@lead_lastUpdated varchar(50)=null,
	@mfgyear int=null,
	@status varchar(50)=null,
	@substatus varchar(50)=null,
	@statusDate varchar(50)=null,
	@custComments varchar(1000)=null,
	@statusCategory varchar(50)=null,
	@apiResponse varchar(2000)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	insert into ct_migrationfailedleads
	(
		custName, custMobile, custEmail, cw_lead_id, cw_dealer_id, model_id, source, lead_date, 
		lead_lastUpdated, mfgyear, status, substatus, statusDate, custComments, statusCategory, apiResponse
	)
	values
	(
		@custName, @custMobile, @custEmail, @cw_lead_id, @cw_dealer_id, @model_id, @source, @lead_date, 
		@lead_lastUpdated, @mfgyear, @status, @substatus, @statusDate, @custComments, @statusCategory, @apiResponse
	)
END
