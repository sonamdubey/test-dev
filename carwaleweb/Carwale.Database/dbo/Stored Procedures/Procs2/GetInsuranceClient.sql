IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetInsuranceClient]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetInsuranceClient]
GO

	-- =============================================      
-- Author: ROHAN
-- Create date: 16 JUNE 2016
-- Description: INSURANCE INPUTS <---> WHICH CLIENT RULES
-- =============================================      
CREATE PROCEDURE [dbo].[GetInsuranceClient]    
@LeadSource int = NULL
,@ApplicationId int =NULL
,@PlatformId int   =NULL
,@CityId int       =NULL
,@State INT      =NULL
,@Price int     =NULL
,@isNew bit        =NULL
,@Age bit   =NULL
,@Salary int       =NULL
,@ClientId int out
AS      
BEGIN
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;      
      
SELECT top 1 @ClientId=ClientId from InsuranceRules I With(nolock) where 
I.IsActive=1
AND
(@LeadSource IS NULL OR I.LeadSource=@LeadSource OR I.LeadSource IS NULL)
AND
(@ApplicationId IS NULL OR I.ApplicationId=@ApplicationId OR I.ApplicationId IS NULL)
 AND
(@PlatformId IS NULL OR I.PlatformId=@PlatformId OR I.PlatformId IS NULL)
AND
(@CityId IS NULL OR I.CityId=@CityId OR I.CityId IS NULL)
AND
(@State IS NULL OR I.State=@State OR I.State IS NULL)
AND
(@Price IS NULL OR (@Price BETWEEN I.MinPrice AND I.MaxPrice) OR I.MinPrice IS NULL OR I.MaxPrice IS NULL)
AND
(@isNew IS NULL OR I.isNew=@isNew OR I.isNew IS NULL)
AND
(@Age IS NULL OR (@Age BETWEEN I.MinAge AND I.MaxAge) OR I.MinAge IS NULL OR I.MaxAge IS NULL)
AND
(@Salary IS NULL OR (@Price BETWEEN I.MinSalary AND I.MaxSalary) OR I.MinSalary IS NULL OR I.MaxSalary IS NULL)
order by I.RULEID desc
END 