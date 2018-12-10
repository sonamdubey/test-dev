IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[GetRecommendedUsedCarAlerts]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [UCAlert].[GetRecommendedUsedCarAlerts]
GO

	-- =============================================
-- Author:	   Manish Chourasiya
-- Create date: 09/07/2013
-- Description:	Procedure to retrieve recommend used car data
-- Modified By: Manish on 23-07-2013 Adding order by clause for handling duplicate mails. This ordering will help in front end coding logic for sending mail
-- Modified by: Manish on 11-03-2014 adding column customer security key 
-- Modified by: Manish on 28-03-2014 adding column Algorithm ID
-- Modified by: Manish on 23-04-2014 changed for sending email on third day also.
-- Modified by: Manish on 25-04-2014 changed for sending email on 10,15,20 day also.
-- =============================================
CREATE PROCEDURE [UCAlert].[GetRecommendedUsedCarAlerts]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	with CTE
	as
	( 
	SELECT 
	[Rank] AS rownum,
    U.UsedCarAlert_Id	AS	UsedCarAlert_Id, 
    CustomerName AS CustomerName,    
    CustomerAlert_Email	AS	CustomerAlert_Email,     
    ProfileId	AS	ProfileId,       
	Car_City	AS	Car_City,        
	Car_SellerId	AS	Car_SellerId,    
	Car_Price	AS	Car_Price,       
	Car_MakeId	AS	Car_MakeId,      
	Car_ModelId	AS	Car_ModelId,     
	Car_Make	AS	Car_Make,        
	Car_Model	AS	Car_Model,       
	CASE Car_SellerType WHEN 1 THEN 'Dealer' else 'Individual' end  AS	Car_Seller,      
	Car_Kms	        AS      Car_Kms, 
	Car_Version	AS	Car_Version,     
	YEAR(Car_Year)	AS	Car_Year,        
	Car_Color	AS	Car_Color,
	Car_HasPhoto	AS	Car_HasPhoto,
	Car_LastUpdated	AS	Car_LastUpdated ,
	Car_City	AS	CustomerAlert_City,      
	NULL	AS	CustomerAlert_Budget,    
	NULL	AS	CustomerAlert_MakeYear,  
	NULL	AS	CustomerAlert_Kms,       
	CustomerAlert_Make	AS	CustomerAlert_Make,      
	CustomerAlert_Model	AS	CustomerAlert_Model,     
	NULL	AS	CustomerAlert_FuelType,  
	NULL	AS	CustomerAlert_BodyStyle, 
	NULL	AS	CustomerAlert_Transmission,
	NULL	AS	CustomerAlert_Seller,
	W.alertUrl	AS	alertUrl,
	ImageUrl AS ImageUrl,
	CSK.CustomerKey AS CustomerKey,
	W.UsedCarAlertAlgoTypeId
	from UCAlert.RecommendUsedCarAlert as W  WITH (NOLOCK)
	JOIN UCAlert.UserCarAlerts AS U WITH (NOLOCK) ON W.CustomerId=U.CustomerId AND U.IsAutomated=1 AND U.IsActive=1      
	                                AND (   U.EntryDateTime=CONVERT(DATE,GETDATE()-4) 
									     OR U.EntryDateTime=CONVERT(DATE,GETDATE())
										 OR U.EntryDateTime=CONVERT(DATE,GETDATE()-2)
										 OR U.EntryDateTime = CONVERT(DATE,GETDATE()-9)
							             OR U.EntryDateTime = CONVERT(DATE,GETDATE()-14)
							             OR U.EntryDateTime = CONVERT(DATE,GETDATE()-19)
                                        )  --this condition checks the subscribtion of user when user unsubscribed when got first mail
    LEFT JOIN CustomerSecurityKey AS CSK WITH (NOLOCK) ON CSK.CustomerId=U.CustomerId
	where Is_Mailed=0 AND CustomerAlert_Email NOT LIKE '%@unknown.com'
	)
	select w1.*,w3.MatchingCars
	from CTE w1
	  join (select UsedCarAlert_Id,COUNT(*) as MatchingCars -- Count is to set total matching Cars found for the criteria
			from CTE w2		
			group by UsedCarAlert_Id 
		  ) as w3 on w3.UsedCarAlert_Id=w1.UsedCarAlert_Id
	       ORDER BY UsedCarAlert_Id,UsedCarAlertAlgoTypeId,[rownum] DESC  -----This order by clause used to handle duplicate emails. This helps in front end coding logic
END


