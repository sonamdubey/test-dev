IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetDealerDetailsforAddCar]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetDealerDetailsforAddCar]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 31st Aug 2015
-- Description:	To get all the details of dealer for add car api
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetDealerDetailsforAddCar] 
	@DealerId INT
AS
BEGIN
	SELECT DISTINCT  D.Id DealerId,D.Organization AS DealerName,D.RCNotMandatory AS IsRCNotMandatory,
		    C.Name City,D.MobileNo, ISNULL(D.Address1,D.Address2) Address,D.EmailId EmailId,D.AreaId AreaId,D.CityId CityId
						
	FROM	Dealers D WITH(NOLOCK)
			INNER JOIN Cities C						WITH(NOLOCK)	ON C.ID = D.CityId			
			INNER JOIN States S						WITH(NOLOCK)	ON S.ID = D.StateId	
			INNER JOIN Areas A						WITH(NOLOCK)	ON A.ID = D.AreaId

	WHERE D.ID=@DealerId
END
