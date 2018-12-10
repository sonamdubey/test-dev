IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Impact_GetRunningSlot]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Impact_GetRunningSlot]
GO

	

-- =============================================
-- Author      : Vinay Kumar Prajapati
-- Create date : 19th Sept 2014
-- Description : Get Running Slot and package  from  Impact_Slot Table  on the basis of brand and city .
-- =============================================

CREATE PROCEDURE  [dbo].[Impact_GetRunningSlot]
    (
	@Brand INT,
	@CityId INT ,
	@PackageId INT OUTPUT 
	)
 AS

   BEGIN
		SELECT @PackageId= IPS.PackageTypeId 
		FROM Impact_Slot AS IPS WITH(NOLOCK) WHERE  
			IPS.CityId=@CityId AND IPS.MakeId=@Brand AND
			IPS.IsActive=1

		IF @@ROWCOUNT = 0 
			SET @PackageId=-1
   END