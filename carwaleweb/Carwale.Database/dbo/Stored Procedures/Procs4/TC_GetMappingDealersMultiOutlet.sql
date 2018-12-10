IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetMappingDealersMultiOutlet]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetMappingDealersMultiOutlet]
GO

	
-- =============================================
-- Author:		<vivek rajak>
-- Create date: <20/05/2015>
-- Description:	<To insert distinct dropdown delaers>
-- Modifier  : Ajay Singh(1 feb 2016)
-- Description : To add GroupType
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetMappingDealersMultiOutlet]
@CityId VARCHAR(20)	,
@DealerAdminId VARCHAR(20),
@ApplicationId VARCHAR(20),
@IsGroup BIT = 0
AS
BEGIN
    IF @IsGroup = 1
	   BEGIN
	       SELECT D.ID AS Value, (D.Organization + '-' + CONVERT(VARCHAR, D.ID)) AS Text 
	       FROM Dealers D	WITH(NOLOCK)		
           WHERE D.CityID = @CityId 
	       AND D.Organization IS NOT NULL
	       AND D.Organization <> '' 	
	       AND IsDealerActive = 1 
	       AND IsTCDealer = 1 
	       AND D.ApplicationId = @ApplicationId
	       AND D.ID NOT IN (SELECT DealerId 
					 FROM TC_DealerAdminMapping WITH(NOLOCK) 
					 WHERE DealerAdminId = @DealerAdminId)
           AND D.IsMultiOutlet = @IsGroup
		   AND D.ID NOT IN(SELECT DealerId FROM TC_DealerAdminMapping WITH(NOLOCK) WHERE IsGroup = 1)
	       ORDER BY Text 
	   END
   ELSE
	  BEGIN
	       SELECT D.ID AS Value, (D.Organization + '-' + CONVERT(VARCHAR, D.ID)) AS Text 
	       FROM Dealers D	WITH(NOLOCK)		
           WHERE D.CityID = @CityId 
	       AND D.Organization IS NOT NULL
	       AND D.Organization <> '' 	
	       AND IsDealerActive = 1 
	       AND IsTCDealer = 1 
	       AND D.ApplicationId = @ApplicationId
	       AND D.ID NOT IN (SELECT DealerId 
					 FROM TC_DealerAdminMapping WITH(NOLOCK) 
					 WHERE DealerAdminId = @DealerAdminId)
	       AND D.IsMultiOutlet = 0
		   AND D.IsGroup=0
		   AND D.ID NOT IN(SELECT DealerId FROM TC_DealerAdminMapping WITH(NOLOCK))
	       ORDER BY Text 
	 END
END

 
