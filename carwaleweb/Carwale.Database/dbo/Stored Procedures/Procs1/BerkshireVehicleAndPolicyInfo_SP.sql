IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BerkshireVehicleAndPolicyInfo_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BerkshireVehicleAndPolicyInfo_SP]
GO

	CREATE PROCEDURE [dbo].[BerkshireVehicleAndPolicyInfo_SP] 
	@leadId  bigint,
	@cityName varchar(100) out,
	@carMakeYr int out,
	@vehicleInfo varchar(300) out,	
	@policyType int out
AS

BEGIN

DECLARE @makeId int,@modelId int,@versionId int,@cityId int

SELECT @carMakeYr= CarMakeYear,@makeId=BerkshireMakeId,@modelId=BerkshireModelId,@versionId=BerkshireVersionId,
@cityId= BerkshireCityId ,@policyType=PolicyType
from dbo.BerkshireInsuranceLeads as B where B.BerkshireLeadId = @leadId

SELECT @cityName= CITY  FROM dbo.BerkshireCityInfo WHERE CITY_CODE=@cityId

SELECT @vehicleInfo=COMBINATION2  from dbo.BerkshireVehicleInfo  WHERE MAKE_CODE=@makeId AND MODEL_CODE=@modelId AND SUBTYPE_CODE=@versionId

END
