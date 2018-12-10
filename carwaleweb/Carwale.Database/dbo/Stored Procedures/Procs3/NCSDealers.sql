IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCSDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCSDealers]
GO

	-- =============================================
-- Author:		Raghupathy
-- Create date: 11/9/2013
-- Description:	Returns all the New Car Dealers for a city for a particular make
-- Modified By - Deepak 14th Sep 2013, Added AND NCSD.IsNCDDealer = 0 AND NCSD.Name NOT LIKE '%H-5000%'
-- Modified By - Deepak 30th Jan 2014
-- Modified By - Raghu 30th jan 2014, Added MakeId <> 11 and NCSD.Name NOT LIKE '%H5000%'
-- Modified By - Vinayak 7th oct 2014, Added AreaName in select statement'
-- =============================================
CREATE PROCEDURE [dbo].[NCSDealers] --[dbo].[NCSDealers]9,1
	-- Add the parameters for the stored procedure here
		@MakeId INT,
		@CityId INT
AS
BEGIN
    SELECT DISTINCT NCSD.ID AS ID,  ISNULL(NCSD.AreaName  + ' - ','') + NCSD.DealerTitle As DealerName -- Modified By - Vinayak 7th oct 2014, Added AreaName in select statement'
    FROM NCS_Dealers AS NCSD WITH(NOLOCK)  
    LEFT JOIN NCS_DealerMakes DM WITH(NOLOCK) ON NCSD.Id = DM.DealerId AND NCSD.IsActive = 1
    WHERE NCSD.CityId =@CityId and DM.MakeId = @MakeId --AND DM.MakeId <> 11
    AND NCSD.DealerType = 0
	AND NCSD.Name NOT LIKE '%H-5000%'
	AND NCSD.Name NOT LIKE '%H5000%'
	--Added By Deepak on 30th Jan 2014 to stop showing blocked dealers.
	AND NCSD.ID NOT IN (SELECT DealerID FROM CRM.FLCDealerPriority WITH (NOLOCK) WHERE ((ModelId IN(SELECT Id FROM CarModels WITH(NOLOCK) WHERE CarMakeId = @MakeId) AND Priority=2) OR (ModelId=-1 AND Priority=2))) 
    --ORDER BY ID
	ORDER BY DealerName
END
