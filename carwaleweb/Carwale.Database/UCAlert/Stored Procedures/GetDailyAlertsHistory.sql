IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[GetDailyAlertsHistory]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [UCAlert].[GetDailyAlertsHistory]
GO

	-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 20/1/2012
-- Description:	Procedure to retrieve the Daily alerts data with total no of cars found.
-- Modified By: Manish on 19-08-2015 added try and exception block
-- =============================================
CREATE PROCEDURE [UCAlert].[GetDailyAlertsHistory]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	BEGIN TRY
	
	insert into   [UCAlert].[DailyAlertsHistory]
	select  UsedCarAlert_Id	,
			CustomerAlert_Email,	
			alertUrl,COUNT(*) as cnt,
			Getdate() as UpdatedOn	
	from UCAlert.DailyAlerts as W 
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
						            VALUES ('Used Car Daily alert email',
									        'UCAlert.GetDailyAlertsHistory',
											 ERROR_MESSAGE(),
											 'UCAlert.DailyAlertsHistory',
											 NULL,
											 GETDATE()
                                            )

	END CATCH;
		
END

