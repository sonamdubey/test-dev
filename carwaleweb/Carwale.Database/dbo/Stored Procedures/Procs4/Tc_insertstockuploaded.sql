IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Tc_insertstockuploaded]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Tc_insertstockuploaded]
GO

	
CREATE PROCEDURE [dbo].[Tc_insertstockuploaded] 
AS 
    INSERT INTO TC_DealerStock( DealerId,
								StockCount,
								Entrydate) 
    SELECT BranchId  AS DealerId, 
           Count(id) AS Stockcount, 
           Getdate() AS Entrydate 
    FROM   TC_Stock with(nolock)
    WHERE  IsSychronizedCW = 1 
           AND StatusId = 1 
           AND IsApproved = 1 
           AND IsActive = 1 
    GROUP  BY BranchId 


 