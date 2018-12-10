IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[GetWeeklyAlerts]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [UCAlert].[GetWeeklyAlerts]
GO

	-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 20/1/2012
-- Description:	Procedure to retrieve the weekly alerts data with total no of cars found.
-- Modified By: Manish Chourasiya on 24-06-2014 for considering the emails for used car new design.
-- =============================================
CREATE PROCEDURE [UCAlert].[GetWeeklyAlerts]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	with WKAlert
	as
	(
	select ROW_NUMBER() over( partition by  w.UsedCarAlert_Id order by w.weeklyAlert_Id ) as rownum,
		    w.UsedCarAlert_Id	,
			CustomerAlert_Email	,
			W.ProfileId	,
			Car_City	,
			Car_SellerId	,
			Car_Price	,
			Car_MakeId	,
			Car_ModelId	,
			Car_Make	,
			Car_Model	,
			Car_Seller	,
			Car_Kms	,
			Car_Version	,
			Car_Year	,
			Car_Color,
			Car_HasPhoto,
			Car_LastUpdated	,
			CustomerAlert_City	,
			CustomerAlert_Budget	,
			CustomerAlert_MakeYear	,
			CustomerAlert_Kms	,
			CustomerAlert_Make	,
			CustomerAlert_Model	,
			CustomerAlert_FuelType	,
			CustomerAlert_BodyStyle	,
			CustomerAlert_Transmission,
			CustomerAlert_Seller,
			w.alertUrl  
	from UCAlert.WeeklyAlerts as W
	JOIN UCAlert.UserCarAlerts as U on U.UsedCarAlert_Id=w.UsedCarAlert_Id and U.IsActive=1
	JOIN UCAlert.NewlyAddedWeeklyCars as N on N.ProfileId=W.ProfileId 
	where Is_Mailed=0
	), cte as  (
	select ROW_NUMBER() over( partition by  w.UsedCarAlert_Id order by w.weeklyAlert_Id ) as rownum,
		    w.UsedCarAlert_Id	,
			CustomerAlert_Email	,
			W.ProfileId	,
			Car_City	,
			Car_SellerId	,
			Car_Price	,
			Car_MakeId	,
			Car_ModelId	,
			Car_Make	,
			Car_Model	,
			Car_Seller	,
			Car_Kms	,
			Car_Version	,
			Car_Year	,
			Car_Color,
			Car_HasPhoto,
			Car_LastUpdated	,
			CustomerAlert_City	,
			CustomerAlert_Budget	,
			CustomerAlert_MakeYear	,
			CustomerAlert_Kms	,
			CustomerAlert_Make	,
			CustomerAlert_Model	,
			CustomerAlert_FuelType	,
			CustomerAlert_BodyStyle	,
			CustomerAlert_Transmission,
			CustomerAlert_Seller,
			w.alertUrl  
	from UCAlert.WeeklyAlerts as W
	JOIN UCAlert.UserCarAlerts as U on U.UsedCarAlert_Id=w.UsedCarAlert_Id and U.IsActive=1
	JOIN UCAlert.NewlyAddedWeeklyCars as N on N.ProfileId=W.ProfileId 
	where Is_Mailed=0
	)
	select w1.*,w3.MatchingCars
	from WKAlert w1
	  join (select UsedCarAlert_Id,COUNT(*) as MatchingCars -- Count is to set total matching Cars found for the criteria
			from WKAlert w2		
			group by UsedCarAlert_Id 
		  ) as w3 on w3.UsedCarAlert_Id=w1.UsedCarAlert_Id
	where w1.rownum<=5
	UNION ALL
	select w1.*,w3.MatchingCars
	from CTE w1
	  join (select UsedCarAlert_Id,COUNT(*) as MatchingCars -- Count is to set total matching Cars found for the criteria
			from CTE w2		
			group by UsedCarAlert_Id 
		  ) as w3 on w3.UsedCarAlert_Id=w1.UsedCarAlert_Id
	where w1.rownum<=5
END

