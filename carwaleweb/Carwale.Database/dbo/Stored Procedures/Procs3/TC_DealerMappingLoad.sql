IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DealerMappingLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DealerMappingLoad]
GO

	-- =============================================
-- Author:        Tejashree Patil.
-- Create date: 10 Dec 2013.
-- Description:    This sp is used for load Inventory(Stock) related dealer mapping.
-- EXEC TC_DealerMappingLoad 20,NULL
-- Modified By : Tejashree Patil on 16 Dec 2013, Added DISTINCT in else part query.
-- =============================================
CREATE PROCEDURE    [dbo].[TC_DealerMappingLoad]
    @MakeId INT,
    @DealerAdminId INT = NULL,--To identify Is Dealer list required or Mapping Dealer list required.
    @CityId INT = NULL
AS
BEGIN

    --Get all dealer list for mapping.
    IF (@DealerAdminId IS NULL)
    BEGIN
        SELECT    DISTINCT D.ID Value, D.Organization Text, D.DealerCode
        FROM    Dealers D WITH(NOLOCK)
                INNER JOIN    TC_BrandZone BZ WITH(NOLOCK) ON 
                            BZ.TC_BrandZoneId=D.TC_BrandZoneId
                INNER JOIN    TC_StockInventory SI WITH(NOLOCK) ON 
                            SI.BranchId = D.ID
        WHERE    D.IsDealerActive=1
                AND BZ.MakeId=@MakeId
        --ORDER BY D.ID DESC, D.TC_BrandZoneId

        EXEC TC_GetMappingDealersList

        SELECT  DISTINCT C.ID AS Value, C.Name AS Text 
        FROM    Cities AS C
                INNER JOIN Dealers D ON D.CityId = C.ID
                INNER JOIN TC_BrandZone BZ ON BZ.TC_BrandZoneId = D.TC_BrandZoneId
        WHERE    D.IsDealerActive=1
                AND BZ.MakeId=@MakeId
                AND IsDeleted=0
          
    END
    ELSE
    BEGIN
        /*
        --Get all dealer list for mapping except one which is to map with.
        SELECT    D.ID Value,    D.Organization Text,    D.DealerCode,
                (CASE WHEN SD.SubDealerId IS NULL THEN 0 ELSE 1 END) AS IsMapped,
                (CASE WHEN DM.DealerAdminId IS NULL THEN NULL ELSE DM.DealerAdminId END) AS AdminDealerId,
                SD.IsActive AS IsActiveSubDealer
        FROM    Dealers D WITH(NOLOCK)
                --INNER JOIN    TC_BrandZone BZ WITH(NOLOCK) ON 
                --            BZ.TC_BrandZoneId=D.TC_BrandZoneId
                INNER JOIN    TC_SubDealers SD WITH(NOLOCK)
                            ON SD.SubDealerId = D.ID AND SD.IsActive=1
                INNER JOIN    TC_DealerMapping DM WITH(NOLOCK)
                            ON DM.TC_DealerMappingId = SD.TC_DealerMappingId
        WHERE    D.IsDealerActive=1
                --AND BZ.MakeId=@MakeId
                AND D.ID <> @DealerAdminId
                AND DM.DealerAdminId = @DealerAdminId
                --ORDER BY IsMapped DESC, SD.SubDealerId, D.TC_BrandZoneId, D.ID 

        UNION
        */

        SELECT    DISTINCT D.ID AS Value, D.Organization AS Text , D.DealerCode--,0 AS IsMapped, 0 AS AdminDealerId, AS AdminDealerId
        FROM    Dealers D  
                INNER JOIN Cities AS C ON D.CityId = C.ID  
                INNER JOIN TC_BrandZone BZ WITH(NOLOCK) ON 
                            BZ.TC_BrandZoneId=D.TC_BrandZoneId  
                LEFT JOIN TC_DealerCities AS TD ON D.ID=TD.DealerId  
        WHERE    C.Id = @CityId 
                AND C.IsDeleted = 0
                AND D.IsDealerActive=1
                AND D.ID <> @DealerAdminId
                AND (TD.DealerId IS NULL OR TD.IsActive=1)
                AND BZ.MakeId=@MakeId
        --ORDER BY D.Organization, C.ID
    END
END