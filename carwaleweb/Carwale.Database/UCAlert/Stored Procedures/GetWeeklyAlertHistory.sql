IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[GetWeeklyAlertHistory]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [UCAlert].[GetWeeklyAlertHistory]
GO

	-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 20/1/2012
-- Description:	Procedure to retrieve the weekly alerts data with total no of cars found.
-- Modified By: Manish on 26-08-2015 added try and exception block
-- =============================================
CREATE PROCEDURE [UCAlert].[GetWeeklyAlertHistory]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY
    -- Insert statements for procedure here
	insert into  [UCAlert].[WeeklyAlertHistory]
	select  UsedCarAlert_Id	,
			CustomerAlert_Email,	
			alertUrl,COUNT(*) as cnt,
			Getdate() as UpdatedOn	
	--into  [UCAlert].[WeeklyAlertHistory]
	from UCAlert.WeeklyAlerts as W WITH(NOLOCK)
	where Is_Mailed=0
	group by UsedCarAlert_Id,CustomerAlert_Email,alertUrl 
	 END TRY
    BEGIN CATCH
	INSERT INTO CarWaleWebSiteExceptions (
			                                ModuleName,
											SPName,
											ErrorMsg,
											TableName,
											FailedId,
											CreatedOn)
						            VALUES ('Used Car Weekly alert email',
									        'UCAlert.GetWeeklyAlertHistory',
											 ERROR_MESSAGE(),
											 'UCAlert.WeeklyAlertHistory',
											 NULL,
											 GETDATE()
                                            )

	END CATCH;
	
END

